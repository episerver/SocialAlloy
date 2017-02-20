namespace EPiServer.SocialAlloy.Web.Social.Models
{
    /// <summary>
    /// The PageCommentFilter class exposes a set of properties by which
    /// PageComments may be filtered.
    /// </summary>
    public class PageCommentFilter
    {
        /// <summary>
        /// The comment author to filter on.
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// The comment target to filter on.
        /// </summary>
        public string Target { get; set; }

        /// <summary>
        /// The number of comments to retrieve.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// The offset to start retrieving the next page of comments from.
        /// </summary>
        public int PageOffset { get; set; }
    }
}