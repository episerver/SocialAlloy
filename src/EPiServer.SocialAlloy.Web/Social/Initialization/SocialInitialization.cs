using EPiServer.Cms.UI.AspNetIdentity;
using EPiServer.Core;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using EPiServer.SocialAlloy.ExtensionData.Group;
using EPiServer.SocialAlloy.Web.Social.Adapters;
using EPiServer.SocialAlloy.Web.Social.Models.Groups;
using EPiServer.SocialAlloy.Web.Social.Pages;
using EPiServer.SocialAlloy.Web.Social.Repositories;
using EPiServer.SocialAlloy.Web.Social.Repositories.Moderation;
using EPiServer.Web;
using EPiServer.Web.Routing;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using StructureMap;
using System;
using System.Globalization;
using System.Linq;
using System.Web.Routing;

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
        private SocialGroupRepository groupRepository;
        private SocialModerationRepository moderationRepository;
        private UrlResolver urlResolver;

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
            var contentEvents = ServiceLocator.Current.GetInstance<IContentEvents>();
            groupRepository = ServiceLocator.Current.GetInstance<SocialGroupRepository>();
            moderationRepository = ServiceLocator.Current.GetInstance<SocialModerationRepository>();
            urlResolver = ServiceLocator.Current.GetInstance<UrlResolver>();
            contentEvents.CreatingContent += SociaCommunityPage_CreationEvent;
           // contentEvents.SavedContent += SociaCommunityPage_PublishedEvent;
        }

        /// <summary>
        /// Uninitializes this instance.
        /// </summary>
        /// <param name="context">The instance context.</param>
        public void Uninitialize(InitializationEngine context)
        {
            var contentEvents = ServiceLocator.Current.GetInstance<IContentEvents>();
            contentEvents.CreatingContent -= SociaCommunityPage_CreationEvent;
           // contentEvents.SavedContent -= SociaCommunityPage_PublishedEvent;
        }

        /// <summary>
        /// Community Event for when a SocialCommunityPage is published
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SociaCommunityPage_PublishedEvent(object sender, EPiServer.ContentEventArgs e)
        {
            if (e.Content is SocialCommunityPage)
            {

                var communityPage = e.Content as SocialCommunityPage;

                var groupName = communityPage.Name;
                var url1 = UrlResolver.Current.GetUrl(communityPage.ContentLink);
                var group = groupRepository.Get(groupName);
                var lang = new CultureInfo("en-US").Name;
                //var content = contentre
                //var url = UrlResolver.Current.GetUrl(communityPage.ContentLink, null, new VirtualPathArguments { ContextMode = ContextMode.Default });
                var cotentRepo = ServiceLocator.Current.GetInstance<IContentRepository>();

                var result = ServiceLocator.Current
                                           .GetInstance<UrlResolver>()
                                           .GetUrl(
                                                    communityPage.ContentLink,
                                                    lang,
                                                    new VirtualPathArguments
                                                    {
                                                        ContextMode = ContextMode.Default,
                                                        ValidateTemplate = false
                                                    }
                                                );

                var communityPageURL2 = urlResolver.GetVirtualPath(communityPage.ContentLink);
                // var pr = communityPage.ContentLink.ToPageReference();
                // pr.
               // group.PageLink = result; //.VirtualPath;

                //var requestContext = new RequestContext { RouteData = new RouteData() }; //Also set the fake HTTP Context if required
                //foreach (RouteBase route in RouteTable.Routes.Where(r => r is IContentRoute))
                //{
                //    // Create new route values for each route in case they are manipulated by the route
                //    var contentRouteValues = new RouteValueDictionary
                //    {
                //      {RoutingConstants.NodeKey, communityPage.ContentLink},
                //      {RoutingConstants.LanguageKey, lang},
                //    };
                //    var virtualPath = route.GetVirtualPath(requestContext, contentRouteValues);

                //    var path = virtualPath.GetUrl();

                   group = groupRepository.Update(group);

                //}
            }
        }
            /// <summary>
            /// Community Event for the creation of any SocialCommunityPage
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            void SociaCommunityPage_CreationEvent(object sender, EPiServer.ContentEventArgs e)
    
            {
                if (e.Content is SocialCommunityPage)
                {
                    var communityPage = e.Content as SocialCommunityPage;

                //Build a new group 
                var groupName = communityPage.Name;
                    communityPage.MetaDescription = String.IsNullOrEmpty(communityPage.MetaDescription) ? "The is the home page for \"" + groupName + "\"" : communityPage.MetaDescription;
                    var groupDescription = communityPage.MetaDescription;
                    var socialGroup = new SocialGroup(groupName, groupDescription);
                    socialGroup.PageLink = "";

                //Add group to Social
                var group = groupRepository.Add(socialGroup);



                //Configure CommentBlock
                communityPage.Comments.Heading = groupName + " Comments";
                    communityPage.Comments.ShowHeading = true;

                //Configure SubscriptionBlock
                communityPage.Subscriptions.ShowHeading = false;

                //Configure RatingsBlock
                communityPage.Ratings.Heading = groupName + " Page Rating";
                    communityPage.Ratings.ShowHeading = true;

                //Configure GroupAdmissionBlock
                communityPage.GroupAdmission.GroupName = groupName;
                    communityPage.GroupAdmission.ShowHeading = true;
                    communityPage.GroupAdmission.Heading = groupName + " Admission Form";

                //Configure MembershipBlock
                communityPage.Memberships.GroupName = groupName;
                    communityPage.Memberships.ShowHeading = true;
                    communityPage.Memberships.Heading = groupName + " Member List"; ;
                }
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
            configuration.For<ISocialModerationRepository>().Use<SocialModerationRepository>();
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