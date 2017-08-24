using EPiServer.SocialAlloy.Web.Social.Models;

namespace EPiServer.SocialAlloy.Web.Social.Repositories
{
    /// <summary>
    /// This interface defines the operations used to add activities pretaining to community actions.
    /// </summary>
    public interface ICommunityActivityRepository
    {
        /// <summary>
        /// Adds a page comment activity.
        /// </summary>
        /// <param name="actor">the actor who initiated the activity</param>
        /// <param name="target">the target of the activity</param>
        /// <param name="activity">an instance of PageCommentActivity</param>
        void Add(string actor, string target, PageCommentActivity activity);

        /// <summary>
        /// Adds a page rating activity.
        /// </summary>
        /// <param name="actor">the actor who initiated the activity</param>
        /// <param name="target">the target of the activity</param>
        /// <param name="activity">an instance of PageRatingActivity</param>
        void Add(string actor, string target, PageRatingActivity activity);
    }
}
