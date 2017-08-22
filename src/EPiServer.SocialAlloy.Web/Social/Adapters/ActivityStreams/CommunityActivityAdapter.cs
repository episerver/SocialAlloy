using EPiServer.Social.ActivityStreams.Core;
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
    public class CommunityActivityAdapter
    {
        private CommunityFeedItemViewModel feedModel;
        private IUserRepository userRepository;
        private IPageRepository pageRepository;
        private string actor;
        private string pageName;

        /// <summary>
        /// Constructor
        /// </summary>
        public CommunityActivityAdapter(IUserRepository userRepository, IPageRepository pageRepository)
        {
            this.userRepository = userRepository;
            this.pageRepository = pageRepository;
        }

        /// <summary>
        /// Adpats a Composite feed item with CommunityActivity as payload to a CommunityFeedItemViewModel
        /// </summary>
        /// <param name="composite"></param>
        /// <returns></returns>
        public CommunityFeedItemViewModel Adapt(FeedItem<CommunityActivity> composite)
        {
            // Create and populate the CommunityFeedItemViewModel 
            feedModel = new CommunityFeedItemViewModel
            {
                ActivityDate = composite.Data.ActivityDate,
            };

            this.actor = userRepository.GetUserName(composite.Data.Actor.Id);
            this.pageName = this.pageRepository.GetPageName(composite.Data.Target.Id);

            // Interpret the activity
            feedModel.Heading = AdaptHeading(composite);
            feedModel.Description = AdaptDescription(composite);

            return this.feedModel;
        }

        private string AdaptHeading(FeedItem<CommunityActivity> feedItem)
        {
            var heading = "";
            var activity = feedItem.Extension as CommunityActivity;

            // if reviews were supported we may have done something like the following:
            /*if (activity.commentBody != null && activity.ratingValue != null)
            {
                heading = String.Format("{0} reviewed \"{1}\".", feedItem.Data.Actor, pageName);
            } else ...*/

            if (activity.Body != null)
            {
                heading = String.Format($"{this.actor} commented on {this.pageName}");
            }
            else if (activity.Value != null)
            {
                heading = String.Format($"{this.actor} rated {this.pageName} with a {activity.Value}");
            }

            return heading;
        }

        private string AdaptDescription(FeedItem<CommunityActivity> feedItem)
        {
            var description = "";
            var activity = feedItem.Extension as CommunityActivity;

            if (activity.Body != null)
            {
                description = activity.Body;
            }

            return description;
        }
    }
}