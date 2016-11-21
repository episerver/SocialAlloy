using EPiServer.SocialAlloy.Web.Social.Comments.Config;
using System.Web.Mvc;

namespace EPiServer.SocialAlloy.Web
{
    public class EPiServerApplication : EPiServer.Global
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            // Register the paths for social views.
            ViewEngines.Engines.Add(new SocialViewEngine());

            //Tip: Want to call the EPiServer API on startup? Add an initialization module instead (Add -> New Item.. -> EPiServer -> Initialization Module)
        }
    }
}