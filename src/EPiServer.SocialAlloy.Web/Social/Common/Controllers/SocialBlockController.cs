using EPiServer.Core;
using EPiServer.Web.Mvc;
using System.Web.Mvc;

namespace EPiServer.SocialAlloy.Web.Social.Common.Controllers
{
    /// <summary>
    /// The SocialBlockController may contain social data/logic common to all social controllers.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SocialBlockController<T> : BlockController<T> where T : BlockData
    {
        /// <summary>
        /// Save model state.
        /// </summary>
        /// <param name="blockLink"></param>
        /// <param name="state"></param>
        protected virtual void SaveModelState(ContentReference blockLink, ModelStateDictionary state)
        {
            TempData[StateKey(blockLink)] = state;
        }

        /// <summary>
        /// Load previously saved model state.
        /// </summary>
        /// <param name="blockLink"></param>
        protected virtual void LoadModelState(ContentReference blockLink)
        {
            var key = StateKey(blockLink);
            var modelState = TempData[key] as ModelStateDictionary;

            if (modelState != null)
            {
                ViewData.ModelState.Merge(modelState);
                TempData.Remove(key);
            }
        }

        /// <summary>
        /// Attempt to get a model state given its key.
        /// </summary>
        /// <param name="key">The key of the state to get.</param>
        /// <returns>The </returns>
        protected ModelState GetStateValue(string key)
        {
            ModelState value;
            ViewData.ModelState.TryGetValue(key, out value);
            return value;
        }

        private string StateKey(ContentReference blockLink)
        {
            return "FormBlock_" + blockLink.ID;
        }
    }
}
