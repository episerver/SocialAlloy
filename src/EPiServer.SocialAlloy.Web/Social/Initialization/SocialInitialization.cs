using EPiServer.Cms.UI.AspNetIdentity;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using EPiServer.SocialAlloy.Web.Social.Repositories;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using StructureMap;
using System;

namespace EPiServer.SocialAlloy.Web.Social.Initialization
{
    [InitializableModule]
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class SocialInitialization : IConfigurableModule
    {
        public void ConfigureContainer(ServiceConfigurationContext context)
        {
            context.Container.Configure(ConfigureContainer);

        }

        private static void ConfigureContainer(ConfigurationExpression configuration)
        {
            configuration.For<IUserRepository>().Use(() => CreateUserRepository());
        }

        public void Initialize(InitializationEngine context)
        {
        }

        public void Uninitialize(InitializationEngine context)
        {
        }

        private static IUserRepository CreateUserRepository()
        {
            return new UserRepository(new UserManager<IdentityUser>(
                    new UserStore<IdentityUser>(new ApplicationDbContext<IdentityUser>()))
            );
        }
    }
}