﻿using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using System.Collections.Generic;

namespace EPiServer.SocialAlloy.Web.Social.Blocks
{
    /// <summary>
    /// The RatingBlock class defines the configuration used for rendering rating views.
    /// </summary>
    [ContentType(DisplayName = "RatingBlock", 
                 GUID = "069e2c52-fd48-49c5-8993-7a0347ea1f78", 
                 Description = "Configures the frontend view properties of a rating block")]
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
        /// Sets the default property values on the content data.
        /// </summary>
        /// <param name="contentType">Type of the content.</param>
        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);
            Heading = "Ratings and Statistics";
            ShowHeading = false;
        }
    }
}