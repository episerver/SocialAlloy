using EPiServer.Social.ActivityStreams.Core;
using EPiServer.Social.Common;
using EPiServer.SocialAlloy.Web.Social.Common.Exceptions;
using EPiServer.SocialAlloy.Web.Social.Models;
using System.Collections.Generic;
using System.Linq;

namespace EPiServer.SocialAlloy.Web.Social.Repositories
{
    /// <summary>
    /// The SocialSubscriptionRepository class defines the operations that can be issued
    /// against the EPiServer Social cloud subscription repository.
    /// </summary>
    public class SocialSubscriptionRepository : ISocialSubscriptionRepository
    {
        private readonly ISubscriptionService subscriptionService;

        /// <summary>
        /// Constructor
        /// </summary>
        public SocialSubscriptionRepository(ISubscriptionService subscriptionService)
        {
            this.subscriptionService = subscriptionService;
        }

        /// <summary>
        /// Adds a subscription to the EPiServer Social subscription repository.
        /// </summary>
        /// <param name="subscription">The subscription to add.</param>
        /// <returns>The added subscription.</returns>
        /// <exception cref="SocialRepositoryException">Thrown if there are any issues sending the request to the 
        /// EPiServer Social cloud subscription repository.</exception>
        public Subscription Add(SocialSubscription subscription)
        {
            var newSubscription = AdaptSubscription(subscription);
            Subscription addedSubscription = null;

            try
            {
                addedSubscription = this.subscriptionService.Add(newSubscription);
            }
            catch (SocialAuthenticationException ex)
            {
                throw new SocialRepositoryException("The application failed to authenticate with EPiServer social.", ex);
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

            return addedSubscription;
        }

        /// <summary>
        /// Gets whether subscriptions exist in the EpiServer Social subscription repository that match a filter.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Whether subscriptions exist.</returns>
        /// <exception cref="SocialRepositoryException">Thrown if there are any issues sending the request to the 
        /// EPiServer Social subscription repository.</exception>
        public bool Exist(SocialSubscriptionFilter filter)
        {
            var subscriptionFilter = AdaptSubscriptionFilter(filter);
            try
            {
                return this.subscriptionService.Get(
                    new Criteria<SubscriptionFilter>
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
        }

        /// <summary>
        /// Removes a subscription from the EPiServer Social subscription repository.
        /// </summary>
        /// <param name="subscription">The subscription to remove.</param>
        /// <exception cref="SocialRepositoryException">Thrown if there are any issues sending the request to the 
        /// EPiServer Social cloud subscription repository.</exception>
        public void Remove(SocialSubscription subscription)
        {
            var removeSubscription = AdaptSubscription(subscription);

            try
            {
                this.subscriptionService.Remove(
                    new Criteria<SubscriptionFilter>
                    {
                        Filter = new SubscriptionFilter
                        {
                            Subscriber = removeSubscription.Subscriber,
                            Target = removeSubscription.Target,
                            Type = removeSubscription.Type
                        }
                    }
                );
            }
            catch (SocialAuthenticationException ex)
            {
                throw new SocialRepositoryException("The application failed to authenticate with EPiServer social.", ex);
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
        }

        /// <summary>
        /// Adapt the application SocialSubscription to the EPiServer Social Subscription 
        /// </summary>
        /// <param name="subscription">The application's SocialSubscription.</param>
        /// <returns>The EPiServer Social Subscription.</returns>
        private Subscription AdaptSubscription(SocialSubscription subscription)
        {
            return new Subscription(Reference.Create(subscription.Subscriber), 
                                    Reference.Create(subscription.Target), 
                                    SubscriptionType.Create(subscription.Type));
        }

        /// <summary>
        /// Adapt a list of EPiServer Social Subscription to application's SocialSubscription.
        /// </summary>
        /// <param name="subscriptions">The list of EPiServer Social Subscription.</param>
        /// <returns>The list of application SocialSubscription.</returns>
        private IEnumerable<SocialSubscription> AdaptSocialSubscription(List<Subscription> subscriptions)
        {
            return subscriptions.Select(c =>
                new SocialSubscription
                {
                    Id = c.Id.ToString(),
                    Subscriber = c.Subscriber.ToString(),
                    Target = c.Target.ToString(),
                    Type = c.Type.Type
                }
            );
        }

        /// <summary>
        /// Adapt a SocialSubscriptionFilter to a SubscriptionFilter
        /// </summary>
        /// <param name="filter">The SocialSubscriptionFilter </param>
        /// <returns>The SubscriptionFilter</returns>
        private SubscriptionFilter AdaptSubscriptionFilter(SocialSubscriptionFilter filter)
        {
            return new SubscriptionFilter
            {
                Subscriber = !string.IsNullOrWhiteSpace(filter.Subscriber) ? Reference.Create(filter.Subscriber) : Reference.Empty,
                Target = !string.IsNullOrWhiteSpace(filter.Target) ? Reference.Create(filter.Target) : Reference.Empty,
                Type = !string.IsNullOrWhiteSpace(filter.Type) ? SubscriptionType.Create(filter.Type) : SubscriptionType.Empty
            };
        }
    }
}