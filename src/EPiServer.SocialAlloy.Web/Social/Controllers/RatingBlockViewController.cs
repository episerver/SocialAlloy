using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.SocialAlloy.Web.Social.Blocks;
using EPiServer.SocialAlloy.Web.Social.Common.Controllers;
using EPiServer.SocialAlloy.Web.Social.Models;
using EPiServer.Web.Routing;
using System.Web.Mvc;

namespace EPiServer.SocialAlloy.Web.Social.Controllers
{
    /// <summary>
    /// The RatingBlockViewController handles the rendering the rating statistics frontend view as well
    /// as the posting of a new rating.
    /// </summary>
    public class RatingBlockViewController : SocialBlockController<RatingBlock>
    {
        /// <summary>
        /// Render the rating block frontend view.
        /// </summary>
        /// <param name="currentBlock">The current frontend block instance.</param>
        /// <returns></returns>
        public override ActionResult Index(RatingBlock currentBlock)
        {
            var pageRouteHelper = ServiceLocator.Current.GetInstance<IPageRouteHelper>();
            var currentBlockLink = ((IContent)currentBlock).ContentLink;
            var target = pageRouteHelper.PageLink.ID;

            // Include some context in the rating form view rendering so that when new rating
            // is submitted, we have this context handy to save the newly submitted rating.
            var model = new RatingFormViewModel(pageRouteHelper.PageLink, currentBlockLink);

            if (this.User.Identity.IsAuthenticated)
            {
                model.Rater = this.User.Identity.Name; //Do we need to get UserId here instead of name??
                //fetch Rating from Social API for rater
                //if (rating found)
                    //model.CurrentRating = 4;
            }

            //Fetch rating statistics for target and set values in the viewmodel.
            model.Average = 3.5m;
            model.TotalCount = 5;
            var ratingBlockViewModel = new RatingBlockViewModel(currentBlock, model);

            return PartialView("~/Views/Social/RatingBlock/RatingView.cshtml", ratingBlockViewModel);
        }

        /// <summary>
        /// Submit handles the submitting of new ratings.  It accepts a rating form model,
        /// stores the submitted rating, and redirects back to the current page.
        /// </summary>
        /// <param name="ratingForm">The rating form being submitted.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Submit(RatingPostModel ratingForm)
        {
            // TODO:  validate/store the rating here


            return Redirect(UrlResolver.Current.GetUrl(ratingForm.CurrentPageLink));
        }
    }
}
