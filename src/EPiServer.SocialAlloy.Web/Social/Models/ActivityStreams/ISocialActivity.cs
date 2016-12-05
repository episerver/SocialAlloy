namespace EPiServer.SocialAlloy.Web.Social.Models
{
    /// <summary>
    /// This interface represents a set of operations supported by all types of social activities 
    /// in the Episerver Social sample.
    /// </summary>
    public interface ISocialActivity
    {
        /// <summary>
        /// Accepts an ISocialActivityVisitor instance capable of interpreting an activity instance.
        /// </summary>
        /// <param name="visitor">an instance of ISocialActivityVisitor</param>
        void Accept(ISocialActivityVisitor visitor);
    }
}