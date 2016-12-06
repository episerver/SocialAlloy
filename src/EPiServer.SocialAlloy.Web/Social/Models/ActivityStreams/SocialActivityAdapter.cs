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

        /// <summary>
        /// Constructor
        /// </summary>
        public SocialActivityAdapter(IUserRepository userRepository, IContentRepository contentRepository)
        {
            this.userRepository = userRepository;
            this.contentRepository = contentRepository;
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
            feedModel.Description = String.Format("Commented on the page: {0}", activity.Body);

            // Replace page Id of target with Page Name
            feedModel.Target = GetPageName(feedModel.Target);
        }

        /// <summary>
        /// Interprets a SocialRatingActivity
        /// </summary>
        /// <param name="activity">the SocialRatingActivity to interpret</param>
        public void Visit(SocialRatingActivity activity)
        {
            // Interpret activity and set description.
            feedModel.Description = String.Format("Rated the page: {0}", activity.Value.ToString());

            // Replace page Id of target with Page Name
            feedModel.Target = GetPageName(feedModel.Target);
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

        private string GetPageName(string pageId)
        {
            var pageName = String.Empty;
            try
            {
                Guid g = Guid.Parse(pageId);
                var data = contentRepository.Get<PageData>(g);
                pageName = data.Name;
            }
            catch (ContentNotFoundException)
            {
                pageName = "Could not determine the name of the page with Id: " + pageId;
            }
            catch (TypeMismatchException e)
            {
                //IF not PageData type of content Guid
            }

            return pageName;
        }
    }
}