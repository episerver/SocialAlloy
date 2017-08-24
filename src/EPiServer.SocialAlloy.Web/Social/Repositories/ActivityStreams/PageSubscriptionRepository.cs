using EPiServer.Social.ActivityStreams.Core;
using EPiServer.Social.Common;
using EPiServer.SocialAlloy.Web.Social.Common.Exceptions;
using EPiServer.SocialAlloy.Web.Social.Models;
using System.Collections.Generic;
using System.Linq;

namespace EPiServer.SocialAlloy.Web.Social.Repositories
{
    /// <summary>
    /// The PageSubscriptionRepository class defines the operations that can be issued
    /// against the Episerver Social cloud subscription repository.
    /// </summary>
    public class PageSubscriptionRepository : IPageSubscriptionRepository
    {
        private readonly ISubscriptionService subscriptionService;
        private readonly SubscriptionFilters subscriptionFilters;

        /// <summary>
        /// Constructor
        /// </summary>
        public PageSubscriptionRepository(ISubscriptionService subscriptionService)
        {
            this.subscriptionService = subscriptionService;
            this.subscriptionFilters = new SubscriptionFilters();
        }

        /// <summary>
        /// Adds a subscription to the Episerver Social Framework.
        /// </summary>
        /// <param name="subscription">The subscription to add.</param>
        /// <exception cref="SocialRepositoryException">Thrown if there are any issues sending the request to the 
        /// Episerver Social Framework.</exception>
        public void Add(PageActivitySubscription subscription)
        {
            try
            {
                var newSubscription = AdaptSubscription(subscription);
                this.subscriptionService.Add(newSubscription);
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

        /// <summary>
        /// Gets whether subscriptions exist in the Episerver Social subscription repository that match a filter.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Whether subscriptions exist.</returns>
        /// <exception cref="SocialRepositoryException">Thrown if there are any issues sending the request to the 
        /// Episerver Social subscription repository.</exception>
        public bool Exist(PageSubscriptionFilter filter)
        {
            try
            {
                var subscriptionFilter = AdaptSubscriptionFilter(filter);
                return this.subscriptionService.Get(
                    new Criteria
                    {
                        PageInfo = new PageInfo
                        {
                            PageSize = 0
                        },
                        Filter = subscriptionFilter
                    }
                ).HasMore;
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

        /// <summary>
        /// Removes a subscription from the Episerver Social subscription repository.
        /// </summary>
        /// <param name="subscription">The subscription to remove.</param>
        /// <exception cref="SocialRepositoryException">Thrown if there are any issues sending the request to the 
        /// Episerver Social cloud subscription repository.</exception>
        public void Remove(PageActivitySubscription subscription)
        {
            try
            {
                var removeSubscription = AdaptSubscription(subscription);
                var subscriberFilter = subscriptionFilters.Subscriber.EqualTo(removeSubscription.Subscriber);
                var targetFilter = subscriptionFilters.Target.EqualTo(removeSubscription.Target);
                var typeFilter = subscriptionFilters.Type.EqualTo(removeSubscription.Type.Type);
                var removeFilter = new AndExpression(subscriberFilter, targetFilter, typeFilter);

                this.subscriptionService.Remove(removeFilter);
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

        /// <summary>
        /// Adapt the application PageActivitySubscription to the Episerver Social Subscription 
        /// </summary>
        /// <param name="subscription">The application's PageActivitySubscription.</param>
        /// <returns>The Episerver Social Subscription.</returns>
        private Subscription AdaptSubscription(PageActivitySubscription subscription)
        {
            return new Subscription(Reference.Create(subscription.Subscriber),
                                    Reference.Create(subscription.Target),
                                    SubscriptionType.Create(typeof(PageActivity).Name));
        }

        /// <summary>
        /// Adapt a list of Episerver Social Subscription to application's PageActivitySubscription.
        /// </summary>
        /// <param name="subscriptions">The list of Episerver Social Subscription.</param>
        /// <returns>The list of application PageActivitySubscription.</returns>
        private IEnumerable<PageActivitySubscription> AdaptSocialSubscription(List<Subscription> subscriptions)
        {
            return subscriptions.Select(c =>
                new PageActivitySubscription
                {
                    Id = c.Id.ToString(),
                    Subscriber = c.Subscriber.ToString(),
                    Target = c.Target.ToString(),
                }
            );
        }

        /// <summary>
        /// Adapt a PageSubscriptionFilter to a FilterExpression
        /// </summary>
        /// <param name="filter">The PageSubscriptionFilter </param>
        /// <returns>The FilterExpression</returns>
        private FilterExpression AdaptSubscriptionFilter(PageSubscriptionFilter filter)
        {
            var filters = new List<FilterExpression>();

            if (!string.IsNullOrWhiteSpace(filter.Subscriber))
            {
                filters.Add(this.subscriptionFilters.Subscriber.EqualTo(filter.Subscriber));
            }

            if (!string.IsNullOrWhiteSpace(filter.Target))
            {
                filters.Add(this.subscriptionFilters.Target.EqualTo(filter.Target));
            }

            return (filters.Count > 1) ? new AndExpression(filters) : filters.FirstOrDefault();
        }
    }
}