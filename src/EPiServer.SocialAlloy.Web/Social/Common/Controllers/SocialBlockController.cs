using EPiServer.Core;
using EPiServer.Web.Mvc;
using System.Web.Mvc;

namespace EPiServer.SocialAlloy.Web.Social.Common.Controllers
{
    /// <summary>
    /// The SocialBlockController may contain social data/logic common to all social controllers.
    /// </summary>
    /// <typeparam name="T">The social block type.</typeparam>
    public abstract class SocialBlockController<T> : BlockController<T> where T : BlockData
    {
        /// <summary>
        /// Save model state.
        /// </summary>
        /// <param name="block">A block reference to use as a key under which to save the model state.</param>
        /// <param name="state">The model state dictionary to save.</param>
        protected virtual void SaveModelState(ContentReference block, ModelStateDictionary state)
        {
            TempData[GetModelStateKey(block)] = state;
        }

        /// <summary>
        /// Load previously saved model state.
        /// </summary>
        /// <param name="block">A block reference to use as the key from which to load the model state.</param>
        protected virtual void LoadModelState(ContentReference block)
        {
            var key = GetModelStateKey(block);
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
        /// <returns>The model state.</returns>
        protected ModelState GetModelState(string key)
        {
            ModelState value;
            ViewData.ModelState.TryGetValue(key, out value);
            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="block">A block reference used to composed a qualified model state key.</param>
        /// <returns>The fully qualified model state key.</returns>
        private string GetModelStateKey(ContentReference block)
        {
            return "SocialFormBlock_" + block.ID;
        }
    }
}
