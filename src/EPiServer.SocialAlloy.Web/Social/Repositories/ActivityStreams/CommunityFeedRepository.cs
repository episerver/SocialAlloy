using EPiServer.Social.ActivityStreams.Core;
using EPiServer.Social.Common;
using EPiServer.SocialAlloy.Web.Social.Adapters;
using EPiServer.SocialAlloy.Web.Social.Common.Exceptions;
using EPiServer.SocialAlloy.Web.Social.Models;
using System.Collections.Generic;
using System.Linq;

namespace EPiServer.SocialAlloy.Web.Social.Repositories
{
    /// <summary>
    /// The CommunityFeedRepository class implements the operations for accessing feeds of community activities. 
    /// </summary>
    public class CommunityFeedRepository : ICommunityFeedRepository
    {
        private readonly IFeedService feedService;
        private readonly CommunityActivityAdapter activityAdapter;
        private readonly FeedItemFilters feedItemFilters;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userRepository">an instance of the user repository</param>
        /// <param name="pageRepository">an instance of the page repository</param>
        /// <param name="feedService">an instance of the Social Feed Service</param>
        /// <param name="adapter">an instance of the CommunityActivityAdapter</param>
        public CommunityFeedRepository(IUserRepository userRepository, 
                                       IPageRepository pageRepository,
                                       IFeedService feedService)
        {
            this.feedService = feedService;
            this.activityAdapter = new CommunityActivityAdapter(userRepository, pageRepository);
            this.feedItemFilters = new FeedItemFilters();
        }

        /// <summary>
        /// Gets feed items from the underlying feed repository based on a filter.
        /// </summary>
        /// <param name="filter">a filter by which to retrieve feed items by</param>
        /// <returns>A list of feed items.</returns>
        /// <remarks>
        /// This shows an important aspect of the social platform. Activity feeds were
        /// generated using either a PageCommentActivity or PageRatingActivity model.
        /// We can use a totally different model to retrieve the activity feed data.
        /// The CommunityActivity has both a Body and Value property. The platform will
        /// fill either property in the CommunityActivity model depending on what the
        /// feed item is. If reviews were supported then the feed would contain both
        /// pieces of data (a comment body and a rating value) and the platform would
        /// have filled the output CommunityActivity model with both pieces of data.
        /// </remarks>
        public IEnumerable<CommunityFeedItemViewModel> Get(CommunityFeedFilter filter)
        {
            try
            {
                // Only get feed items that match the specified subscriber
                var subscriberFilter = this.feedItemFilters.Subscriber.EqualTo(Reference.Create(filter.Subscriber));

                // Also ensure that only feed for page activities are retrieved
                var pageCommentActivityTypeIdFilter = this.feedItemFilters.Extension.Type.Is(PageCommentActivity.TypeId);
                var pageRatingActivityTypeIdFilter = this.feedItemFilters.Extension.Type.Is(PageRatingActivity.TypeId);
                var typeIdFilter = new OrExpression(pageCommentActivityTypeIdFilter, pageRatingActivityTypeIdFilter);

                // The final filter...
                var feedItemFilter = new AndExpression(subscriberFilter, typeIdFilter);

                var feedItems = this.feedService.Get<CommunityActivity>(
                    new Criteria
                    {
                        Filter = feedItemFilter,
                        PageInfo = new PageInfo { PageSize = filter.PageSize }, 
                        OrderBy = { new SortInfo(FeedItemSortFields.ActivityDate, false) },
                    }
                ).Results.ToList();

                return AdaptFeedItems(feedItems);
            }
            catch (SocialAuthenticationException ex)
            {
                throw new SocialRepositoryException("The application failed to authenticate with Episerver Social.", ex);
            }
            catch (MaximumDataSizeExceededException ex)
            {
                throw new SocialRepositoryException("The application request was deemed too large for Episerver Social.", ex);
            }
            catch (SocialCommunicationException ex)
            {
                throw new SocialRepositoryException("The application failed to communicate with Episerver Social.", ex);
            }
            catch (SocialException ex)
            {
                throw new SocialRepositoryException("Episerver Social failed to process the application request.", ex);
            }
        }

        private IEnumerable<CommunityFeedItemViewModel> AdaptFeedItems(List<FeedItem<CommunityActivity>> feedItems)
        {
            return feedItems.Select(c => this.activityAdapter.Adapt(c));
        }
    }
}