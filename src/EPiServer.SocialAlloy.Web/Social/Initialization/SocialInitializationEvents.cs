using EPiServer.Core;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using EPiServer.SocialAlloy.Web.Social.Models.Groups;
using EPiServer.SocialAlloy.Web.Social.Pages;
using EPiServer.SocialAlloy.Web.Social.Repositories;
using EPiServer.SocialAlloy.Web.Social.Repositories.Moderation;
using System;


namespace EPiServer.SocialAlloy.Web.Social.Initialization
{
    /// <summary>
    /// The SocialInitialization class initializes the IOC container mapping social component
    /// interfaces to their implementations.
    /// </summary>
    [InitializableModule]
    [ModuleDependency(typeof(SocialInitialization))]
    public class SocialInitializationEvents : IInitializableModule
    {
        private CommunityRepository groupRepository;
        private CommunityMembershipModerationRepository moderationRepository;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        /// <param name="context">The instance context.</param>
        public void Initialize(InitializationEngine context)
        {
            var contentEvents = context.Locate.ContentEvents();

            groupRepository = ServiceLocator.Current.GetInstance<CommunityRepository>();
            moderationRepository = ServiceLocator.Current.GetInstance<CommunityMembershipModerationRepository>();

            contentEvents.CreatingContent += SociaCommunityPage_CreationEvent;
            contentEvents.SavedContent += SociaCommunityPage_PublishedEvent;
        }

        /// <summary>
        /// Uninitializes this instance.
        /// </summary>
        /// <param name="context">The instance context.</param>
        public void Uninitialize(InitializationEngine context)
        {
            var contentEvents = ServiceLocator.Current.GetInstance<IContentEvents>();
            contentEvents.CreatingContent -= SociaCommunityPage_CreationEvent;
            contentEvents.SavedContent -= SociaCommunityPage_PublishedEvent;
        }

        /// <summary>
        /// Community Event for when a CommunityPage is published
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SociaCommunityPage_PublishedEvent(object sender, EPiServer.ContentEventArgs e)
        {
            if (e.Content is CommunityPage)
            {
                var communityPage = e.Content as CommunityPage;
                var groupName = communityPage.Name;
                var group = groupRepository.Get(groupName);
                var url = ExternalURL(communityPage);
                group.PageLink = url;
                group = groupRepository.Update(group);
            }
        }

        /// <summary>
        /// Community Event for the creation of any CommunityPage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SociaCommunityPage_CreationEvent(object sender, EPiServer.ContentEventArgs e)

        {
            if (e.Content is CommunityPage)
            {
                var communityPage = e.Content as CommunityPage;

                var communityName = communityPage.Name;
                Community community;

                //Check if the community name exists in Social
                //If it does, the page will use the existing Social group and workflow details
                community = groupRepository.Get(communityName);
                if (community == null)
                {
                    //Build a new group 
                    communityPage.MetaDescription = String.IsNullOrEmpty(communityPage.MetaDescription) ? "This is the home page for \"" + communityName + "\"" : communityPage.MetaDescription;
                    var groupDescription = communityPage.MetaDescription;
                    var socialGroup = new Community(communityName, groupDescription);

                    //Add group to Social
                    socialGroup = groupRepository.Add(socialGroup);

                    //Add a workflow for the new group
                    this.moderationRepository.AddWorkflow(socialGroup);
                }

                //Configure CommentBlock
                communityPage.Comments.Heading = communityName + " Comments";
                communityPage.Comments.ShowHeading = true;
                communityPage.Comments.SendActivity = true;

                //Configure SubscriptionBlock
                communityPage.Subscriptions.ShowHeading = false;

                //Configure RatingsBlock
                communityPage.Ratings.Heading = communityName + " Page Rating";
                communityPage.Ratings.ShowHeading = true;
                communityPage.Ratings.SendActivity = true;

                //Configure GroupAdmissionBlock
                communityPage.GroupAdmission.GroupName = communityName;
                communityPage.GroupAdmission.ShowHeading = true;
                communityPage.GroupAdmission.Heading = communityName + " Admission Form";

                //Configure MembershipBlock
                communityPage.Memberships.GroupName = communityName;
                communityPage.Memberships.ShowHeading = true;
                communityPage.Memberships.Heading = communityName + " Member List";
            }
        }

        /// <summary>
        /// Returns absolute encoded url for a community page. 
        /// </summary>
        /// <param name="pageData">The page data that is associated with a specific community</param>
        /// <returns>The absolute encoded url of a community page </returns>
        private string ExternalURL(PageData pageData)
        {
            UrlBuilder pageURLBuilder = new UrlBuilder(pageData.LinkURL);
            EPiServer.Global.UrlRewriteProvider.ConvertToExternal(pageURLBuilder, pageData.PageLink, System.Text.Encoding.UTF8);
            string pageURL = pageURLBuilder.ToString();
            UriBuilder uriBuilder = new UriBuilder(UriSupport.SiteUrl);
            uriBuilder.Path = pageURL;
            return uriBuilder.Uri.AbsoluteUri;
        }

    }
}