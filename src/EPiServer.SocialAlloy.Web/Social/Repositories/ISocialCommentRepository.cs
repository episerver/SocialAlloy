using EPiServer.Social.Comments.Core;
using EPiServer.SocialAlloy.Web.Social.Models;
using System.Collections.Generic;

namespace EPiServer.SocialAlloy.Web.Social.Repositories
{
    /// <summary>
    /// The ICommentRepository interface defines the operations that can be issued
    /// against a comment repository.
    /// </summary>
    public interface ISocialCommentRepository
    {
        /// <summary>
        /// Adds a comment to the underlying comment repository.
        /// </summary>
        /// <param name="comment">The comment to add.</param>
        /// <returns></returns>
        Comment Add(SocialComment comment);

        /// <summary>
        /// Gets comments based on the given filter.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        IEnumerable<SocialComment> Get(SocialCommentFilter filter);
    }
}