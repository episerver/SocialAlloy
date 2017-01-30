using EPiServer.SocialAlloy.Web.Models.ViewModels;
using EPiServer.SocialAlloy.Web.Social.Pages;
using EPiServer.Web.Mvc;
using System.Web.Mvc;

namespace EPiServer.SocialAlloy.Web.Social.Controllers
{
    public class SocialProfilePageController : PageController<SocialProfilePage>
    {
        public ActionResult Index(SocialProfilePage currentPage)
        {
            var pageViewModel = new PageViewModel<SocialProfilePage>(currentPage);
            return View("~/Views/Social/SocialProfilePage/Index.cshtml", pageViewModel);
        }
    }
}