using System;

namespace EPiServer.SocialAlloy.Web.Social.Models
{
    /// <summary>
    /// This class represents the view model of the feed items displayed by a 
    /// Social feed block.
    /// </summary>
    public class SocialFeedViewModel
    {
        /// <summary>
        /// The activity actor.
        /// </summary>
        public string Actor { get; set; }

        /// <summary>
        /// The page on which the activity occured.
        /// </summary>
        public string Target { get; set; }

        /// <summary>
        /// The date/time on which the activity was received by the Social Activity Streams system.
        /// </summary>
        public DateTime ActivityDate { get; set; }

        /// <summary>
        /// A string representation describing the activity that was received by the Social Activity Streams system.
        /// </summary>
        public string Description { get; set; }

    }
}