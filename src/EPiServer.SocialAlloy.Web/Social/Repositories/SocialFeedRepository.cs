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
    public class SocialFeedRepository : ISocialFeedRepository
    {
        private readonly IUserRepository userRepository;
        private readonly IFeedService feedService;

        /// <summary>
        /// Constructor
        /// </summary>
        public SocialFeedRepository(IUserRepository userRepository, IFeedService feedService)
        {
            this.userRepository = userRepository;
            this.feedService = feedService;
        }

        public IEnumerable<SocialActivityFeedViewModel> Get(SocialFeedFilter filter)
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
                new FeedItem { ActivityDate = DateTime.UtcNow, Actor = Reference.Create("a1"), Target = Reference.Create("1") },
                new SocialCommentActivity { Body = "comment body" }
                ));

            results.Add(new Composite<FeedItem, SocialActivity>(
                new FeedItem { ActivityDate = DateTime.UtcNow, Actor = Reference.Create("a2"), Target = Reference.Create("2") },
                new SocialRatingActivity { Value = 5 }
                ));

            return results;
        }

        private IEnumerable<SocialActivityFeedViewModel> AdaptSocialActivityFeedItems(List<Composite<FeedItem, SocialActivity>> feedItems)
        {
            SocialActivityAdapter adapter = new SocialActivityAdapter();
            return feedItems.Select(c => adapter.Adapt(c));
        }

        
    }
}