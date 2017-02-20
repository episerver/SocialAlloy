using EPiServer.SocialAlloy.Web.Models.ViewModels;
using EPiServer.SocialAlloy.Web.Social.Pages;
using EPiServer.Web.Mvc;
using System.Web.Mvc;

namespace EPiServer.SocialAlloy.Web.Social.Controllers
{
    /// <summary>
    /// The ProfilePageController handles the rendering of social profile pages and the corresponding blocks embedded on those pages
    /// </summary>
    public class ProfilePageController : PageController<ProfilePage>
    {
        /// <summary>
        /// Renders the social profile page view
        /// </summary>
        /// <param name="currentPage">The current social profile page</param>
        /// <returns>The corresponding action result</returns>
        public ActionResult Index(ProfilePage currentPage)
        {
            var pageViewModel = new PageViewModel<ProfilePage>(currentPage);
            return View("~/Views/Social/ProfilePage/Index.cshtml", pageViewModel);
        }
    }
}