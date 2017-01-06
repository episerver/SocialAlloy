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
        /// <summary>
        /// Adapts a SocialMember with its corresponding extension data.
        /// </summary>
        /// <param name="member">The SocialMember to be adapted</param>
        /// <param name="extension">The MemberExtensionData to be adapted</param>
        /// <returns>The adapted SocialCompositeMember</returns>
        public SocialCompositeMember Adapt(SocialMember member, MemberExtensionData extension)
        {
            return new SocialCompositeMember { Member = member, MemberExtension = extension };
        }

        /// <summary>
        /// Adapts a Member into a SocialMember
        /// </summary>
        /// <param name="member">The member to be adapted </param>
        /// <returns>The adapted SocialMember</returns>
        public SocialMember Adapt(Member member)
        {
            return new SocialMember(member.User, member.Group);
        }
    }
}