using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;

namespace EPiServer.SocialAlloy.Web.Social.Blocks.Groups
{
    /// <summary>
    /// The MembershipAffiliationBlock class defines the configuration used for the list of groups that a member is associated with.
    /// </summary>
    [ContentType(DisplayName = "MembershipGroupAssociation", GUID = "d7f22a41-a26c-4e85-b4a5-15929d4222fc", Description = "")]
    public class MembershipAffiliationBlock : BlockData
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
        /// Configures the maximum number of members that should be displayed in the view.
        /// </summary>
        [Display(
            GroupName = SystemTabNames.Content,
            Order = 4)]
        public virtual int DisplayPageSize { get; set; }

        /// <summary>
        /// Sets the default property values on the content data.
        /// </summary>
        /// <param name="contentType">Type of the content.</param>
        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);
            Heading = "Membership Affiliation ";
            ShowHeading = false;
            DisplayPageSize = 10;
        }
    }
}