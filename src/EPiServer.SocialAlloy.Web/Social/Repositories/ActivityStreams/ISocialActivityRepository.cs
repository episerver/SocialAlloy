using EPiServer.SocialAlloy.Web.Social.Models;

namespace EPiServer.SocialAlloy.Web.Social.Repositories
{
    /// <summary>
    /// This interface defines the operations used to add activities to the 
    /// Episerver Social ActivityStreams system.
    /// </summary>
    public interface ISocialActivityRepository
    {
        /// <summary>
        /// Adds an activity to the Episerver Social ActivityStreams system.
        /// </summary>
        /// <typeparam name="T">Type of the activity</typeparam>
        /// <param name="actor">the actor who initiated the activity</param>
        /// <param name="target">the target of the activity</param>
        /// <param name="activity">an instance of SocialActivity</param>
        void Add<T>(string actor, string target, T activity) where T : SocialActivity;
    }
}
