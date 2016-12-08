using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace EPiServer.SocialAlloy.Web.Social.Blocks.Groups
{
    /// <summary>
    /// The MembershipDisplayBlock class defines the configuration used for rendering group creation views.
    /// </summary>
    [ContentType(DisplayName = "Membership Display Block", GUID = "0d5075ad-31ea-40cb-ae8f-a88b519db35f", Description = "Social Membership Diaplay")]
    public class MembershipDisplayBlock : BlockData
    {
        [Display(
            GroupName = SystemTabNames.Content,
            Order = 1)]
        [CultureSpecific]
        public virtual string Heading { get; set; }

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
            Heading = "Membership Display Block";
            GroupName = "defaultGroup";//GetGroupInfo();
        }
    }
}
