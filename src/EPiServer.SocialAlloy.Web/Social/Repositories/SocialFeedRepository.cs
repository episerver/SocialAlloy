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
                new FeedItem { ActivityDate = DateTime.UtcNow, Actor = Reference.Create("4404127d-966b-4f37-ab9c-0ac9dc340b2c"), Target = Reference.Create("18a8d890-055e-48c8-a1bb-7dcd156e7a28") },
                new SocialCommentActivity { Body = "comment body" }
                ));

            results.Add(new Composite<FeedItem, SocialActivity>(
                new FeedItem { ActivityDate = DateTime.UtcNow.AddDays(1), Actor = Reference.Create("a1003887-9c8a-41cc-93e3-b8dbca255204"), Target = Reference.Create("1ae8531b-e5ac-4b82-af2c-7bd83e83b4a2") },
                new SocialRatingActivity { Value = 5 }
                ));

            return results;
        }

        private IEnumerable<SocialFeedViewModel> AdaptSocialActivityFeedItems(List<Composite<FeedItem, SocialActivity>> feedItems)
        {
            return feedItems.Select(c => this.activityAdapter.Adapt(c));
        }
    }
}