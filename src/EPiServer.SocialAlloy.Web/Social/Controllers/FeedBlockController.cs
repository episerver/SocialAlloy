﻿using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.SocialAlloy.Web.Social.Blocks;
using EPiServer.SocialAlloy.Web.Social.Common.Controllers;
using EPiServer.SocialAlloy.Web.Social.Common.Exceptions;
using EPiServer.SocialAlloy.Web.Social.Models;
using EPiServer.SocialAlloy.Web.Social.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EPiServer.SocialAlloy.Web.Social.Controllers
{
    /// <summary>
    /// The FeedBlockController handles the rendering of feed items, if any, that were automatically
    /// generated by the Social Activity Streams system in response to activities occuring on any 
    /// target items that the logged in user has subscribed to.
    /// </summary>
    public class FeedBlockController : SocialBlockController<FeedBlock>
    {
        private readonly IUserRepository userRepository;
        private readonly ISocialFeedRepository feedRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        public FeedBlockController()
        {
            this.userRepository = ServiceLocator.Current.GetInstance<IUserRepository>();
            this.feedRepository = ServiceLocator.Current.GetInstance<ISocialFeedRepository>();
        }

        /// <summary>
        /// Render the feed block frontend view.
        /// </summary>
        /// <param name="currentBlock">The current frontend block instance.</param>
        /// <returns>The action's result.</returns>
        public override ActionResult Index(FeedBlock currentBlock)
        {
            var currentBlockLink = ((IContent)currentBlock).ContentLink;

            // Create a feed block view model to fill the frontend block view
            var feedBlockViewModel = new FeedBlockViewModel(currentBlock);

            // If user logged in, retreive activity feed for logged in user
            if (this.User.Identity.IsAuthenticated)
            {
                GetSocialActivityFeed(currentBlock, feedBlockViewModel);
            }

            return PartialView("~/Views/Social/FeedBlock/FeedView.cshtml", feedBlockViewModel);
        }

        private void GetSocialActivityFeed(FeedBlock currentBlock, FeedBlockViewModel feedBlockViewModel)
        {
            feedBlockViewModel.DisplayErrorMessage = String.Empty;

            try
            {
                var userId = userRepository.GetUserId(this.User);
                if (!String.IsNullOrWhiteSpace(userId))
                {
                    feedBlockViewModel.FeedItems =
                        this.feedRepository.Get(new SocialFeedFilter
                        {
                            Subscriber = userId,
                            PageSize = currentBlock.FeedDisplayMax
                        });
                }
                else
                {
                    feedBlockViewModel.DisplayErrorMessage = String.Format("There was an error identifying the logged in user. Please make sure you are logged in and try again.");
                }
            }
            catch (SocialRepositoryException ex)
            {
                feedBlockViewModel.DisplayErrorMessage = ex.Message;
            }
        }
    }
}