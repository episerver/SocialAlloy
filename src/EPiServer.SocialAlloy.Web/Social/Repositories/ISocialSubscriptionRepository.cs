using EPiServer.Social.ActivityStreams.Core;
using EPiServer.SocialAlloy.Web.Social.Models;
using System.Collections.Generic;

namespace EPiServer.SocialAlloy.Web.Social.Repositories
{
    /// <summary>
    /// The ISubscriptionRepository interface defines the operations that can be issued
    /// against a subscription repository.
    /// </summary>
    public interface ISocialSubscriptionRepository
    {
        /// <summary>
        /// Adds a subscription to the underlying subscription repository.
        /// </summary>
        /// <param name="subscription">The subscription to add.</param>
        /// <returns>The added subscription.</returns>
        Subscription Add(SocialSubscription subscription);

        /// <summary>
        /// Gets subscriptions from the underlying subscription repository based on a filter.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>A list of subscriptions.</returns>
        IEnumerable<SocialSubscription> Get(SocialSubscriptionFilter filter);
    }
}