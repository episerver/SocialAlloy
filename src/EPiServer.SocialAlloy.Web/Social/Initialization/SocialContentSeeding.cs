using EPiServer.Core;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.Security;
using EPiServer.ServiceLocation;
using EPiServer.SocialAlloy.Web.Models.Pages;
using EPiServer.SocialAlloy.Web.Social.Pages;
using EPiServer.Web;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace EPiServer.SocialAlloy.Web.Social.Initialization
{
    /// <summary>
    /// The SocialContentSeeding class populates the default alloy site with social community specific content.
    /// The pages created include a profile page, a reseller community title page and three reseller community pages. 
    /// This module is dependant on the SocialInitialization module. 
    /// </summary>
    [InitializableModule]
    [ModuleDependency(typeof(SocialInitialization))]
    public class SocialContentSeeding : IInitializableModule
    {
        /// <summary>
        /// Initializes this instance.
        /// </summary>
        /// <param name="context">The instance context.</param>
        public void Initialize(InitializationEngine context)
        {
            ContentSeeding();
        }

        /// <summary>
        /// Seeding the site with reseller community and profile page.
        /// </summary>
        private void ContentSeeding()
        {
            var contentRepository = ServiceLocator.Current.GetInstance<IContentRepository>();
            var urlSegmentCreator = ServiceLocator.Current.GetInstance<IUrlSegmentCreator>();
            ResellerCommunitySeeding(contentRepository, urlSegmentCreator);
            ProfilePageSeeding(contentRepository, urlSegmentCreator);
        }

        /// <summary>
        /// This method checks to see if the profile page already exists. 
        /// If no profile page can be found a new one will be created on application start.
        /// </summary>
        /// <param name="contentRepository"></param>
        /// <param name="urlSegmentCreator"></param>
        private void ProfilePageSeeding(IContentRepository contentRepository, IUrlSegmentCreator urlSegmentCreator)
        {
            var profilePage = contentRepository.GetBySegment(PageReference.StartPage, "my-profile", CultureInfo.CurrentCulture);
            if (profilePage == null)
            {
                SocialProfilePage myPage = contentRepository.GetDefault<SocialProfilePage>(PageReference.StartPage);
                myPage.PageName = "My Profile";
                myPage.URLSegment = urlSegmentCreator.Create(myPage);
                myPage.MainBody = new XhtmlString("<p>This is page is your personal profile page!</p>");
                myPage.VisibleInMenu = true;
                myPage.Feed.FeedTitle = "Your Activity Feed";
                myPage.Feed.ShowHeading = true;
                myPage.Feed.Heading = "Profile Feed";
                myPage.MembershipAffiliation.Heading = "Your Groups";
                myPage.MembershipAffiliation.ShowHeading = true;
                contentRepository.Save(myPage, DataAccess.SaveAction.Publish, AccessLevel.NoAccess);
            }
        }

        /// <summary>
        /// This method is used to seed the content for the Reseller Community Pages.
        /// </summary>
        /// <param name="contentRepository"></param>
        /// <param name="urlSegmentCreator"></param>
        private void ResellerCommunitySeeding(IContentRepository contentRepository, IUrlSegmentCreator urlSegmentCreator)
        {
            //Checking to see if the reseller title page already exists. If no reseller title page can be found a new one will be created on application start.
            var resellerTitlePage = contentRepository.GetBySegment(PageReference.StartPage, "reseller-community", CultureInfo.CurrentCulture);
            if (resellerTitlePage == null)
            {
                StandardPage communityPage = contentRepository.GetDefault<StandardPage>(PageReference.StartPage);
                communityPage.PageName = "Reseller Community";
                communityPage.URLSegment = urlSegmentCreator.Create(communityPage);
                communityPage.MainBody = new XhtmlString("<p>This is the homepage for all reseller community pages.</p>");
                communityPage.VisibleInMenu = true;
                contentRepository.Save(communityPage, EPiServer.DataAccess.SaveAction.Publish, AccessLevel.NoAccess);
            }

            resellerTitlePage = contentRepository.GetBySegment(PageReference.StartPage, "reseller-community", CultureInfo.CurrentCulture);
            //If the title reseller page exists there is a check if there are any child pages. If no child pages exist then create example reseller pages. 
            if (resellerTitlePage != null)
            {
                var parentReference = resellerTitlePage.ContentLink;
                var resellerTitlePageChildren = contentRepository.GetChildren<SocialCommunityPage>(parentReference);
                if (resellerTitlePageChildren != null && resellerTitlePageChildren.Any() != true)
                {
                    var listOfGroups = new List<string> { "Platinum Reseller Group", "Gold Reseller Group", "Silver Reseller Group" };
                    foreach (var group in listOfGroups)
                    {
                        SocialCommunityPage resellerGroupPage = contentRepository.GetDefault<SocialCommunityPage>(parentReference);
                        resellerGroupPage.PageName = group;
                        resellerGroupPage.URLSegment = urlSegmentCreator.Create(resellerGroupPage);
                        resellerGroupPage.VisibleInMenu = true;
                        contentRepository.Save(resellerGroupPage, EPiServer.DataAccess.SaveAction.Publish, AccessLevel.NoAccess);
                    }
                }
            }
        }

        /// <summary>
        /// Uninitializes this instance.
        /// </summary>
        /// <param name="context">The instance context.</param>
        public void Uninitialize(InitializationEngine context)
        {
        }
    }
}