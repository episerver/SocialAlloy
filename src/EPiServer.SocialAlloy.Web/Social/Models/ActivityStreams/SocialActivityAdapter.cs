using EPiServer.Core;
using EPiServer.Social.ActivityStreams.Core;
using EPiServer.Social.Common;
using EPiServer.SocialAlloy.Web.Social.Repositories;
using System;

namespace EPiServer.SocialAlloy.Web.Social.Models
{
    /// <summary>
    /// The SocialActivityAdapter class encapsulates a set of operations used for adapting and 
    /// interpreting various types of Social activities in the Episerver Social sample and 
    /// converting them to a SocialFeedViewModel.
    /// </summary>
    public class SocialActivityAdapter : ISocialActivityAdapter
    {
        private SocialFeedViewModel feedModel;
        private IUserRepository userRepository;
        private IContentRepository contentRepository;
        private IPageRepository pageRepository;

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
        /// Adpats a Composite feed item with SocialActivity as payload to a SocialFeedViewModel
        /// </summary>
        /// <param name="composite"></param>
        /// <returns></returns>
        public SocialFeedViewModel Adapt(Composite<FeedItem, SocialActivity> composite)
        {
            // Create and populate the SocialFeedViewModel 
            feedModel = new SocialFeedViewModel
            {
                ActivityDate = composite.Data.ActivityDate,
                Actor = userRepository.GetUserName(composite.Data.Actor.Id),
                Target = composite.Data.Target.Id
            };

            //Interpret the activity
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
            var pageName = this.pageRepository.GetPageName(feedModel.Target);
            feedModel.Description = String.Format("{0} commented on {1} page [{2}] on {3}", 
                                    feedModel.Actor, pageName, activity.Body, feedModel.ActivityDate.ToLocalTime());
        }

        /// <summary>
        /// Interprets a SocialRatingActivity
        /// </summary>
        /// <param name="activity">the SocialRatingActivity to interpret</param>
        public void Visit(SocialRatingActivity activity)
        {
            // Interpret activity and set description.
            var pageName = this.pageRepository.GetPageName(feedModel.Target);
            feedModel.Description = String.Format("{0} rated {1} page with a [{2}] on {3}",
                                    feedModel.Actor, pageName, activity.Value.ToString(), feedModel.ActivityDate.ToLocalTime());
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