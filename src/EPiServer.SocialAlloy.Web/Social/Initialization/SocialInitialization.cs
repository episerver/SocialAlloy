using EPiServer.Cms.UI.AspNetIdentity;
using EPiServer.Core;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using EPiServer.SocialAlloy.Web.Social.Adapters;
using EPiServer.SocialAlloy.Web.Social.Models.Groups;
using EPiServer.SocialAlloy.Web.Social.Pages;
using EPiServer.SocialAlloy.Web.Social.Repositories;
using EPiServer.SocialAlloy.Web.Social.Repositories.Moderation;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using StructureMap;
using System;

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
        private SocialGroupRepository GroupRepo;
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
            GroupRepo = ServiceLocator.Current.GetInstance<SocialGroupRepository>();
            contentEvents.CreatingContent += SociaCommunityPage_CreationEvent;
        }

        /// <summary>
        /// Uninitializes this instance.
        /// </summary>
        /// <param name="context">The instance context.</param>
        public void Uninitialize(InitializationEngine context)
        {
            var contentEvents = ServiceLocator.Current.GetInstance<IContentEvents>();
            contentEvents.CreatingContent -= SociaCommunityPage_CreationEvent;
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

                //Add a new group 
                var groupName = communityPage.Name;
                communityPage.MetaDescription = String.IsNullOrEmpty(communityPage.MetaDescription) ? "The is the home page for \"" + groupName + "\"" : communityPage.MetaDescription;
                var groupDescription = communityPage.MetaDescription;
                var group = GroupRepo.Add(new SocialGroup(groupName, groupDescription));

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