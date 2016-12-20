using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EPiServer.Social.Groups.Core;
using EPiServer.SocialAlloy.Web.Social.Models;
using EPiServer.SocialAlloy.Web.Social.Models.Groups;

namespace EPiServer.SocialAlloy.Web.Social.Adapters.Groups
{
    public class SocialMemberAdapter : ISocialMemberAdapter
    {
        public SocialCompositeMember Adapt(SocialMember member, MemberExtensionData extension)
        {
            return new SocialCompositeMember { Member = member, MemberExtension = extension };
        }

        public SocialMember Adapt(Member member)
        {
            return new SocialMember(member.User, member.Group);
        }
    }
}