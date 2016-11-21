using EPiServer.SocialAlloy.Web.Models.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPiServer.SocialAlloy.Web.Models.ViewModels
{
    public class ProductViewModel<T> : PageViewModel<T> where T : SitePageData
    {
        public ProductViewModel(T currentPage)
            : base(currentPage)
        {
            PageRatings = new List<int>() { 1, 2, 3, 4, 5 };
        }

        public String CurrentUserName { get; set; }
        public String PageId { get; set; }

        public List<int> PageRatings { get; set; }

        public int? UserRating { get; set; }

    }
    public static class ProductViewModel
    {
        public static ProductViewModel<T> Create<T>(T page) where T : SitePageData
        {
            return new ProductViewModel<T>(page);
        }

    }
}