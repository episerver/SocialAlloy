using EPiServer.Core;
using EPiServer.Web.Mvc;
using System.Web.Mvc;

namespace EPiServer.SocialAlloy.Web.Social.Common.Controllers
{
    /// <summary>
    /// The SocialBlockController may contain social data/logic common to all social controllers.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SocialBlockController<T> : BlockController<T> where T : BlockData
    {
        protected virtual void SaveModelState(ContentReference blockLink, ModelStateDictionary otherState = null)
        {
            var modelState = ViewData.ModelState;
            if (otherState != null)
            {
                modelState.Merge(otherState);
            }
            TempData[StateKey(blockLink)] = modelState;
        }

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

        private string StateKey(ContentReference blockLink)
        {
            return "FormBlock_" + blockLink.ID;
        }
    }
}
