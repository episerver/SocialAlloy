namespace EPiServer.SocialAlloy.Web.Social.Models
{
    public abstract class SocialActivity
    {
    }

    public class SocialRatingActivity : SocialActivity
    {
        /// <summary>
        /// The rating value that was submitted by a user rating a page.
        /// </summary>
        public int Value { get; set; }
    }

    public class SocialCommentActivity : SocialActivity
    {
        /// <summary>
        /// The body of the comment that was posted on a page.
        /// </summary>
        public string Body { get; set; }
    }

}