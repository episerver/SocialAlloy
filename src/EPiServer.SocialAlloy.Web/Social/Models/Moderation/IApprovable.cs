namespace EPiServer.SocialAlloy.Web.Social.Models.Moderation
{
    /// <summary>
    /// The IApprovable interface describes a request under
    /// moderation, which can be approved.
    /// </summary>
    public interface IApprovable
    {
        /// <summary>
        /// Confirms approval of the request represented
        /// by this item.
        /// </summary>
        void Approve();
    }
}