namespace EPiServer.SocialAlloy.Web.Social.Models
{
    public interface ISocialActivityVisitor
    {
        void Visit(SocialActivity activity);

        void Visit(SocialCommentActivity activity);

        void Visit(SocialRatingActivity activity);
    }
}