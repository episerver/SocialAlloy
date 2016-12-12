using EPiServer.Social.ActivityStreams.Core;
using EPiServer.Social.Common;
using EPiServer.SocialAlloy.Web.Social.Models;
using EPiServer.SocialAlloy.Web.Social.Repositories;
using System;

namespace EPiServer.SocialAlloy.Web.Social.Adapters
{
    /// <summary>
    /// The SocialActivityAdapter class encapsulates a set of operations used for adapting and 
    /// interpreting various types of Social activities in the Episerver Social sample and 
    /// converting them to a SocialFeedItemViewModel.
    /// </summary>
    public class SocialActivityAdapter : ISocialActivityAdapter
    {
        private SocialFeedItemViewModel feedModel;
        private IUserRepository userRepository;
        private IPageRepository pageRepository;
        private string actor;
        private string pageName;

        /// <summary>
        /// Constructor
        /// </summary>
        public SocialActivityAdapter(IUserRepository userRepository, 
                                     IContentRepository contentRepository,
                                     IPageRepository pageRepository)
        {
            this.userRepository = userRepository;
            this.pageRepository = pageRepository;
        }

        /// <summary>
        /// Adpats a Composite feed item with SocialActivity as payload to a SocialFeedItemViewModel
        /// </summary>
        /// <param name="composite"></param>
        /// <returns></returns>
        public SocialFeedItemViewModel Adapt(Composite<FeedItem, SocialActivity> composite)
        {
            // Create and populate the SocialFeedItemViewModel 
            feedModel = new SocialFeedItemViewModel
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
        /// Interprets a SocialCommentActivity
        /// </summary>
        /// <param name="activity">the SocialCommentActivity to interpret</param>
        public void Visit(SocialCommentActivity activity)
        {
            // Interpret activity and set description.
            feedModel.Heading = String.Format("{0} commented on \"{1}\".", this.actor, pageName);
            feedModel.Description = activity.Body;
        }

        /// <summary>
        /// Interprets a SocialRatingActivity
        /// </summary>
        /// <param name="activity">the SocialRatingActivity to interpret</param>
        public void Visit(SocialRatingActivity activity)
        {
            // Interpret activity and set description.
            feedModel.Heading = String.Format("{0} rated \"{1}\" with a {2}.", this.actor, pageName, activity.Value);
        }

        /// <summary>
        /// Interprets a SocialActivity
        /// </summary>
        /// <param name="activity">the SocialActivity to interpret</param>
        public void Visit(SocialActivity activity)
        {
            activity.Accept(this);
        }

        #endregion

        
    }
}