using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.SocialAlloy.Web.Social.Blocks;
using EPiServer.SocialAlloy.Web.Social.Common.Controllers;
using EPiServer.SocialAlloy.Web.Social.Common.Exceptions;
using EPiServer.SocialAlloy.Web.Social.Models;
using EPiServer.SocialAlloy.Web.Social.Repositories;
using EPiServer.Web.Routing;
using System;
using System.Collections.Generic;
using System.Globalization;
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
            var currentBlockLink = ((IContent)currentBlock).ContentLink;
            var target = pageRouteHelper.Page.ContentGuid.ToString();

            // Restore the saved model state
            LoadModelState(currentBlockLink);

            // Create a rating block view model to fill the frontend block view
            var formModel = new RatingFormViewModel(pageRouteHelper.PageLink, currentBlockLink);
            var blockModel = new RatingBlockViewModel(currentBlock, formModel);

            // Apply current model state to the rating block view model
            ApplyModelStateToRatingBlockViewModel(blockModel);

            // If user logged in, check if logged in user has already rated the page
            if (this.User.Identity.IsAuthenticated)
            {
                GetRating(target, blockModel);
            }

            // If there are no errors so far communicating with Social services, 
            // retrieve rating statistics for this page
            if (String.IsNullOrWhiteSpace(blockModel.ErrorMessage))
            {
                GetRatingStatistics(target, blockModel);
            }

            return PartialView("~/Views/Social/RatingBlock/RatingView.cshtml", blockModel);
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
            var currentBlock = this.contentRepository.Get<IContentData>(ratingForm.CurrentBlockLink) as RatingBlock;
            var blockModel = new RatingBlockViewModel(currentBlock, ratingForm);

            ValidateSubmitRatingForm(ratingForm, blockModel);

            if (String.IsNullOrEmpty(blockModel.SubmitErrorMessage))
            {
                // Add the rating
                AddRating(ratingForm.SubmittedRating.Value, blockModel);

                if (currentBlock.SendActivity)
                {
                    // Add a rating activity
                    AddActivity(ratingForm.SubmittedRating.Value, blockModel);
                }
            }

            SaveModelState(ratingForm.CurrentBlockLink, CollectViewModelStateToSave(blockModel));

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
            blockModel.ErrorMessage = String.Empty;

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
                    blockModel.ErrorMessage = String.Format("There was an error identifying the logged in user. Please make sure you are logged in and try again.");
                }
            }
            catch (SocialRepositoryException ex)
            {
                blockModel.ErrorMessage = ex.Message;
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
            blockModel.ErrorMessage = String.Empty;
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
                    blockModel.NoStatisticsFoundMessage = "This page not been rated, be the first one to rate this page!";
                }
            }
            catch (SocialRepositoryException ex)
            {
                blockModel.ErrorMessage = ex.Message;
            }
        }

        /// <summary>
        /// Adds the rating submitted by the logged in user
        /// </summary>
        /// <param name="value">The value of the submitted rating</param>
        /// <param name="blockModel">a reference to the RatingBlockViewModel to 
        /// populate with errors, if any</param>
        private void AddRating(int value, RatingBlockViewModel blockModel)
        {
            blockModel.SubmitErrorMessage = String.Empty;
            blockModel.SubmitSuccessMessage = String.Empty;

            try
            {
                ratingRepository.AddRating(this.userId, this.pageId, value);
                blockModel.SubmitSuccessMessage = "Thank you for submitting your rating!";
            }
            catch (SocialRepositoryException ex)
            {
                blockModel.SubmitErrorMessage = ex.Message;
            }
        }

        /// <summary>
        /// Adds an activity corresponding to the rating submitted action by the logged in user
        /// </summary>
        /// <param name="value">The value of the submitted rating</param>
        /// <param name="blockModel">a reference to the RatingBlockViewModel to 
        /// populate with errors, if any</param>
        private void AddActivity(int value, RatingBlockViewModel blockModel)
        {
            blockModel.SubmitErrorMessage = String.Empty;

            try
            {
                var activity = new SocialRatingActivity { Value = value };
                activityRepository.Add(this.userId, this.pageId, activity);
            }
            catch (SocialRepositoryException ex)
            {
                blockModel.SubmitErrorMessage = ex.Message;
            }
        }

        /// <summary>
        /// Validates the rating that was submitted.
        /// </summary>
        /// <param name="ratingForm">The rating form that was submitted.</param>
        /// <param name="blockModel">a reference to the RatingBlockViewModel to 
        /// populate with validation errors, if any</param>
        private void ValidateSubmitRatingForm(RatingFormViewModel ratingForm, RatingBlockViewModel blockModel)
        {
            blockModel.SubmitErrorMessage = String.Empty;

            // Validate user is logged in
            if (!this.User.Identity.IsAuthenticated)
            {
                blockModel.SubmitErrorMessage = "Session timed out, you have to be logged in to submit your rating. Please login and try again.";
            }
            else
            {
                // Validate a rating was submitted
                if (!ratingForm.SubmittedRating.HasValue)
                {
                    blockModel.SubmitErrorMessage = "Please select a valid rating";
                }
                else
                {
                    // Retrieve and validate the page identifier of the page that was rated 
                    this.pageId = this.pageRepository.GetPageId(ratingForm.CurrentPageLink);
                    if (String.IsNullOrWhiteSpace(this.pageId))
                    {
                        blockModel.SubmitErrorMessage = "The page id of this page could not be determined. Please try rating this page again.";
                    }
                    else
                    {
                        // Retrieve and validate the user identifier of the rater
                        this.userId = userRepository.GetUserId(this.User);
                        if (String.IsNullOrWhiteSpace(this.userId))
                        {
                            blockModel.SubmitErrorMessage = "There was an error identifying the logged in user. Please make sure you are logged in and try again.";
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Collects view model state that needs to be saved.
        /// </summary>
        /// <param name="blockModel">the RatingBlockViewModel containing the state to save</param>
        /// <returns>The dictionary containing the posted form state</returns>
        private ModelStateDictionary CollectViewModelStateToSave(RatingBlockViewModel blockModel)
        {
            var transientState = new ModelStateDictionary
            {
                new KeyValuePair<string, ModelState>
                (
                    "SubmitSuccessMessage",
                    new ModelState() {
                        Value = new ValueProviderResult(blockModel.SubmitSuccessMessage, blockModel.SubmitSuccessMessage, CultureInfo.CurrentCulture)
                    }
                ),
                new KeyValuePair<string, ModelState>
                (
                    "SubmitErrorMessage",
                    new ModelState() {
                        Value = new ValueProviderResult(blockModel.SubmitErrorMessage, blockModel.SubmitErrorMessage, CultureInfo.CurrentCulture)
                    }
                )
            };

            var modelState = ViewData.ModelState;
            if (transientState != null)
            {
                modelState.Merge(transientState);
            }

            return modelState;
        }

        /// <summary>
        /// Applies current model state to the rating block view model.
        /// </summary>
        /// <param name="blockModel">The rating block view model to apply model state to.</param>
        private void ApplyModelStateToRatingBlockViewModel(RatingBlockViewModel blockModel)
        {
            // Get success/error model state
            var submitErrorMessage = GetModelState("SubmitErrorMessage");
            var submitSuccessMessage = GetModelState("SubmitSuccessMessage");

            // Apply success/error model state to the view model
            blockModel.SubmitErrorMessage = (submitErrorMessage != null && submitErrorMessage.Value != null)
                                            ? (string)submitErrorMessage.Value.RawValue
                                            : string.Empty;

            
            blockModel.SubmitSuccessMessage = (submitSuccessMessage != null && submitSuccessMessage.Value != null)
                                            ? (string)submitErrorMessage.Value.RawValue
                                            : string.Empty;
        }
    }
}