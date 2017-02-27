﻿using EPiServer.Social.ActivityStreams.Core;
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
        private readonly IUserRepository userRepository;
        private readonly IFeedService feedService;
        private readonly IContentRepository contentRepository;
        private readonly ICommunityActivityAdapter activityAdapter;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userRepository">an instance of the user repository</param>
        /// <param name="feedService">an instance of the Social Feed Service</param>
        /// <param name="contentRepository">an instance of the Episerver's content repository</param>
        /// <param name="adapter">an instance of the CommunityActivityAdapter</param>
        public CommunityFeedRepository(IUserRepository userRepository, IFeedService feedService, IContentRepository contentRepository, ICommunityActivityAdapter adapter)
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
        public IEnumerable<CommunityFeedItemViewModel> Get(CommunityFeedFilter filter)
        {
            var feedItems = new List<Composite<FeedItem, CommunityActivity>>();

            try
            {
                feedItems = this.feedService.Get(
                    new CompositeCriteria<FeedItemFilter, CommunityActivity>
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

            return AdaptFeedItems(feedItems);
        }

        private IEnumerable<CommunityFeedItemViewModel> AdaptFeedItems(List<Composite<FeedItem, CommunityActivity>> feedItems)
        {
            return feedItems.Select(c => this.activityAdapter.Adapt(c));
        }
    }
}