using System;

namespace EPiServer.SocialAlloy.Web.Social.Models
{
    public abstract class SocialActivity : ISocialActivity
    {
        public abstract void Accept(ISocialActivityVisitor visitor);
    }

}