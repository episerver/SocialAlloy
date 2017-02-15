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
    [ContentType(DisplayName = "SocialCommunity", GUID = "56ba715e-3fb9-4050-a5e3-ab7fe1690742", Description = "A reseller's community page using Episerver Social.")]
    [ImageUrl(Global.StaticGraphicsFolderPath + "page-type-thumbnail-standard.png")]
    public class SocialCommunityPage : StandardPage
    {
        /// <summary>
        /// The main content body of the page
        /// </summary>
        [CultureSpecific]
        [Display(
            Name = "Main body",
            Description = "The main body will be shown in the main content area of the page, using the XHTML-editor you can insert for example text, images and tables.",
            GroupName = SystemTabNames.Content,
            Order = 1)]
        public virtual XhtmlString MainBody { get; set; }

        /// <summary>
        /// The comment section of the page. Local comment block will display comments only for this page
        /// </summary>
        [Display(
            Name = "Comment Block",
            Description = "The comment section of the page. Local comment block will display comments only for this page",
            GroupName = SystemTabNames.Content,
            Order = 2)]
        public virtual CommentsBlock Comments { get; set; }

        /// <summary>
        /// The comment section of the page. Local ratings block will allow a logged in user to rate this page
        /// </summary>
        [Display(
            Name = "Ratings Block",
            Description = "The comment section of the page. Local ratings block will allow a logged in user to rate this page",
            GroupName = SystemTabNames.Content,
            Order = 3)]
        public virtual RatingBlock Ratings { get; set; }

        /// <summary>
        /// The subscription section of the page. Local subscription block will allow a logged in user to subscribe to this page
        /// </summary>
        [Display(
            Name = "Subscription Block",
            Description = "The subscription section of the page. Local subscription block will allow a logged in user to subscribe to this page",
            GroupName = SystemTabNames.Content,
            Order = 4)]
        public virtual SubscriptionBlock Subscriptions { get; set; }

        /// <summary>
        /// The membership display section of the page. Local membership display block will display existing membership for the group that corresponds to this page
        /// </summary>
        [Display(
            Name = "Membership Display Block",
            Description = "The membership display section of the page. Local membership display block will display existing membership for the group that corresponds to this page",
            GroupName = SystemTabNames.Content,
            Order = 5)]
        public virtual MembershipDisplayBlock Memberships { get; set; }

        /// <summary>
        /// The group admission section of the page. Local group creation block will allow a logged in user to submit a request for membrship admission for the group that corresponds to this page
        /// </summary>
        [Display(
            Name = "Group Admission Block",
            Description = "The group admission section of the page. Local group creation block will allow a logged in user to submit a request for membrship admission for the group that corresponds to this page",
            GroupName = SystemTabNames.Content,
            Order = 6)]
        public virtual GroupAdmissionBlock GroupAdmission { get; set; }
    }
}