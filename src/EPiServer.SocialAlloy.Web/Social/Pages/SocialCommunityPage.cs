using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.SocialAlloy.Web.Models.Pages;
using EPiServer.SocialAlloy.Web.Social.Blocks;

namespace EPiServer.SocialAlloy.Web.Social.Pages
{

    /// <summary>
    /// Used for the pages that wish to contain Social community features
    /// </summary>
    [ContentType(DisplayName = "SocialCommunity", GUID = "56ba715e-3fb9-4050-a5e3-ab7fe1690742", Description = "")]
    [ImageUrl(Global.StaticGraphicsFolderPath + "page-type-thumbnail-standard.png")]
    public class SocialCommunityPage : StandardPage
    {

        [CultureSpecific]
        [Display(
            Name = "Main body",
            Description = "The main body will be shown in the main content area of the page, using the XHTML-editor you can insert for example text, images and tables.",
            GroupName = SystemTabNames.Content,
            Order = 1)]
        public virtual XhtmlString MainBody { get; set; }

        [Display(
            Name = "Comment Block",
            Description = "The comment section of the page. Local comment block will display comments only for this page",
            GroupName = SystemTabNames.Content,
            Order = 1)]
        public virtual CommentsBlock Comments { get; set; }

    }
}