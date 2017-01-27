using EPiServer.ServiceLocation;
using EPiServer.Social.Common;
using EPiServer.Social.Ratings.Core;
using EPiServer.SocialAlloy.Web.Social.Blocks;
using EPiServer.SocialAlloy.Web.Social.Models;
using EPiServer.Web.Mvc;
using EPiServer.Web.Routing;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace EPiServer.SocialAlloy.Web.Social.Controllers
{
    /// <summary>
    /// The LikeButtonBlockController is a simple implementation of a Like button using
    /// the EPiServer Social Ratings service API.  In this simplistic implementation we 
    /// assume that if the user is not logged in that some anonymous user can like the page
    /// indefinitely; not practical but should help to illustrate rating statistics
    /// as well as keep things simpler without going into managing user authentication.
    /// </summary>
    public class LikeButtonBlockController : BlockController<LikeButtonBlock>
    {
        private readonly IRatingService ratingService;
        private readonly IPageRouteHelper pageRouteHelper;
        private readonly IContentRepository contentRepository;
        private const int LIKED_RATING = 1;

        /// <summary>
        /// Constructor
        /// </summary>
        public LikeButtonBlockController()
        {
            // This is all wired up by the installation of the EPiServer.Social.Ratings.Site package
            this.ratingService = ServiceLocator.Current.GetInstance<IRatingService>();

            // This is wired up by EPiServer Core/Framework
            this.contentRepository = ServiceLocator.Current.GetInstance<IContentRepository>();
            this.pageRouteHelper = ServiceLocator.Current.GetInstance<IPageRouteHelper>();
        }

        /// <summary>
        /// Render the Like button block frontend view.
        /// </summary>
        /// <param name="currentBlock">The current block instance.</param>
        /// <returns>Result of the redirect to the current page.</returns>
        public override ActionResult Index(LikeButtonBlock currentBlock)
        {
            var targetPage = pageRouteHelper.Page.ContentGuid.ToString();
            var raterUser = this.User.Identity.GetUserId();
            if (string.IsNullOrEmpty(raterUser)) raterUser = "anonymous-" + Guid.NewGuid();

            // Create a rating block view model to fill the frontend block view
            var blockModel = new LikeButtonBlockViewModel();

            try
            {
                // Using the EPiServer Social Rating service, get the existing rating for the current user (rater) and page (target).
                // Again, if there's no user logged into the site then we generate a random anonymous user, which should not currently 
                // have any ratings, thus he/she will always be allow to rate the current page.
                var ratingPage = ratingService.Get(
                    new Criteria<RatingFilter>()
                    {
                        Filter = new RatingFilter()
                        {
                            Rater = Reference.Create(raterUser),
                            Targets = new List<Reference> { Reference.Create(targetPage) }
                        },
                        PageInfo = new PageInfo() { PageSize = 1 }
                    }
                );

                // Add the current Like rating, if any, to the block view model. If the user is logged 
                // into the site and had previously liked the current page then the CurrentRating value
                // should be 1 (LIKED_RATING).  Anonymous user Likes are generated with unique random users
                // and thus the current anonymous user will never see a current rating value as he/she
                // can Like the page indefinitely.
                if (ratingPage.Results.Count() > 0)
                {
                    blockModel.CurrentRating = ratingPage.Results.ToList().FirstOrDefault().Value.Value;
                }

                // Using the EPiServer Social Rating service, get the existing Like statistics for the page (target)
                var ratingStatisticsPage = ratingService.Get(
                    new Criteria<RatingStatisticsFilter>()
                    {
                        Filter = new RatingStatisticsFilter()
                        {
                            Targets = new List<Reference> { Reference.Create(targetPage) }
                        },
                        PageInfo = new PageInfo() { PageSize = 1 }
                    }
                );

                // Add the page Like statistics to the block view model
                if (ratingStatisticsPage.Results.Count() > 0)
                {
                    var statistics = ratingStatisticsPage.Results.ToList().FirstOrDefault();
                    if (statistics.TotalCount > 0)
                    {
                        blockModel.TotalCount = statistics.TotalCount;
                    }
                }
            }
            catch (Exception ex)
            {
                // the rating service may throw a number of possible exceptions
                // should handle each one accordingly -- see rating service documentation
            }

            return PartialView("~/Views/Social/LikeButtonBlock/LikeButtonView.cshtml", blockModel);
        }

        /// <summary>
        /// Submit handles a click of the Like button.  It accepts a Like button block model,
        /// saves the Like rating, and redirects back to the current page.
        /// </summary>
        /// <param name="likeButtonBlock">The Like button block model.</param>
        /// <returns>Result of the redirect to the current page.</returns>
        [HttpPost]
        public ActionResult Submit(LikeButtonBlockViewModel likeButtonBlock)
        {
            var pageLink = this.pageRouteHelper.PageLink;

            var targetPage = this.pageRouteHelper.Page.ContentGuid.ToString();
            var raterUser = this.User.Identity.GetUserId();
            if (string.IsNullOrEmpty(raterUser)) raterUser = "anonymous-" + Guid.NewGuid();

            try
            {
                // Add the rating using the EPiServer Social Rating service
                var addedRating = ratingService.Add(
                    new Rating(
                        Reference.Create(raterUser),
                        Reference.Create(targetPage),
                        new RatingValue(LIKED_RATING)
                    )
                );
            }
            catch (Exception ex)
            {
                // the rating service may throw a number of possible exceptions
                // should handle each one accordingly -- see rating service documentation
            }

            return Redirect(UrlResolver.Current.GetUrl(pageLink));
        }
    }
}