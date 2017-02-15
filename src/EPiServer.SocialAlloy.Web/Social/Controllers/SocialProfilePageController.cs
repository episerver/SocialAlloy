using EPiServer.SocialAlloy.Web.Models.ViewModels;
using EPiServer.SocialAlloy.Web.Social.Pages;
using EPiServer.Web.Mvc;
using System.Web.Mvc;

namespace EPiServer.SocialAlloy.Web.Social.Controllers
{
    /// <summary>
    /// The SocialProfilePageController handles the rendering of social profile pages and the corresponding blocks embedded on those pages
    /// </summary>
    public class SocialProfilePageController : PageController<SocialProfilePage>
    {
        /// <summary>
        /// Renders the social profile page view
        /// </summary>
        /// <param name="currentPage">The current social profile page</param>
        /// <returns>The corresponding action result</returns>
        public ActionResult Index(SocialProfilePage currentPage)
        {
            var pageViewModel = new PageViewModel<SocialProfilePage>(currentPage);
            return View("~/Views/Social/SocialProfilePage/Index.cshtml", pageViewModel);
        }
    }
}