namespace EPiServer.SocialAlloy.Web.Social.Models
{
    /// <summary>
    /// The ISocialActivityVisitor interface represents a set of operations used for interpreting
    /// and describing various types of Social activities in the Episerver Social sample.
    /// </summary>
    public interface ISocialActivityVisitor
    {
        /// <summary>
        /// Interprets a SocialActivity
        /// </summary>
        /// <param name="activity">the SocialActivity to interpret</param>
        void Visit(SocialActivity activity);

        /// <summary>
        /// Interprets a SocialCommentActivity
        /// </summary>
        /// <param name="activity">the SocialCommentActivity to interpret</param>
        void Visit(SocialCommentActivity activity);

        /// <summary>
        /// Interprets a SocialRatingActivity
        /// </summary>
        /// <param name="activity">the SocialRatingActivity to interpret</param>
        void Visit(SocialRatingActivity activity);
    }
}