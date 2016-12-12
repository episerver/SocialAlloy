using EPiServer.SocialAlloy.Web.Social.Adapters;

namespace EPiServer.SocialAlloy.Web.Social.Models
{
    /// <summary>
    /// This interface represents a set of operations supported by all types of social activities 
    /// in the Episerver Social sample.
    /// </summary>
    public interface ISocialActivity
    {
        /// <summary>
        /// Accepts an ISocialActivityAdapter instance capable of interpreting an activity instance.
        /// </summary>
        /// <param name="adapter">an instance of ISocialActivityAdapter</param>
        void Accept(ISocialActivityAdapter adapter);
    }
}