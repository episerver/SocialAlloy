using System.Web.Mvc;
using System.Web.Routing;

namespace EPiServer.SocialAlloy.Web
{
    public class EPiServerApplication : EPiServer.Global
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            //Tip: Want to call the EPiServer API on startup? Add an initialization module instead (Add -> New Item.. -> EPiServer -> Initialization Module)
        }

        protected override void RegisterRoutes(RouteCollection routes)
        {
            base.RegisterRoutes(routes);

            routes.MapRoute(
                    "Moderation",
                    "moderation/{action}/{workflowItemId}",
                    new
                    {
                        controller = "Moderation",
                        action = "Index",
                        workflowItemId = UrlParameter.Optional
                    }
                );
        }
    }
}