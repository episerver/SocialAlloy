namespace EPiServer.SocialAlloy.Web.Social.Models
{
    /// <summary>
    /// The SocialCommentFilter class exposes a set of properties by which
    /// social comments may be filtered.
    /// </summary>
    public class SocialCommentFilter
    {
        public string Author { get; set; }
        public string Target { get; set; }
        public int PageSize { get; set; }
        public int PageOffset { get; set; }
        public bool Visible { get; set; }
    }
}