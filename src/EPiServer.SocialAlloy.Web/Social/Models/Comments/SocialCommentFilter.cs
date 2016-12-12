namespace EPiServer.SocialAlloy.Web.Social.Models
{
    /// <summary>
    /// The SocialCommentFilter class exposes a set of properties by which
    /// social comments may be filtered.
    /// </summary>
    public class SocialCommentFilter
    {
        /// <summary>
        /// Comment visibility filter values
        /// </summary>
        public enum VisibilityFilter
        {
            All,
            Visible,
            NotVisible
        }

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

        /// <summary>
        /// Whether to filter only on comments that are visible.
        /// </summary>
        public VisibilityFilter Visibility { get; set; }
    }
}