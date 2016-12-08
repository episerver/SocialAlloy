using EPiServer.SocialAlloy.Web.Social.Models;

namespace EPiServer.SocialAlloy.Web.Social.Repositories
{
    /// <summary>
    /// This interface defines the operations used to add activities to the 
    /// Episerver Social Activity Streams system.
    /// </summary>
    public interface ISocialActivityRepository
    {
        /// <summary>
        /// Adds an activity to the Episerver Social Activity Streams system.
        /// </summary>
        /// <param name="actor">the actor who initiated the activity</param>
        /// <param name="target">the target of the activity</param>
        /// <param name="activity">an instance of SocialActivity</param>
        void Add(string actor, string target, SocialActivity activity);
    }
}
