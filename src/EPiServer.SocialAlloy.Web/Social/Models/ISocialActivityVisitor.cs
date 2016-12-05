namespace EPiServer.SocialAlloy.Web.Social.Models
{
    public interface ISocialActivityVisitor
    {
        void Visit(SocialCommentActivity activity);

        void Visit(SocialRatingActivity activity);
    }
}