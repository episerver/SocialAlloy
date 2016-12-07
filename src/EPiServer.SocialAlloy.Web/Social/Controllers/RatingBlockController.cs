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
            var ratingFormViewModel = new RatingFormViewModel(pageRouteHelper.PageLink, currentBlockLink);
            var ratingBlockViewModel = new RatingBlockViewModel(currentBlock, ratingFormViewModel);

            // Apply current model state to the rating block view model
            ApplyModelStateToRatingBlockViewModel(ratingBlockViewModel);

            // If user logged in, check if logged in user has already rated the page
            if (this.User.Identity.IsAuthenticated)
            {
                GetRating(target, ratingBlockViewModel);
            }

            // If there are no errors so far communicating with Social services, 
            // retrieve rating statistics for this page
            if (String.IsNullOrWhiteSpace(ratingBlockViewModel.ErrorMessage))
            {
                GetRatingStatistics(target, ratingBlockViewModel);
            }

            return PartialView("~/Views/Social/RatingBlock/RatingView.cshtml", ratingBlockViewModel);
        }

        /// <summary>
        /// Submit handles the submission of a new rating.  It accepts a rating form model,
        /// stores the submitted rating, and redirects back to the current page.
        /// </summary>
        /// <param name="ratingForm">The rating form being submitted.</param>
        /// <returns>The submit action result.</returns>
        [HttpPost]
        public ActionResult Submit(RatingFormViewModel ratingForm)
        {
            var data = this.contentRepository.Get<IContentData>(ratingForm.CurrentBlockLink);
            var blockData = data as RatingBlock;
            var ratingViewBlockModel = new RatingBlockViewModel(data as RatingBlock, ratingForm);

            if (!this.User.Identity.IsAuthenticated)
            {
                ratingViewBlockModel.SubmitErrorMessage = "Session timed out, you have to be logged in to submit your rating, please re-login and try again.";
            }
            else
            {
                if (IsValid(ratingForm.SubmittedRating))
                {
                    // Retrieve the page identifier of the page that was rated 
                    var pageId = this.pageRepository.GetPageId(ratingForm.CurrentPageLink);
                    if (!string.IsNullOrWhiteSpace(pageId))
                    {
                        var userId = userRepository.GetUserId(this.User);
                        if (String.IsNullOrWhiteSpace(userId))
                        {
                            ratingViewBlockModel.SubmitErrorMessage = "There was an error identifying the logged in user.  Please make sure you are logged in and try again.";
                        }
                        else
                        {
                            // Save the rating
                            AddRating(userId, pageId, ratingForm.SubmittedRating.Value, ratingViewBlockModel);

                            if (blockData.SendActivity)
                            {
                                //Add a rating activity
                                AddActivity(userId, pageId, ratingForm.SubmittedRating.Value, ratingViewBlockModel);
                            }
                        }
                    }
                    else
                    {
                        ratingViewBlockModel.SubmitErrorMessage = "The page id of this page could not be determined, please try again.";
                    }
                }
                else
                    ratingViewBlockModel.SubmitErrorMessage = "Please select a valid rating";
            }

            SaveModelState(ratingForm.CurrentBlockLink, CollectViewModelStateToSave(ratingViewBlockModel));

            return Redirect(UrlResolver.Current.GetUrl(ratingForm.CurrentPageLink));
        }

        /// <summary>
        /// Validates the rating value that was submitted 
        /// </summary>
        /// <param name="submittedRating">the rating value that was submitted</param>
        /// <returns>true if rating value is non-null, false otherwise</returns>
        private bool IsValid(int? submittedRating)
        {
            return submittedRating.HasValue;
        }

        /// <summary>
        /// Collects view model state that needs to be saved.
        /// </summary>
        /// <param name="ratingBlockViewModel">the RatingBlockViewModel containing the state to save</param>
        /// <returns>The dictionary containing the posted form state</returns>
        private ModelStateDictionary CollectViewModelStateToSave(RatingBlockViewModel ratingBlockViewModel)
        {
            var transientState = new ModelStateDictionary
            {
                new KeyValuePair<string, ModelState>
                (
                    "SubmitSuccessMessage",
                    new ModelState() {
                        Value = new ValueProviderResult(ratingBlockViewModel.SubmitSuccessMessage, ratingBlockViewModel.SubmitSuccessMessage, CultureInfo.CurrentCulture)
                    }
                ),
                new KeyValuePair<string, ModelState>
                (
                    "SubmitErrorMessage",
                    new ModelState() {
                        Value = new ValueProviderResult(ratingBlockViewModel.SubmitErrorMessage, ratingBlockViewModel.SubmitErrorMessage, CultureInfo.CurrentCulture)
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
        /// Gets the rating for the logged in user
        /// </summary>
        /// <param name="target">The current page on which the RatingBlock resides</param>
        /// <param name="ratingViewBlockModel">a reference to the RatingBlockViewModel to 
        /// populate with rating for the logged in user and errors, if any</param>
        private void GetRating(string target, RatingBlockViewModel ratingViewBlockModel)
        {
            ratingViewBlockModel.CurrentRating = null;
            ratingViewBlockModel.ErrorMessage = String.Empty;

            try
            {
                var userId = userRepository.GetUserId(this.User);
                if (!String.IsNullOrWhiteSpace(userId))
                {
                    ratingViewBlockModel.CurrentRating =
                        this.ratingRepository.GetRating(new SocialRatingFilter
                        {
                            Rater = userId,
                            Target = target
                        });
                }
                else
                {
                    ratingViewBlockModel.ErrorMessage = String.Format("There was an error identifying the logged in user. Please make sure you are logged in and try again.");
                }
            }
            catch (SocialRepositoryException ex)
            {
                ratingViewBlockModel.ErrorMessage = ex.Message;
            }
        }

        /// <summary>
        /// Gets the rating statistics for the page on which the RatingBlock resides
        /// </summary>
        /// <param name="target">The current page on which the RatingBlock resides</param>
        /// <param name="ratingViewBlockModel">a reference to the RatingBlockViewModel to 
        /// populate with rating statistics for the current page and errors, if any</param>
        private void GetRatingStatistics(string target, RatingBlockViewModel ratingViewBlockModel)
        {
            ratingViewBlockModel.ErrorMessage = String.Empty;
            ratingViewBlockModel.NoStatisticsFoundMessage = String.Empty;

            try
            {
                var result = ratingRepository.GetRatingStatistics(target);
                if (result != null)
                { 
                    ratingViewBlockModel.Average = result.Average;
                    ratingViewBlockModel.TotalCount = result.TotalCount;
                }
                else
                {
                    ratingViewBlockModel.NoStatisticsFoundMessage = "This page not been rated, be the first one to rate this page!";
                }
            }
            catch (SocialRepositoryException ex)
            {
                ratingViewBlockModel.ErrorMessage = ex.Message;
            }
        }

        /// <summary>
        /// Adds the rating submitted by the logged in user
        /// </summary>
        /// <param name="userId">The user who submitted the rating</param>
        /// <param name="target">The current page on which the RatingBlock resides</param>
        /// <param name="value">The value of the submitted rating</param>
        /// <param name="ratingViewBlockModel">a reference to the RatingBlockViewModel to 
        /// populate with errors, if any</param>
        private void AddRating(string userId, string target, int value, RatingBlockViewModel ratingViewBlockModel)
        {
            ratingViewBlockModel.SubmitErrorMessage = String.Empty;
            ratingViewBlockModel.SubmitSuccessMessage = String.Empty;

            try
            {
                ratingRepository.AddRating(userId, target, value);
                ratingViewBlockModel.SubmitSuccessMessage = "Thank you for submitting your rating!";
            }
            catch (SocialRepositoryException ex)
            {
                ratingViewBlockModel.SubmitErrorMessage = ex.Message;
            }
        }

        /// <summary>
        /// Adds an activity corresponding to the rating submitted action by the logged in user
        /// </summary>
        /// <param name="actor">The user who initated the activity</param>
        /// <param name="target">The current page on which the RatingBlock resides</param>
        /// <param name="value">The value of the submitted rating</param>
        /// <param name="ratingViewBlockModel">a reference to the RatingBlockViewModel to 
        /// populate with errors, if any</param>
        private void AddActivity(string actor, string target, int value, RatingBlockViewModel ratingViewBlockModel)
        {
            ratingViewBlockModel.SubmitErrorMessage = String.Empty;

            try
            {
                var activity = new SocialRatingActivity { Value = value };
                activityRepository.Add(actor, target, activity);
            }
            catch (SocialRepositoryException ex)
            {
                ratingViewBlockModel.SubmitErrorMessage = ex.Message;
            }
        }

        /// <summary>
        /// Applies current model state to the rating block view model.
        /// </summary>
        /// <param name="ratingBlockViewModel">The rating block view model to apply model state to.</param>
        private void ApplyModelStateToRatingBlockViewModel(RatingBlockViewModel ratingBlockViewModel)
        {
            // Get success/error model state
            var submitErrorMessage = GetModelState("SubmitErrorMessage");
            var submitSuccessMessage = GetModelState("SubmitSuccessMessage");
            var submitActivityErrorMessage = GetModelState("SubmitActivityErrorMessage");

            // Apply success/error model state to the view model
            ratingBlockViewModel.SubmitErrorMessage = (submitErrorMessage != null && submitErrorMessage.Value != null)
                                            ? (string)submitErrorMessage.Value.RawValue
                                            : string.Empty;

            
            ratingBlockViewModel.SubmitSuccessMessage = (submitSuccessMessage != null && submitSuccessMessage.Value != null)
                                            ? (string)submitErrorMessage.Value.RawValue
                                            : string.Empty;
        }
    }
}