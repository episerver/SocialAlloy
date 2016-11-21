using System.Linq;
using System.Web.Mvc;

namespace EPiServer.SocialAlloy.Web.Social.Comments.Config
{
    /// <summary>
    /// This class configures ~/Views/Social for social related partial/views lookup.
    /// </summary>
    public class SocialViewEngine : RazorViewEngine
    {
        private static readonly string[] SocialPartialViewFormats = new[]
        {
            "~/Views/Social/{1}/{0}.cshtml"
        };

        public SocialViewEngine()
        {
            base.PartialViewLocationFormats = base.PartialViewLocationFormats.Union(SocialPartialViewFormats).ToArray();
            base.ViewLocationFormats = base.ViewLocationFormats.Union(SocialPartialViewFormats).ToArray();
        }
    }
}