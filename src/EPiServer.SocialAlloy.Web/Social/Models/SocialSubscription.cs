namespace EPiServer.SocialAlloy.Web.Social.Models
{
    /// <summary>
    /// The SocialSubscription class describes a subscription model used by the SocialAlloy site.
    /// </summary>
    public class SocialSubscription
    {
        /// <summary>
        /// The list of possible subscription types.
        /// </summary>
        public static readonly string PageSubscription = "Page";

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

        /// <summary>
        /// Gets or sets the type of subscription.
        /// </summary>
        public string Type { get; set; }
    }
}