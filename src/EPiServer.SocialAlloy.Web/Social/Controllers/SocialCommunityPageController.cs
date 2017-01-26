using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EPiServer;
using EPiServer.Core;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Web.Mvc;
using EPiServer.SocialAlloy.Web.Social.Pages;
using EPiServer.SocialAlloy.Web.Models.ViewModels;

namespace EPiServer.SocialAlloy.Web.Social.Controllers
{
    public class SocialCommunityPageController : PageController<SocialCommunityPage>
    {
        public ActionResult Index(SocialCommunityPage currentPage)
        {
            /* Implementation of action. You can create your own view model class that you pass to the view or
             * you can pass the page type for simpler templates */
            var pageViewModel = new PageViewModel<SocialCommunityPage>(currentPage);
            return View("~/Views/Social/SocialCommunityPage/Index.cshtml", pageViewModel);
        }
    }
}