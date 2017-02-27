using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.SocialAlloy.Web.Social.Common.Models;
using EPiServer.Web.Mvc;
using EPiServer.Web.Routing;
using System.Collections.Generic;
using System.Linq;

namespace EPiServer.SocialAlloy.Web.Social.Common.Controllers
{
    /// <summary> 
    /// The SocialBlockController may contain social data/logic common to all social controllers. 
    /// </summary>
    /// <typeparam name="T">The social block type.</typeparam>
    public abstract class SocialBlockController<T> : BlockController<T> where T : BlockData
    {
        protected readonly IPageRouteHelper pageRouteHelper;

        /// <summary>
        /// Constructor
        /// </summary>
        public SocialBlockController()
        {
            this.pageRouteHelper = ServiceLocator.Current.GetInstance<IPageRouteHelper>();
        }

        /// <summary>
        /// Used to retrieve the TempData stored for a specific controller
        /// </summary>
        /// <param name="key">Sring value of the TempData key</param>
        /// <returns>The list of MessageViewModels that was requested</returns>
        public List<MessageViewModel> RetrieveMessages(string key)
        {
            var listOfMessages = (List<MessageViewModel>)TempData[key];

            return (listOfMessages != null) && (listOfMessages.Any()) ? listOfMessages : new List<MessageViewModel>();
        }

        /// <summary>
        /// Stores a desired key / value in the TempData dictionary 
        /// </summary>
        /// <param name="key">The key used to reference the stored value upon retrieval</param>
        /// <param name="value">The value that is being stored in TempData</param>
        public void AddMessage(string key, MessageViewModel value)
        {
            var listOfMessages = RetrieveMessages(key);
            listOfMessages.Add(value);
            TempData[key] = listOfMessages;
        }
    }
}
