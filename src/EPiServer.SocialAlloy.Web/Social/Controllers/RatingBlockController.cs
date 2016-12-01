﻿using EPiServer.Core;
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
    /// This controller also allows logged in users to submit ratings for pages they have not yet 
    /// rated or view any rating they have already submitted for a page.
    /// </summary>
    public class RatingBlockController : SocialBlockController<RatingBlock>
    {
        private readonly ISocialRatingRepository ratingRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        public RatingBlockController()
        {
            this.ratingRepository = ServiceLocator.Current.GetInstance<ISocialRatingRepository>();
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

            //Populate the view model
            var ratingFormViewModel = new RatingFormViewModel(pageRouteHelper.PageLink, currentBlockLink);
            var ratingBlockViewModel = new RatingBlockViewModel(currentBlock, ratingFormViewModel);

            // Apply current model state to the rating block view model
            ApplyModelStateToRatingBlockViewModel(ratingBlockViewModel);

            //If user logged in, check if logged in user has already rated the page
            if (this.User.Identity.IsAuthenticated)
            {
                GetRating(target, ratingBlockViewModel);
            }

            //If no errors communicating with Social services so far, 
            // retrieve rating statistics for this page
            if (String.IsNullOrWhiteSpace(ratingBlockViewModel.ErrorMessage))
            {
                GetRatingStatistics(target, ratingBlockViewModel);
            }

            return PartialView("~/Views/Social/RatingBlock/RatingView.cshtml", ratingBlockViewModel);
        }

        /// <summary>
        /// Submit handles the submitting of new ratings.  It accepts a rating form model,
        /// stores the submitted rating, and redirects back to the current page.
        /// </summary>
        /// <param name="ratingForm">The rating form being submitted.</param>
        /// <returns>The submit action result.</returns>
        [HttpPost]
        public ActionResult Submit(RatingFormViewModel ratingForm)
        {
            var data = this.contentRepository.Get<IContentData>(ratingForm.CurrentBlockLink);
            var ratingViewBlockModel = new RatingBlockViewModel(data as RatingBlock, ratingForm);

            if (!this.User.Identity.IsAuthenticated)
            {
                ratingViewBlockModel.SubmitErrorMessage = "Session timed out, you have to be logged in to submit your rating, please re-login and try again.";
            }
            else
            {
                // validate rating != null and was submitted, TODO UI shd validate too. submit button shd be enabled iff 1 radio btn is selected
                if (IsValid(ratingForm.SubmittedRating))
                {
                    //Get the page Id 
                    var pageId = GetPageId(ratingForm.CurrentPageLink);
                    if (!string.IsNullOrWhiteSpace(pageId))
                    {
                        //save rating
                        AddRating(pageId, ratingForm.SubmittedRating.Value, ratingViewBlockModel);
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
        /// Ensures the user submitted a valid rating
        /// </summary>
        /// <param name="submittedRating"></param>
        /// <returns></returns>
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
        /// populate with errors, if any</param>
        private void GetRating(string target, RatingBlockViewModel ratingViewBlockModel)
        {
            ratingViewBlockModel.CurrentRating = null;
            ratingViewBlockModel.ErrorMessage = String.Empty;

            try
            {
                var userService = ServiceLocator.Current.GetInstance<IUserRepository>();
                var userId = userService.GetUserId(this.User);
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
        /// populate with errors, if any</param>
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
        /// <param name="target">The current page on which the RatingBlock resides</param>
        /// <param name="value">The value of the submitted rating</param>
        /// <param name="ratingViewBlockModel">a reference to the RatingBlockViewModel to 
        /// populate with errors, if any</param>
        private void AddRating(string target, int value, RatingBlockViewModel ratingViewBlockModel)
        {
            ratingViewBlockModel.SubmitErrorMessage = String.Empty;
            ratingViewBlockModel.SubmitSuccessMessage = String.Empty;

            try
            {
                var userService = ServiceLocator.Current.GetInstance<IUserRepository>();
                var userId = userService.GetUserId(this.User);
                if (!String.IsNullOrWhiteSpace(userId))
                {
                    ratingRepository.AddRating(userId, target, value);

                    ratingViewBlockModel.SubmitSuccessMessage = "Thank you for submitting your rating!";
                }
                else
                {
                    ratingViewBlockModel.SubmitErrorMessage = "There was an error identifying the logged in user.  Please make sure you are logged in and try again.";
                }
            }
            catch (SocialRepositoryException ex)
            {
                ratingViewBlockModel.SubmitErrorMessage = ex.Message; ;
            }
        }

        /// <summary>
        /// Applies current model state to the rating block view model.
        /// </summary>
        /// <param name="ratingBlockViewModel">The rating block view model to apply model state to.</param>
        private void ApplyModelStateToRatingBlockViewModel(RatingBlockViewModel ratingBlockViewModel)
        {
            // Set model state from saved model state/prior form submission
            var submitErrorMessage = GetModelState("SubmitErrorMessage");
            var submitSuccessMessage = GetModelState("SubmitSuccessMessage");

            ratingBlockViewModel.SubmitErrorMessage = (submitErrorMessage != null && submitErrorMessage.Value != null)
                                            ? submitErrorMessage.Value.AttemptedValue
                                            : string.Empty;
            ratingBlockViewModel.SubmitSuccessMessage = (submitSuccessMessage != null && submitSuccessMessage.Value != null)
                                            ? submitSuccessMessage.Value.AttemptedValue
                                            : string.Empty;
        }
    }
}