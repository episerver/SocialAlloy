using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EPiServer.SocialAlloy.Web.Social.Models;
using EPiServer.Social.Common;
using EPiServer.Social.ActivityStreams.Core;
using EPiServer.SocialAlloy.Web.Social.Common.Exceptions;

namespace EPiServer.SocialAlloy.Web.Social.Repositories
{
    /// <summary>
    /// The ISocialFeedRepository class defines the operations that can be issued
    /// against a feed item repository.
    /// </summary>
    public class SocialFeedRepository : ISocialFeedRepository
    {
        private readonly IUserRepository userRepository;
        private readonly IFeedService feedService;
        private readonly IContentRepository contentRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        public SocialFeedRepository(IUserRepository userRepository, IFeedService feedService, IContentRepository contentRepository)
        {
            this.userRepository = userRepository;
            this.feedService = feedService;
            this.contentRepository = contentRepository;
        }

        /// <summary>
        /// Gets feed items from the underlying feed repository based on a filter.
        /// </summary>
        /// <param name="filter">a feed item filter to filter the feed items by</param>
        /// <returns>A list of feed items.</returns>
        public IEnumerable<SocialFeedViewModel> Get(SocialFeedFilter filter)
        {
            var feedItems = new List<Composite<FeedItem, SocialActivity>>();

            try
            {
                feedItems = GetMockData();

                //feedItems = this.feedService.Get(
                //    new CompositeCriteria<FeedItemFilter, SocialActivity>
                //    {
                //        PageInfo = new PageInfo
                //        {
                //            PageSize = filter.PageSize
                //        },
                //        Filter = new FeedItemFilter
                //        {
                //            Subscriber = Reference.Create(filter.Subscriber)
                //        }
                //        ,
                //        OrderBy = { new SortInfo(FeedItemSortFields.ActivityDate, false) }
                //    }
                //).Results.ToList();
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

        private List<Composite<FeedItem, SocialActivity>> GetMockData()
        {
            List<Composite<FeedItem, SocialActivity>> results = new List<Composite<FeedItem, SocialActivity>>();

            results.Add(new Composite<FeedItem, SocialActivity>(
                new FeedItem { ActivityDate = DateTime.UtcNow, Actor = Reference.Create("2b392142-d275-4559-b25b-7e40747f368c"), Target = Reference.Create("18a8d890-055e-48c8-a1bb-7dcd156e7a28") },
                new SocialCommentActivity { Body = "comment body" }
                ));

            results.Add(new Composite<FeedItem, SocialActivity>(
                new FeedItem { ActivityDate = DateTime.UtcNow, Actor = Reference.Create("2b392142-d275-4559-b25b-7e40747f368c"), Target = Reference.Create("1ae8531b-e5ac-4b82-af2c-7bd83e83b4a2") },
                new SocialRatingActivity { Value = 5 }
                ));

            return results;
        }

        private IEnumerable<SocialFeedViewModel> AdaptSocialActivityFeedItems(List<Composite<FeedItem, SocialActivity>> feedItems)
        {
            SocialActivityAdapter adapter = new SocialActivityAdapter(userRepository, contentRepository);
            return feedItems.Select(c => adapter.Adapt(c));
        }

        
    }
}