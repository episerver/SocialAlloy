using System;

namespace EPiServer.SocialAlloy.Web.Social.Models
{
    /// <summary>
    /// This class represents the view model of a feed item displayed in a 
    /// Social feed block.
    /// </summary>
    public class SocialFeedItemViewModel
    {
        /// <summary>
        /// A header summarizing the activity that occurred.
        /// </summary>
        public string Heading { get; set; }

        /// <summary>
        /// A description of the activity that was received by the Social Activity Streams system.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The date/time on which the activity was received by the Social Activity Streams system.
        /// </summary>
        public DateTime ActivityDate { get; set; }
    }
}