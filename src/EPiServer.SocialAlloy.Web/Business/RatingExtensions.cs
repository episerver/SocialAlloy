using EPiServer.ServiceLocation;
using EPiServer.Social.Common;
using EPiServer.Social.Ratings.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPiServer.SocialAlloy.Web.Business
{
    public static class RatingExtensions
    {
        public static void Rate(this Reference user, Reference target, int userRating)
        {
            var ratingService = ServiceLocator.Current.GetInstance<IRatingService>();

            // build a Rating object
            var rating = new RatingValue(userRating);

            // use the rating service to apply the rating to the target
            ratingService.Add(new Rating(user, target, rating));
        }

        public static RatingStatistics GetStatistics(this Reference target)
        {
            var ratingService = ServiceLocator.Current.GetInstance<IRatingService>();

            var result = ratingService.Get(new Criteria<RatingStatisticsFilter>()
            {
                Filter = new RatingStatisticsFilter()
                {
                    Targets = new List<Reference> { target }
                },
                PageInfo = new PageInfo() { PageSize = 1 }
            });

            return (result.Results.Count() > 0) ? result.Results.ToList().FirstOrDefault() : null;
        }

        public static Rating GetRating(this Reference user, Reference target)
        {
            var ratingService = ServiceLocator.Current.GetInstance<IRatingService>();

            var result = ratingService.Get(new Criteria<RatingFilter>()
            {
                Filter = new RatingFilter()
                {
                    Rater = user,
                    Targets = new List<Reference> { target }
                },
                PageInfo = new PageInfo() { PageSize = 1 }
            });

            return (result.Results.Count() > 0) ? result.Results.ToList().FirstOrDefault() : null;
        }
    }
}