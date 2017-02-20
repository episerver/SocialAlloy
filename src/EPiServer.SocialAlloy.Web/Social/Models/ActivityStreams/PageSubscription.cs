namespace EPiServer.SocialAlloy.Web.Social.Models
{
    /// <summary>
    /// The PageSubscription class describes a subscription model used by the SocialAlloy site.
    /// </summary>
    public class PageSubscription
    {
        /// <summary>
        /// Gets or sets the subscription Id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the subscriber.
        /// </summary>
        public string Subscriber { get; set; }

        /// <summary>
        /// Gets or sets the target to subscribe or subscribed to.
        /// </summary>
        public string Target { get; set; }
    }
}