using EPiServer.SocialAlloy.Web.Models.ViewModels;
using EPiServer.SocialAlloy.Web.Social.Pages;
using EPiServer.Web.Mvc;
using System.Web.Mvc;

namespace EPiServer.SocialAlloy.Web.Social.Controllers
{
    public class SocialCommunityPageController : PageController<SocialCommunityPage>
    {
        public ActionResult Index(SocialCommunityPage currentPage)
        {
            var pageViewModel = new PageViewModel<SocialCommunityPage>(currentPage);
            return View("~/Views/Social/SocialCommunityPage/Index.cshtml", pageViewModel);
        }
    }
}