using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using System.Collections.Generic;

namespace EPiServer.SocialAlloy.Web.Social.Blocks
{
    [ContentType(DisplayName = "RatingBlock", GUID = "069e2c52-fd48-49c5-8993-7a0347ea1f78", Description = "Social Rating Statistics")]
    public class RatingBlock : BlockData
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
            Order = 2)]
        public virtual bool ShowHeading { get; set; }

        /// <summary>
        /// Gets or sets a page of comments for this commentblock.
        /// </summary>
        [Ignore]
        public virtual List<int> RatingValues { get; set; }

        /// <summary>
        /// Sets the default property values on the content data.
        /// </summary>
        /// <param name="contentType">Type of the content.</param>
        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);
            Heading = "Social Ratings & Statistics";
            RatingValues = new List<int> { 1, 2, 3, 4, 5 };
            ShowHeading = false;
        }
    }
}