using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Web.Mvc;
using EPiServer.Web.Routing;
using System;
using System.Web.Mvc;

namespace EPiServer.SocialAlloy.Web.Social.Common.Controllers
{
    /// <summary> 
    /// The SocialBlockController may contain social data/logic common to all social controllers. 
    /// </summary>
    /// <typeparam name="T">The social block type.</typeparam>
    public abstract class SocialBlockController<T> : BlockController<T> where T : BlockData
    {
        protected readonly IContentRepository contentRepository;
        protected readonly IPageRouteHelper pageRouteHelper;

        /// <summary>
        /// Constructor
        /// </summary>
        public SocialBlockController()
        {
            this.contentRepository = ServiceLocator.Current.GetInstance<IContentRepository>();
            this.pageRouteHelper = ServiceLocator.Current.GetInstance<IPageRouteHelper>();
        }

        public Temp GetFromTempData<Temp>(string key)
        {
            return (Temp)TempData[key];
        }

        public void AddToTempData<Temp>(string key, Temp value)
        {
            TempData[key] = value;
        }
    }
}
