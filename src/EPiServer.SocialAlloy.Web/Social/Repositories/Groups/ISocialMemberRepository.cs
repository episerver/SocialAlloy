using EPiServer.Social.Common;
using EPiServer.Social.Groups.Core;
using EPiServer.SocialAlloy.Web.Social.Models;
using EPiServer.SocialAlloy.Web.Social.Models.Groups;
using System.Collections.Generic;

namespace EPiServer.SocialAlloy.Web.Social.Repositories
{
    public interface ISocialMemberRepository
    {
        /// <summary>
        /// Adds a member to the underlying member repository.
        /// </summary>
        /// <param name="member">The member to add.</param>
        /// <param name="memberExtension">The member extension data to add.</param>
        /// <returns>The added member.</returns>
        SocialCompositeMember Add(SocialMember member, MemberExtensionData memberExtension);

        /// <summary>
        /// Retrieves a list of members to the underlying member repository.
        /// </summary>
        /// <param name="memberFilter">The member filter used to retrieve the correct list of members</param>
        /// <returns>The list of members that are part of the specified group.</returns>
        IEnumerable<SocialCompositeMember> Get(SocialMemberFilter memberFilter);
    }
}
