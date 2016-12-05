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
    public class SocialActivityAdapter : ISocialActivityVisitor
    {
        private SocialFeedViewModel feedModel;
        private IUserRepository userRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        public SocialActivityAdapter(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        /// <summary>
        /// Adpats a Composite feed item with SocialActivity as payload to a SocialFeedViewModel
        /// </summary>
        /// <param name="composite"></param>
        /// <returns></returns>
        public SocialFeedViewModel Adapt(Composite<FeedItem, SocialActivity> composite)
        {
            // Adapt any composite.Data stuff and populate view model accordingly…
            feedModel = new SocialFeedViewModel
            {
                ActivityDate = composite.Data.ActivityDate,
                Actor = (!String.IsNullOrWhiteSpace(composite.Data.Actor.Id))
                        ? userRepository.GetUser(composite.Data.Actor.Id).Name
                        : User.Anonymous.Name,
                Target = composite.Data.Target.Id //TODO get page name from Id
            };

            //Interpret the activity and create a description for the 
            composite.Extension.Accept(this);

            return this.feedModel;
        }

        /// <summary>
        /// Interprets a SocialCommentActivity
        /// </summary>
        /// <param name="activity">the SocialCommentActivity to interpret</param>
        public void Visit(SocialCommentActivity activity)
        {
            feedModel.Description = String.Format("Commented on the page: {0}", activity.Body);
        }

        /// <summary>
        /// Interprets a SocialRatingActivity
        /// </summary>
        /// <param name="activity">the SocialRatingActivity to interpret</param>
        public void Visit(SocialRatingActivity activity)
        {
            feedModel.Description = String.Format("Rated the page: {0}", activity.Value.ToString());
        }

        /// <summary>
        /// Interprets a SocialActivity
        /// </summary>
        /// <param name="activity">the SocialActivity to interpret</param>
        public void Visit(SocialActivity activity)
        {
            activity.Accept(this);
        }
    }
}