﻿using EPiServer.Social.ActivityStreams.Core;
using EPiServer.SocialAlloy.Web.Social.Models;
using System.Collections.Generic;

namespace EPiServer.SocialAlloy.Web.Social.Repositories
{
    /// <summary>
    /// The ISubscriptionRepository interface defines the operations that can be issued
    /// against a social subscription repository.
    /// </summary>
    public interface ISocialSubscriptionRepository
    {
        /// <summary>
        /// Adds a subscription to the social subscription repository.
        /// </summary>
        /// <param name="subscription">The subscription to add.</param>
        /// <returns>The added subscription.</returns>
        /// <exception cref="SocialRepositoryException">Thrown if there are any issues sending the request to the 
        /// social subscription repository.</exception>
        Subscription Add(SocialSubscription subscription);

        /// <summary>
        /// Gets subscriptions from the social subscription repository based on a filter.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>A list of subscriptions.</returns>
        /// <exception cref="SocialRepositoryException">Thrown if there are any issues sending the request to the 
        /// social subscription repository.</exception>
        IEnumerable<SocialSubscription> Get(SocialSubscriptionFilter filter);

        /// <summary>
        /// Removes a subscription from the social subscription repository.
        /// </summary>
        /// <param name="subscription">The subscription to remove.</param>
        /// <exception cref="SocialRepositoryException">Thrown if there are any issues sending the request to the 
        /// social subscription repository.</exception>
        void Remove(SocialSubscription subscription);
    }
}