using System;

namespace EPiServer.SocialAlloy.Web.Social.Models
{
    /// <summary>
    /// The SocialFeed class describes a feed model used by the SocialAlloy site.
    /// </summary>
    public class SocialActivityFeed<T> where T : SocialActivity
    {
        /// <summary>
        /// The activity actor.
        /// </summary>
        public string Actor { get; set; }

        /// <summary>
        /// The page on which the activity occured.
        /// </summary>
        public string PageId { get; set; }

        /// <summary>
        /// The date/time on which the activity was received by the Social Activity Streams system.
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// The type of activity that was sent to the Social Activity Streams system.
        /// </summary>
        public T Type { get; set; }

    }
}