namespace EPiServer.SocialAlloy.Web.Social.Models
{
    /// <summary>
    /// The SocialSubscriptionFilter class exposes a set of properties by which
    /// social subscriptions may be filtered.
    /// </summary>
    public class SocialSubscriptionFilter
    {
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