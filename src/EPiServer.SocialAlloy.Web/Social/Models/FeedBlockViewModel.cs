using EPiServer.SocialAlloy.Web.Social.Blocks;
using EPiServer.SocialAlloy.Web.Social.Common.Models;
using System.Collections.Generic;

namespace EPiServer.SocialAlloy.Web.Social.Models
{
    /// <summary>
    /// The FeedBlockViewModel class represents the model that will be used to
    /// display a feed of activities in the feed block frontend view. The feed of activities 
    /// displayed depends on the logged in user and pages that the user has subscribed to.
    /// </summary>
    public class FeedBlockViewModel<T> : SocialBlockViewModel where T : SocialActivity  
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="block">A block reference to use as a key under which to save the model state.</param>
        public FeedBlockViewModel(FeedBlock block)
            : base()
        {
            Heading = block.Heading;
            ShowHeading = block.ShowHeading;
            FeedDisplayMax = block.FeedDisplayMax;
            FeedItems = new List<SocialActivityFeed<SocialActivity>>();
        }

        /// <summary>
        /// Gets or sets the heading for the frontend feed block display.
        /// </summary>
        public string Heading { get; set; }

        /// <summary>
        /// Gets or sets whether to show the block heading in the frontend feed block display.
        /// </summary>
        public bool ShowHeading { get; set; }

        /// <summary>
        /// Gets or sets the max number of feed items that should be displayed in the frontend view.
        /// </summary>
        public int FeedDisplayMax { get; set; }

        /// <summary>
        /// Gets or sets the feed items to show.
        /// </summary>
        public IEnumerable<SocialActivityFeed<SocialActivity>> FeedItems { get; set; }

        /// <summary>
        /// Gets or sets an error message that should be flashed in the message display view.
        /// </summary>
        public string DisplayErrorMessage { get; set; }
    }
}