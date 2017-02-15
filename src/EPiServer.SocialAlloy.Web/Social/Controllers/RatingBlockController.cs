using EPiServer.ServiceLocation;
using EPiServer.SocialAlloy.Web.Social.Blocks;
using EPiServer.SocialAlloy.Web.Social.Common.Controllers;
using EPiServer.SocialAlloy.Web.Social.Common.Exceptions;
using EPiServer.SocialAlloy.Web.Social.Common.Models;
using EPiServer.SocialAlloy.Web.Social.Models;
using EPiServer.SocialAlloy.Web.Social.Repositories;
using EPiServer.Web.Routing;
using System;
using System.Linq;
using System.Web.Mvc;

namespace EPiServer.SocialAlloy.Web.Social.Controllers
{
    /// <summary>
    /// The RatingBlockController handles the rendering of any existing rating statistics
    /// for the page on which the RatingBlock resides.
    /// This controller also allows a logged in user to rate a page which that user has not
    /// yet rated or view the rating that user has already submitted in the past for that page.
    /// </summary>
    public class RatingBlockController : SocialBlockController<RatingBlock>
    {
        private readonly IUserRepository userRepository;
        private readonly ISocialRatingRepository ratingRepository;
        private readonly ISocialActivityRepository activityRepository;
        private readonly IPageRepository pageRepository;
        private string userId;
        private string pageId;
        private const string MessageKey = "RatingBlock";
        private const string ErrorMessage = "Error";
        private const string SuccessMessage = "Success";
        /// <summary>
        /// Constructor
        /// </summary>
        public RatingBlockController()
        {
            this.userRepository = ServiceLocator.Current.GetInstance<IUserRepository>();
            this.ratingRepository = ServiceLocator.Current.GetInstance<ISocialRatingRepository>();
            this.pageRepository = ServiceLocator.Current.GetInstance<IPageRepository>();
            this.activityRepository = ServiceLocator.Current.GetInstance<ISocialActivityRepository>();
        }

        /// <summary>
        /// Render the rating block frontend view.
        /// </summary>
        /// <param name="currentBlock">The current frontend block instance.</param>
        /// <returns>The index action result.</returns>
        public override ActionResult Index(RatingBlock currentBlock)
        {
            var target = pageRouteHelper.Page.ContentGuid.ToString();

            var currentPageLink = pageRouteHelper.PageLink;

            // Create a rating block view model to fill the frontend block view
            var blockModel = new RatingBlockViewModel(currentBlock, currentPageLink);

            //get messages for view
            blockModel.Messages = RetrieveMessages(MessageKey);

            // If user logged in, check if logged in user has already rated the page
            if (this.User.Identity.IsAuthenticated)
            {
                GetRating(target, blockModel);
            }

            //Conditionally retrieving ratingstatistics based on any errors that might have been encountered
            var noMessages = blockModel.Messages.Count == 0;
            var noErrors = blockModel.Messages.Any(x => x.Type != ErrorMessage);
            if (noMessages || noErrors)
            {
                GetRatingStatistics(target, blockModel);
            }

            return PartialView("~/Views/Social/RatingBlock/Index.cshtml", blockModel);
        }

        /// <summary>
        /// Submit handles the submission of a new rating.  It accepts a rating form model,
        /// stores the submitted rating, and redirects back to the current page.
        /// </summary>
        /// <param name="ratingForm">The rating form that was submitted.</param>
        /// <returns>The submit action result.</returns>
        [HttpPost]
        public ActionResult Submit(RatingFormViewModel ratingForm)
        {
            ValidateSubmitRatingForm(ratingForm);

            // Add the rating and verify success
            var addRatingSuccess = AddRating(ratingForm.SubmittedRating.Value);

            if (addRatingSuccess && ratingForm.SendActivity)
            {
                // Add a rating activity
                AddActivity(ratingForm.SubmittedRating.Value);
            }
            return Redirect(UrlResolver.Current.GetUrl(ratingForm.CurrentPageLink));
        }

