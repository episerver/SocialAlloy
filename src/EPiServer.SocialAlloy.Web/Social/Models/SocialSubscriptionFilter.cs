namespace EPiServer.SocialAlloy.Web.Social.Models
{
    /// <summary>
    /// The SocialSubscriptionFilter class exposes a set of properties by which
    /// social subscriptions may be filtered.
    /// </summary>
    public class SocialSubscriptionFilter
    {
        /// <summary>
        /// constructor
        /// </summary>
        public SocialSubscriptionFilter()
        {
            PageSize = 10;
            PageOffset = 0;
        }

        /// <summary>
        /// Gets or sets the subscriber.
        /// </summary>
        public string Subscriber { get; set; }

        /// <summary>
        /// Gets or sets the target to subscribe or subscribed to.
        /// </summary>
        public string Target { get; set; }

        /// <summary>
        /// Gets or sets the type of subscription.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// The number of subscriptions to retrieve.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// The offset to start retrieving the next page of subscriptions from.
        /// </summary>
        public int PageOffset { get; set; }
    }
}