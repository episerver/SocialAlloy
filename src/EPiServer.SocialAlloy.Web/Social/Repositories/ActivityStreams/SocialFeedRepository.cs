using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EPiServer.SocialAlloy.Web.Social.Models;
using EPiServer.Social.Common;
using EPiServer.Social.ActivityStreams.Core;
using EPiServer.SocialAlloy.Web.Social.Common.Exceptions;
using EPiServer.SocialAlloy.Web.Social.Adapters;

namespace EPiServer.SocialAlloy.Web.Social.Repositories
{
    /// <summary>
    /// The ISocialFeedRepository class defines the operations that can be issued
    /// against the Episerver Social feed item repository.
    /// </summary>
    public class SocialFeedRepository : ISocialFeedRepository
    {
        private readonly IUserRepository userRepository;
        private readonly IFeedService feedService;
        private readonly IContentRepository contentRepository;
        private readonly ISocialActivityAdapter activityAdapter;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userRepository">an instance of the user repository</param>
        /// <param name="feedService">an instance of the Social Feed Service</param>
        /// <param name="contentRepository">an instance of the Episerver's content repository</param>
        /// <param name="adapter">an instance of the SocialActivityAdapter</param>
        public SocialFeedRepository(IUserRepository userRepository, 
                                    IFeedService feedService, 
                                    IContentRepository contentRepository,
                                    ISocialActivityAdapter adapter)
        {
            this.userRepository = userRepository;
            this.feedService = feedService;
            this.contentRepository = contentRepository;
            this.activityAdapter = adapter;
        }

        /// <summary>
        /// Gets feed items from the underlying feed repository based on a filter.
        /// </summary>
        /// <param name="filter">a filter by which to retrieve feed items by</param>
        /// <returns>A list of feed items.</returns>
        public IEnumerable<SocialFeedViewModel> Get(SocialFeedFilter filter)
        {
            var feedItems = new List<Composite<FeedItem, SocialActivity>>();

            try
            {
                feedItems = this.feedService.Get(
                    new CompositeCriteria<FeedItemFilter, SocialActivity>
                    {
                        PageInfo = new PageInfo
                        {
                            PageSize = filter.PageSize
                        },
                        IncludeSubclasses = true,
                        Filter = new FeedItemFilter
                        {
                            Subscriber = Reference.Create(filter.Subscriber)
                        }
                        ,
                        OrderBy = { new SortInfo(FeedItemSortFields.ActivityDate, false) }
                    }
                ).Results.ToList();
            }
            catch (SocialAuthenticationException ex)
            {
                throw new SocialRepositoryException("The application failed to authenticate with EPiServer Social.", ex);
            }
            catch (MaximumDataSizeExceededException ex)
            {
                throw new SocialRepositoryException("The application request was deemed too large for EPiServer Social.", ex);
            }
            catch (SocialCommunicationException ex)
            {
                throw new SocialRepositoryException("The application failed to communicate with EPiServer Social.", ex);
            }
            catch (SocialException ex)
            {
                throw new SocialRepositoryException("EPiServer Social failed to process the application request.", ex);
            }

            return AdaptSocialActivityFeedItems(feedItems);
        }

        private IEnumerable<SocialFeedViewModel> AdaptSocialActivityFeedItems(List<Composite<FeedItem, SocialActivity>> feedItems)
        {
            return feedItems.Select(c => this.activityAdapter.Adapt(c));
        }
    }
}