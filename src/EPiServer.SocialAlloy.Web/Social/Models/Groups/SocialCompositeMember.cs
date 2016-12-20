using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPiServer.SocialAlloy.Web.Social.Models.Groups
{
    public class SocialCompositeMember
    {
        public SocialMember Member { get; set; }
        public MemberExtensionData MemberExtension { get; set; }
    }
}