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

        /// <summary>
        /// Used to retrieve the TempData stored for a specific controller action
        /// </summary>
        /// <typeparam name="Temp">The type of the TempData</typeparam>
        /// <param name="key">Sring value of the TempData key</param>
        /// <returns>The TempData that was requested</returns>
        public Temp GetFromTempData<Temp>(string key)
        {
            return (Temp)TempData[key];
        }

        /// <summary>
        /// Stores a desired key / value in the TempData dictionary 
        /// </summary>
        /// <typeparam name="Temp">The type of the value being stored</typeparam>
        /// <param name="key">The key used to reference the stored value upon retrieval</param>
        /// <param name="value">The value that is being stored in TempData</param>
        public void AddToTempData<Temp>(string key, Temp value)
        {
            TempData[key] = value;
        }
    }
}
