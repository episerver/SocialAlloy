namespace EPiServer.SocialAlloy.Web.Social.Models
{
    /// <summary>
    /// The PageCommentFilter class exposes a set of properties by which
    /// PageComments may be filtered.
    /// </summary>
    public class PageCommentFilter
    {
        /// <summary>
        /// The comment target to filter on.
        /// </summary>
        public string Target { get; set; }

        /// <summary>
        /// The number of comments to retrieve.
        /// </summary>
        public int PageSize { get; set; }
    }
}