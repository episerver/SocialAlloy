using EPiServer.SocialAlloy.Web.Social.Models;
using System.Collections.Generic;

namespace EPiServer.SocialAlloy.Web.Social.Repositories
{
    /// <summary>
    /// The ISocialFeedRepository interface defines the operations that can be issued
    /// against a feed item repository.
    /// </summary>
    public interface ISocialFeedRepository
    {
        /// <summary>
        /// Gets feed items from the underlying feed repository based on the specified
        /// filter.
        /// </summary>
        /// <param name="filter">a feed item filter to filter the feed items by</param>
        /// <returns>A list of feed items.</returns>
        IEnumerable<SocialFeedItemViewModel> Get(SocialFeedFilter filter);
    }
}
