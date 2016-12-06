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
    public class SocialActivityAdapter : ISocialActivityVisitor
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
            // For now assuming PageId but could be userId if user is following User
            var pageName = GetPageName(composite.Data.Target.Id);

            // Get user name of Actor in the activity
            var userName = GetUserName(composite.Data.Actor.Id);

            // Create and populate the SocialFeedViewModel 
            feedModel = new SocialFeedViewModel
            {
                ActivityDate = composite.Data.ActivityDate,
                Actor = userName,
                Target = pageName
            };

            //Interpret the activity and create a description for the 
            composite.Extension.Accept(this);

            return this.feedModel;
        }

        #region ISocialActivityVisitor methods

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

        #endregion

        private string GetUserName(string userId)
        {
            return (!String.IsNullOrWhiteSpace(userId))
                    ? userRepository.GetUser(userId).Name
                    : User.Anonymous.Name;
        }

        private string GetPageName(string pageId)
        {
            var pageName = String.Empty;
            try
            {
                Guid g = Guid.Parse(pageId);
                var data = contentRepository.Get<PageData>(g);
                pageName = data.Name;
            }
            catch (ContentNotFoundException e)
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