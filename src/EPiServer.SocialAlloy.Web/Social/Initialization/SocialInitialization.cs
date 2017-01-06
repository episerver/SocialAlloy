using EPiServer.Cms.UI.AspNetIdentity;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using EPiServer.Social.Comments.Core;
using EPiServer.SocialAlloy.Web.Social.Adapters;
using EPiServer.SocialAlloy.Web.Social.Models;
using EPiServer.Social.Groups.Core;
using EPiServer.SocialAlloy.Web.Social.Repositories;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using StructureMap;

namespace EPiServer.SocialAlloy.Web.Social.Initialization
{
    /// <summary>
    /// The SocialInitialization class initializes the IOC container mapping social component
    /// interfaces to their implementations.
    /// </summary>
    [InitializableModule]
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class SocialInitialization : IConfigurableModule
    {
        /// <summary>
        /// Configure the IoC container before initialization.
        /// </summary>
        /// <param name="context">The context on which the container can be accessed.</param>
        public void ConfigureContainer(ServiceConfigurationContext context)
        {
            context.Container.Configure(ConfigureContainer);
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        /// <param name="context">The instance context.</param>
        public void Initialize(InitializationEngine context)
        {
        }

        /// <summary>
        /// Uninitializes this instance.
        /// </summary>
        /// <param name="context">The instance context.</param>
        public void Uninitialize(InitializationEngine context)
        {
        }

        /// <summary>
        /// Configure the IOC container.
        /// </summary>
        /// <param name="configuration">The IOC container configuration.</param>
        private static void ConfigureContainer(ConfigurationExpression configuration)
        {
            configuration.For<IUserRepository>().Use(() => CreateUserRepository());
            configuration.For<IPageRepository>().Use<PageRepository>();

            configuration.For<ISocialCommentRepository>().Use<SocialCommentRepository>();
            configuration.For<ISocialRatingRepository>().Use<SocialRatingRepository>();

            configuration.For<ISocialSubscriptionRepository>().Use<SocialSubscriptionRepository>();
            configuration.For<ISocialActivityAdapter>().Use<SocialActivityAdapter>();
            configuration.For<ISocialFeedRepository>().Use<SocialFeedRepository>();
            configuration.For<ISocialActivityRepository>().Use<SocialActivityRepository>();
            configuration.For<ISocialGroupRepository>().Use<SocialGroupRepository>();
            configuration.For<ISocialMemberRepository>().Use<SocialMemberRepository>();
        }

        /// <summary>
        /// Create a UserRepository.
        /// </summary>
        /// <returns>The created UserRepository instance.</returns>
        private static IUserRepository CreateUserRepository()
        {
            return new UserRepository(new UserManager<IdentityUser>(
                    new UserStore<IdentityUser>(new ApplicationDbContext<IdentityUser>()))
            );
        }
    }
}