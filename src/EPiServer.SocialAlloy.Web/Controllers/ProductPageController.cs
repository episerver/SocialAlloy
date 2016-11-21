using EPiServer.Framework.DataAnnotations;
using EPiServer.SocialAlloy.Web.Models.Pages;
using EPiServer.SocialAlloy.Web.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EPiServer.SocialAlloy.Web.Controllers
{
    [TemplateDescriptor(Inherited = true)]
    public class ProductPageController : PageControllerBase<ProductPage>
    {
        public ViewResult Index(ProductPage currentPage)
        {
            var model = ProductViewModel.Create(currentPage);
            if (this.User.Identity.IsAuthenticated)
            {
                model.CurrentUserName = this.User.Identity.Name;

                //Get existing rating for logged in user and currentPage
            }
            return View(string.Format("~/Views/{0}/Index.cshtml", currentPage.GetOriginalType().Name), model);
        }

        [HttpPost]
        public ActionResult Rate(ProductPage currentContent, int? userRating)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                string productId = currentContent.ContentLink.ToString();

                //Add the rating submitted by logged in user for currentPage
                return RedirectToAction("Index");
            }
            else
                throw new Exception("You must login to Rate the page");
        }
    }
}