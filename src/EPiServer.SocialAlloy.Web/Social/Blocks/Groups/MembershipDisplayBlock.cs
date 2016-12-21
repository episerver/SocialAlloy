using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace EPiServer.SocialAlloy.Web.Social.Blocks.Groups
{
    /// <summary>
    /// The MembershipDisplayBlock class defines the configuration used for rendering group creation views.
    /// </summary>
    [ContentType(DisplayName = "Social Membership Display", GUID = "0d5075ad-31ea-40cb-ae8f-a88b519db35f", Description = "Configures the properties of a membership display block view")]
    public class MembershipDisplayBlock : BlockData
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
        /// The name of the group entered in the admin view and used to display membership.
        /// </summary>
        [Display(
             GroupName = SystemTabNames.Content,
             Order = 3)]
        [CultureSpecific]
        public virtual string GroupName { get; set; }

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
            Heading = "Group Membership Display";
            ShowHeading = false;
            GroupName = "Default Group";
            DisplayPageSize = 10;
        }
    }
}
