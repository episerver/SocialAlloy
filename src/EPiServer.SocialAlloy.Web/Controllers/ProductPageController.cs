using EPiServer.Framework.DataAnnotations;
using EPiServer.ServiceLocation;
using EPiServer.Social.Common;
using EPiServer.Social.Ratings.Core;
using EPiServer.SocialAlloy.Web.Models.Pages;
using EPiServer.SocialAlloy.Web.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EPiServer.SocialAlloy.Web.Business;

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
                var user = Reference.Create(model.CurrentUserName);
                var target = Reference.Create(currentPage.ContentLink.ToString());

                //Get rating statistics for currentPage
                //@ViewBag.Statistics = target.GetStatistics();

                //Get existing rating for logged in user and currentPage
                //@ViewBag.UserRatingForTarget = user.GetRating(target);

            }
            return View(string.Format("~/Views/{0}/Index.cshtml", currentPage.GetOriginalType().Name), model);
        }

        [HttpPost]
        public ActionResult Rate(ProductPage currentContent, int? userRating)
        {
            if (this.User.Identity.IsAuthenticated && userRating.HasValue)
            {
                string productId = currentContent.ContentLink.ToString();

                //Add the rating submitted by logged in user for currentPage
                var user = Reference.Create(this.User.Identity.Name);
                var target = Reference.Create(currentContent.ContentLink.ToString());
                user.Rate(target, userRating.Value);

                return RedirectToAction("Index");
            }
            else
                throw new Exception("You must login to Rate the page");
        }
    }
}