namespace EPiServer.SocialAlloy.Web.Social.Models
{
    public interface ISocialActivity
    {
        void Accept(ISocialActivityVisitor visitor);
    }
}