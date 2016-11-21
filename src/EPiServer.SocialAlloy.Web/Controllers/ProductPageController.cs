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
            @ViewBag.PageReference = currentPage.ContentLink;
            return View(string.Format("~/Views/{0}/Index.cshtml", currentPage.GetOriginalType().Name), model);
        }

        [HttpPost]
        public ActionResult Rate(ProductPage currentContent, int? userRating)
        {
            return RedirectToAction("Index");
        }
    }
}