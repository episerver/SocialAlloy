using System;

namespace EPiServer.SocialAlloy.Web.Social.Models
{
    /// <summary>
    /// Represents a base class for all types of social activities in the Episerver Social sample.
    /// </summary>
    public abstract class SocialActivity : ISocialActivity
    {
        /// <summary>
        /// Accepts an ISocialActivityAdapter instance capable of interpreting this activity instance.
        /// </summary>
        /// <param name="adapter">an instance of ISocialActivityAdapter</param>
        public abstract void Accept(ISocialActivityAdapter adapter);
    }

}