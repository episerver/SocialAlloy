using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.SocialAlloy.Web.Models.Pages;
using EPiServer.SocialAlloy.Web.Social.Blocks;
using EPiServer.SocialAlloy.Web.Social.Blocks.Groups;
using System.ComponentModel.DataAnnotations;

namespace EPiServer.SocialAlloy.Web.Social.Pages
{
    /// <summary>
    /// Used for the pages that wish to contain customized details for currently logged in users
    /// </summary>
    [ContentType(DisplayName = "ProfilePage", GUID = "8b4c5048-2116-467d-9f04-9e7fd5648955", Description = "A profile page for the currently logged in user.")]

    [ImageUrl(Global.StaticGraphicsFolderPath + "page-type-thumbnail-standard.png")]
    public class ProfilePage : StandardPage
    {
        /// <summary>
        /// The feed section of the profile page. Local feed block will display feed items for the pages a user has subscriped to.
        /// </summary>
        [Display(
            Name = "Feed Block",
            Description = "The feed section of the profile page. Local feed block will display feed items for the pages a user has subscriped to.",
            GroupName = SystemTabNames.Content,
            Order = 2)]
        public virtual FeedBlock Feed { get; set; }

        /// <summary>
        /// The membership affiliation section of the profile page. Local membership affiliation block will display the groups that the currently logged in user is a member of.
        /// </summary>
        [Display(
            Name = "Membership Affiliation Block",
            Description = "The membership affiliation section of the profile page. Local membership affiliation block will display the groups that the currently logged in user is a member of.",
            GroupName = SystemTabNames.Content,
            Order = 2)]
        public virtual MembershipAffiliationBlock MembershipAffiliation { get; set; }
    }
}