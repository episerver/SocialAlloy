using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.ServiceLocation;
using EPiServer.Social.Common;
using EPiServer.Social.Groups.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace EPiServer.SocialAlloy.Web.Social.Blocks.Groups
{
    /// <summary>
    /// The GroupAdmissionBlock class defines the configuration used for rendering group admission views.
    /// </summary>
    [ContentType(DisplayName = "Group Admission Block", GUID = "611697e3-3638-445c-a45c-6454eaa5b7b1", Description = "Social Group Admission")]
    public class GroupAdmissionBlock : BlockData
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

        // public virtual Dictionary<string, int> GroupInfo { get; set; }
        [Display(
             GroupName = SystemTabNames.Content,
             Order = 1)]
        [CultureSpecific]
        public virtual string GroupName { get; set; }
        /// <summary>
        /// Sets the default property values on the content data.
        /// </summary>
        /// <param name="contentType">Type of the content.</param>
        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);
            Heading = "Social Group Admission";
            ShowHeading = false;
            GroupName = "defaultGroup";
        }
    }
}
