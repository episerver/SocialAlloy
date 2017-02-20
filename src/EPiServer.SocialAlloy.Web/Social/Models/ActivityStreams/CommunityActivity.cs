using EPiServer.SocialAlloy.Web.Social.Adapters;
using System;

namespace EPiServer.SocialAlloy.Web.Social.Models
{
    /// <summary>
    /// Represents a base class for all types of community activities in the site.
    /// </summary>
    public abstract class CommunityActivity : ICommunityActivity
    {
        /// <summary>
        /// Accepts an ICommunityActivityAdapter instance capable of interpreting this activity instance.
        /// </summary>
        /// <param name="adapter">an instance of ICommunityActivityAdapter</param>
        public abstract void Accept(ICommunityActivityAdapter adapter);
    }

}