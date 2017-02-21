using EPiServer.SocialAlloy.Web.Models.ViewModels;
using EPiServer.SocialAlloy.Web.Social.Pages;
using EPiServer.Web.Mvc;
using System.Web.Mvc;

namespace EPiServer.SocialAlloy.Web.Social.Controllers
{
    /// <summary>
    /// The CommunityPageController handles the rendering of social community pages and the corresponding blocks embedded on those pages
    /// </summary>
    public class CommunityPageController : PageController<CommunityPage>
    {
        /// <summary>
        /// Renders the social community page view.
        /// </summary>
        /// <param name="currentPage">The current page</param>
        public ActionResult Index(CommunityPage currentPage)
        {
            var pageViewModel = new PageViewModel<CommunityPage>(currentPage);
            return View("~/Views/Social/CommunityPage/Index.cshtml", pageViewModel);
        }
    }
}