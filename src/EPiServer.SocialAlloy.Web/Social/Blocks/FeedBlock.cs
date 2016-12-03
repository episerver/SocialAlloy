using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;

namespace EPiServer.SocialAlloy.Web.Social.Blocks
{
    [ContentType(DisplayName = "ActivityStreamsFeedBlock",
                 GUID = "2bb4ac6d-6f09-4d38-adb0-5dc2bcf310ac",
                 Description = "Configures the properties of a feed block frontend view")]
    public class FeedBlock : BlockData
    {
        /// <summary>
        /// Configures the heading that should be used when displaying the block view in the frontend.
        /// </summary>
        [Display(
            GroupName = SystemTabNames.Content,
            Order = 1)]
        [CultureSpecific]
        public virtual string Heading { get; set; }

        /// <summary>
        /// Configures whether the heading should be displayed in the block's frontend view.
        /// </summary>
        [Display(
            GroupName = SystemTabNames.Content,
            Order = 1)]
        public virtual bool ShowHeading { get; set; }

        /// <summary>
        /// Configures the max number of feed items that should be displayed in the frontend view.
        /// </summary>
        [Display(
            GroupName = SystemTabNames.Content,
            Order = 1)]
        public virtual int FeedDisplayMax { get; set; }

        /// <summary>
        /// Sets the default configuration values.
        /// </summary>
        /// <param name="contentType">Type of the content.</param>
        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);
            ShowHeading = false;
            Heading = "Activity Feed";
            FeedDisplayMax = 20;
        }
    }
}