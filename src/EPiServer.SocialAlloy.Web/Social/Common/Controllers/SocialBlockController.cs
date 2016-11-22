using EPiServer.Core;
using EPiServer.Web.Mvc;

namespace EPiServer.SocialAlloy.Web.Social.Common.Controllers
{
    /// <summary>
    /// The SocialBlockController may contain social data/logic common to all social controllers.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SocialBlockController<T> : BlockController<T> where T : BlockData
    {
    }
}
