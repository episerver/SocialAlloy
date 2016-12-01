using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Social.Common;
using EPiServer.Social.Ratings.Core;
using EPiServer.SocialAlloy.Web.Social.Blocks;
using EPiServer.SocialAlloy.Web.Social.Common.Controllers;
using EPiServer.SocialAlloy.Web.Social.Models;
using EPiServer.SocialAlloy.Web.Social.Repositories;
using EPiServer.Web.Routing;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace EPiServer.SocialAlloy.Web.Social.Controllers
{
    /// <summary>
    /// The RatingBlockViewController handles the rendering the rating statistics frontend view as well
    /// as the posting of a new rating.
    /// </summary>
    public class RatingBlockController : SocialBlockController<RatingBlock>
    {
        private readonly IRatingService ratingService;
        private readonly IContentRepository contentRepository;
        private readonly IPageRouteHelper pageRouteHelper;

        public RatingBlockController()
        {
            this.ratingService = ServiceLocator.Current.GetInstance<IRatingService>();
            this.contentRepository = ServiceLocator.Current.GetInstance<IContentRepository>();
            this.pageRouteHelper = ServiceLocator.Current.GetInstance<IPageRouteHelper>();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ratingService"></param>
        /// <param name="contentRepository"></param>
        public RatingBlockController(IRatingService ratingService, IContentRepository contentRepository, IPageRouteHelper pageRouteHelper)
        {
            this.ratingService = ratingService;
            this.contentRepository = contentRepository;
            this.pageRouteHelper = pageRouteHelper;
        }

        /// <summary>
        /// Render the rating block frontend view.
        /// </summary>
        /// <param name="currentBlock">The current frontend block instance.</param>
        /// <returns></returns>
        public override ActionResult Index(RatingBlock currentBlock)
        {
            var currentBlockLink = ((IContent)currentBlock).ContentLink;
            var target = pageRouteHelper.Page.ContentGuid.ToString();

            // Restore the saved model state
            LoadModelState(currentBlockLink);

            //Populate the view model
            var formModel = new RatingFormViewModel(pageRouteHelper.PageLink, currentBlockLink);
            var ratingViewBlockModel = new RatingBlockViewModel(currentBlock, formModel);

            // Set model state from saved model state/prior form submission
            var submitErrorMessage = GetModelState("SubmitErrorMessage");
            var submitSuccessMessage = GetModelState("SubmitSuccessMessage");
            ratingViewBlockModel.SubmitErrorMessage = (submitErrorMessage != null && submitErrorMessage.Value != null)
                                            ? submitErrorMessage.Value.AttemptedValue
                                            : string.Empty;
            ratingViewBlockModel.SubmitSuccessMessage = (submitSuccessMessage != null && submitSuccessMessage.Value != null)
                                            ? submitSuccessMessage.Value.AttemptedValue
                                            : string.Empty;

            //If user logged in, check if logged in user has already rated the page
            if (this.User.Identity.IsAuthenticated)
            {
                GetRating(target, ratingViewBlockModel);
            }

            //Check if there are any rating statistics for the page
            if (String.IsNullOrWhiteSpace(ratingViewBlockModel.ErrorMessage))
            {
                GetRatingStatistics(target, ratingViewBlockModel);
            }

            return PartialView("~/Views/Social/RatingBlock/RatingView.cshtml", ratingViewBlockModel);
        }

        /// <summary>
        /// Submit handles the submitting of new ratings.  It accepts a rating form model,
        /// stores the submitted rating, and redirects back to the current page.
        /// </summary>
        /// <param name="ratingForm">The rating form being submitted.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Submit(RatingFormViewModel ratingForm)
        {
            var data = this.contentRepository.Get<IContentData>(ratingForm.CurrentBlockLink);
            var ratingViewBlockModel = new RatingBlockViewModel(data as RatingBlock, ratingForm);

            if (!this.User.Identity.IsAuthenticated)
            {
                ratingViewBlockModel.SubmitErrorMessage = "Session timed out, You have to be logged in to submit your rating, please re-login and try again.";
                return Redirect(UrlResolver.Current.GetUrl(ratingForm.CurrentPageLink));
            }

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

            SaveModelState(ratingForm.CurrentBlockLink, CollectViewModelStateToSave(ratingViewBlockModel));

            return Redirect(UrlResolver.Current.GetUrl(ratingForm.CurrentPageLink));
        }

        private string GetPageId(PageReference pageLink)
        {
            var pageData = contentRepository.Get<PageData>(pageLink as ContentReference);
            return pageData != null ? pageData.ContentGuid.ToString() : String.Empty;
        }

        private void AddRating(string target, int value, RatingBlockViewModel ratingViewBlockModel)
        {
            ratingViewBlockModel.SubmitErrorMessage = String.Empty;
            ratingViewBlockModel.SubmitSuccessMessage = String.Empty;

            try
            {
                var userService = ServiceLocator.Current.GetInstance<IUserRepository>();
                var userId = userService.GetUserId(this.User);
                if (!string.IsNullOrWhiteSpace(userId))
                {
                    var result = ratingService.Add(new Rating(
                                        Reference.Create(userId),
                                        Reference.Create(target),
                                        new RatingValue(value))
                    );

                    ratingViewBlockModel.SubmitSuccessMessage = "Thank you for submitting your rating!";
                }
                else
                {
                    ratingViewBlockModel.SubmitErrorMessage = "There was an error identifying the logged in user, please re-login and try again.";
                }
            }
            catch (Exception e)
            {
                ratingViewBlockModel.SubmitErrorMessage = String.Format("Error adding rating. Exception Type: {0} Message: {1}", e.GetType().Name, e.Message);
            }
        }

        private bool IsValid(int? submittedRating)
        {
            return submittedRating.HasValue;
        }

        /// <summary>
        /// Collects view model state that needs to be saved.
        /// </summary>
        /// <param name="commentsViewModel"></param>
        /// <returns></returns>
        private ModelStateDictionary CollectViewModelStateToSave(RatingBlockViewModel ratingBlockViewModel)
        {
            var transientState = new ModelStateDictionary
            {
                new System.Collections.Generic.KeyValuePair<string, System.Web.Mvc.ModelState>
                (
                    "SubmitSuccessMessage",
                    new System.Web.Mvc.ModelState() {
                        Value = new ValueProviderResult(ratingBlockViewModel.SubmitSuccessMessage, ratingBlockViewModel.SubmitSuccessMessage, CultureInfo.CurrentCulture)
                    }
                ),
                new System.Collections.Generic.KeyValuePair<string, System.Web.Mvc.ModelState>
                (
                    "SubmitErrorMessage",
                    new System.Web.Mvc.ModelState() {
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

        private void GetRating(string target, RatingBlockViewModel ratingViewBlockModel)
        {
            ratingViewBlockModel.CurrentRating = null;
            ratingViewBlockModel.ErrorMessage = String.Empty;

            try
            {
                var userService = ServiceLocator.Current.GetInstance<IUserRepository>();
                var userId = userService.GetUserId(this.User);
                if (!string.IsNullOrWhiteSpace(userId))
                {
                    var result = ratingService.Get(new Criteria<RatingFilter>()
                    {
                        Filter = new RatingFilter()
                        {
                            Rater = Reference.Create(userId),
                            Targets = new List<Reference> { Reference.Create(target) }
                        },
                        PageInfo = new PageInfo() { PageSize = 1 }
                    });

                    //Remove this, this is only to test this for Comments
                    var user = userService.GetUser(userId);

                    if (result.Results.Count() > 0)
                        ratingViewBlockModel.CurrentRating = result.Results.ToList().FirstOrDefault().Value.Value;
                }
            }
            catch (Exception e)
            {
                ratingViewBlockModel.ErrorMessage = String.Format("Error getting user rating. Exception Type: {0} Message: {1}", e.GetType().Name, e.Message);
            }
        }

        private void GetRatingStatistics(string target, RatingBlockViewModel ratingViewBlockModel)
        {
            ratingViewBlockModel.ErrorMessage = String.Empty;
            ratingViewBlockModel.NoStatisticsFoundMessage = String.Empty;

            try
            {
                var result = ratingService.Get(new Criteria<RatingStatisticsFilter>()
                {
                    Filter = new RatingStatisticsFilter()
                    {
                        Targets = new List<Reference> { Reference.Create(target) }
                    },
                    PageInfo = new PageInfo() { PageSize = 1 }
                });

                if (result.Results.Count() > 0)
                {
                    var statistics = result.Results.ToList().FirstOrDefault();
                    ratingViewBlockModel.Average = Decimal.Divide(statistics.Sum, statistics.TotalCount);
                    ratingViewBlockModel.TotalCount = statistics.TotalCount;
                }
                else
                {
                    ratingViewBlockModel.NoStatisticsFoundMessage = "Be the first one to rate this page!";
                }
            }
            catch (Exception e)
            {
                ratingViewBlockModel.ErrorMessage = String.Format("Error getting rating statistics. Exception Type: {0} Message: {1}", e.GetType().Name, e.Message);
            }
        }
    }
}
