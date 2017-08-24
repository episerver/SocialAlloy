namespace EPiServer.SocialAlloy.Web.Social.Models
{
    /// <summary>
    /// The PageActivitySubscription class describes a page subscription model used by the SocialAlloy site.
    /// </summary>
    public class PageActivitySubscription
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