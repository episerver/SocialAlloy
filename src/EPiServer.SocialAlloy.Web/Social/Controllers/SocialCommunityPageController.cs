using EPiServer.SocialAlloy.Web.Models.ViewModels;
using EPiServer.SocialAlloy.Web.Social.Pages;
using EPiServer.Web.Mvc;
using System.Web.Mvc;

namespace EPiServer.SocialAlloy.Web.Social.Controllers
{
    /// <summary>
    /// The SocialCommunityPageController handles the rendering of social community pages and the corresponding blocks embedded on those pages
    /// </summary>
    public class SocialCommunityPageController : PageController<SocialCommunityPage>
    {
        /// <summary>
        /// Renders the social community page view.
        /// </summary>
        /// <param name="currentPage">The current page</param>
        public ActionResult Index(SocialCommunityPage currentPage)
        {
            var pageViewModel = new PageViewModel<SocialCommunityPage>(currentPage);
            return View("~/Views/Social/SocialCommunityPage/Index.cshtml", pageViewModel);
        }
    }
}