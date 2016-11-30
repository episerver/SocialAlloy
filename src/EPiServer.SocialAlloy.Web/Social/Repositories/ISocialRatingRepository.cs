using EPiServer.SocialAlloy.Web.Social.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPiServer.SocialAlloy.Web.Social.Repositories
{
    interface ISocialRatingRepository
    {
        int? GetRating(SocialRatingFilter filter);

        SocialRatingStatistics GetRatingStatistics(string target);

        void AddRating(string user, string target, int value);
    }
}
