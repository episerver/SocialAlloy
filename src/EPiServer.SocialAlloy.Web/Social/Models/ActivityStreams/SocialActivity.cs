using System;

namespace EPiServer.SocialAlloy.Web.Social.Models
{
    /// <summary>
    /// Represents a base class for all types of social activities in the Episerver Social sample.
    /// </summary>
    public abstract class SocialActivity : ISocialActivity
    {
        /// <summary>
        /// Accepts an ISocialActivityVisitor instance capable of interpreting this activity instance.
        /// </summary>
        /// <param name="visitor">an instance of ISocialActivityVisitor</param>
        public abstract void Accept(ISocialActivityAdapter visitor);
    }

}