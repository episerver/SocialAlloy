using EPiServer.Social.ActivityStreams.Core;
using EPiServer.Social.Common;
using EPiServer.SocialAlloy.Web.Social.Models;
using EPiServer.SocialAlloy.Web.Social.Repositories;
using System;

namespace EPiServer.SocialAlloy.Web.Social.Adapters
{
    /// <summary>
    /// The CommunityActivityAdapter class encapsulates a set of operations used for adapting and 
    /// interpreting various types of Social activities in the Episerver Social sample and 
    /// converting them to a CommunityFeedItemViewModel.
    /// </summary>
    public class CommunityActivityAdapter : ICommunityActivityAdapter
    {
        private CommunityFeedItemViewModel feedModel;
        private IUserRepository userRepository;
        private IPageRepository pageRepository;
        private string actor;
        private string pageName;

        /// <summary>
        /// Constructor
        /// </summary>
        public CommunityActivityAdapter(IUserRepository userRepository, 
                                     IContentRepository contentRepository,
                                     IPageRepository pageRepository)
        {
            this.userRepository = userRepository;
            this.pageRepository = pageRepository;
        }

        /// <summary>
        /// Adpats a Composite feed item with CommunityActivity as payload to a CommunityFeedItemViewModel
        /// </summary>
        /// <param name="composite"></param>
        /// <returns></returns>
        public CommunityFeedItemViewModel Adapt(Composite<FeedItem, CommunityActivity> composite)
        {
            // Create and populate the CommunityFeedItemViewModel 
            feedModel = new CommunityFeedItemViewModel
            {
                ActivityDate = composite.Data.ActivityDate,
            };

            this.actor = userRepository.GetUserName(composite.Data.Actor.Id);
            this.pageName = this.pageRepository.GetPageName(composite.Data.Target.Id);

            // Interpret the activity
            composite.Extension.Accept(this);

            return this.feedModel;
        }

        #region ISocialActivityAdapter methods

        /// <summary>
        /// Interprets a PageCommentActivity
        /// </summary>
        /// <param name="activity">the PageCommentActivity to interpret</param>
        public void Visit(PageCommentActivity activity)
        {
            // Interpret activity and set description.
            feedModel.Heading = String.Format("{0} commented on \"{1}\".", this.actor, pageName);
            feedModel.Description = activity.Body;
        }

        /// <summary>
        /// Interprets a PageRatingActivity
        /// </summary>
        /// <param name="activity">the PageRatingActivity to interpret</param>
        public void Visit(PageRatingActivity activity)
        {
            // Interpret activity and set description.
            feedModel.Heading = String.Format("{0} rated \"{1}\" with a {2}.", this.actor, pageName, activity.Value);
        }

        /// <summary>
        /// Interprets a CommunityActivity
        /// </summary>
        /// <param name="activity">the CommunityActivity to interpret</param>
        public void Visit(CommunityActivity activity)
        {
            activity.Accept(this);
        }

        #endregion

        
    }
}