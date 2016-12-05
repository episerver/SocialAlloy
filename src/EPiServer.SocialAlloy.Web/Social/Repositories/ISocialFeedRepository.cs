using EPiServer.SocialAlloy.Web.Social.Models;
using System.Collections.Generic;

namespace EPiServer.SocialAlloy.Web.Social.Repositories
{
    /// <summary>
    /// The ISocialFeedRepository interface defines the operations that can be issued
    /// against a comment repository.
    /// </summary>
    public interface ISocialFeedRepository
    {
        /// <summary>
        /// Gets comments from the underlying comment repository based on a filter.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>A list of comments.</returns>
        IEnumerable<SocialActivityFeedViewModel> Get(SocialFeedFilter filter);
    }
}
