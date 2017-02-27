using EPiServer.SocialAlloy.Web.Social.Models;
using System.Collections.Generic;

namespace EPiServer.SocialAlloy.Web.Social.Repositories
{
    /// <summary>
    /// The ICommunityFeedRepository interface defines the operations for accessing feeds of community activities. 
    /// </summary>
    public interface ICommunityFeedRepository
    {
        /// <summary>
        ///  Retrieves feed items based on the specified filter.
        /// </summary>
        /// <param name="filter">A feed item filter</param>
        /// <returns>A list of feed items.</returns>
        IEnumerable<CommunityFeedItemViewModel> Get(CommunityFeedFilter filter);
    }
}