        /// <summary>
        /// Gets the rating for the logged in user
        /// </summary>
        /// <param name="target">The current page on which the RatingBlock resides</param>
        /// <param name="blockModel">a reference to the RatingBlockViewModel to 
        /// populate with rating for the logged in user and errors, if any</param>
        private void GetRating(string target, RatingBlockViewModel blockModel)
        {
            blockModel.CurrentRating = null;

            try
            {
                var userId = userRepository.GetUserId(this.User);
                if (!String.IsNullOrWhiteSpace(userId))
                {
                    blockModel.CurrentRating =
                        this.ratingRepository.GetRating(new SocialRatingFilter
                        {
                            Rater = userId,
                            Target = target
                        });
                }
                else
                {
                    var message = "There was an error identifying the logged in user. Please make sure you are logged in and try again.";
                    blockModel.Messages.Add(new MessageViewModel(message, ErrorMessage));
                }
            }
            catch (SocialRepositoryException ex)
            {
                blockModel.Messages.Add(new MessageViewModel(ex.Message, ErrorMessage));
            }
        }

        /// <summary>
        /// Gets the rating statistics for the page on which the RatingBlock resides
        /// </summary>
        /// <param name="target">The current page on which the RatingBlock resides</param>
        /// <param name="blockModel">a reference to the RatingBlockViewModel to 
        /// populate with rating statistics for the current page and errors, if any</param>
        private void GetRatingStatistics(string target, RatingBlockViewModel blockModel)
        {
            blockModel.NoStatisticsFoundMessage = String.Empty;

            try
            {
                var result = ratingRepository.GetRatingStatistics(target);
                if (result != null)
                {
                    blockModel.Average = result.Average;
                    blockModel.TotalCount = result.TotalCount;
                }
                else
                {
                    var loggedInMessage = "This page has not been rated. Be the first!";
                    var loggedOutMessage ="This page has not been rated. Log in and be the first!" ;
                    blockModel.NoStatisticsFoundMessage = this.User.Identity.IsAuthenticated ? loggedInMessage : loggedOutMessage;
                }
            }
            catch (SocialRepositoryException ex)
            {
                blockModel.Messages.Add(new MessageViewModel(ex.Message, ErrorMessage));
            }
        }

        /// <summary>
        /// Adds the rating submitted by the logged in user
        /// </summary>
        /// <param name="value">The value of the submitted rating</param>
        private bool AddRating(int value)
        {
            try
            {
                ratingRepository.AddRating(this.userId, this.pageId, value);
                var message = "Thank you for submitting your rating!";
                AddMessage(MessageKey, new MessageViewModel(message, SuccessMessage));
                return true;
            }
            catch (SocialRepositoryException ex)
            {
                AddMessage(MessageKey, new MessageViewModel(ex.Message, ErrorMessage));
            }
            return false;
        }

        /// <summary>
        /// Adds an activity corresponding to the rating submitted action by the logged in user
        /// </summary>
        /// <param name="value">The value of the submitted rating</param>
        private void AddActivity(int value)
        {
            try
            {
                var activity = new SocialRatingActivity { Value = value };
                activityRepository.Add(this.userId, this.pageId, activity);
            }
            catch (SocialRepositoryException ex)
            {
                AddMessage(MessageKey, new MessageViewModel(ex.Message, ErrorMessage));
            }
        }

        /// <summary>
        /// Validates the rating that was submitted.
        /// </summary>
        /// <param name="ratingForm">The rating form that was submitted.</param>
        private void ValidateSubmitRatingForm(RatingFormViewModel ratingForm)
        {
            string message = string.Empty;
            // Validate user is logged in
            if (!this.User.Identity.IsAuthenticated)
            {
                message = "Session timed out, you have to be logged in to submit your rating. Please login and try again.";
            }
            else
            {
                // Validate a rating was submitted
                if (!ratingForm.SubmittedRating.HasValue)
                {
                    message = "Please select a valid rating";
                }
                else
                {
                    // Retrieve and validate the page identifier of the page that was rated 
                    this.pageId = this.pageRepository.GetPageId(ratingForm.CurrentPageLink);
                    if (String.IsNullOrWhiteSpace(this.pageId))
                    {
                        message = "The page id of this page could not be determined. Please try rating this page again.";
                    }
                    else
                    {
                        // Retrieve and validate the user identifier of the rater
                        this.userId = userRepository.GetUserId(this.User);
                        if (String.IsNullOrWhiteSpace(this.userId))
                        {
                            message = "There was an error identifying the logged in user. Please make sure you are logged in and try again.";
                        }
                    }
                }
            }
            AddMessage(MessageKey, new MessageViewModel(message, ErrorMessage));
        }
    }
}