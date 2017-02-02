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
    /// Used for the pages that wish to contain Social community features
    /// </summary>
    [ContentType(DisplayName = "SocialProfilePage", GUID = "8b4c5048-2116-467d-9f04-9e7fd5648955", Description = "")]

    [ImageUrl(Global.StaticGraphicsFolderPath + "page-type-thumbnail-standard.png")]
    public class SocialProfilePage : StandardPage
    {
        [CultureSpecific]
        [Display(
            Name = "Main body",
            Description = "The main body will be shown in the main content area of the page, using the XHTML-editor you can insert for example text, images and tables.",
            GroupName = SystemTabNames.Content,
            Order = 1)]
        public virtual XhtmlString MainBody { get; set; }

        [Display(
            Name = "Feed Block",
            Description = "The feed section of the profile page. Local feed block will display feed items for the pages a user has subscriped to.",
            GroupName = SystemTabNames.Content,
            Order = 2)]
        public virtual FeedBlock Feed { get; set; }

        [Display(
            Name = "Membership Affiliation Block",
            Description = "The membership affiliation section of the profile page. Local MembershipAffiliation block will display the groups that the currently logged in user is a member of.",
            GroupName = SystemTabNames.Content,
            Order = 2)]
        public virtual MembershipAffiliationBlock MembershipAffiliation { get; set; }
        
    }
}