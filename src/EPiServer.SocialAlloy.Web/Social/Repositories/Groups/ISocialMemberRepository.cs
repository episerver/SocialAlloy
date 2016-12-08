using EPiServer.Social.Common;
using EPiServer.Social.Groups.Core;
using EPiServer.SocialAlloy.Web.Social.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPiServer.SocialAlloy.Web.Social.Repositories
{
    public interface ISocialMemberRepository
    {
        /// <summary>
        /// Adds a member to the underlying member repository.
        /// </summary>
        /// <param name="member">The member to add.</param>
        /// <returns>The added member.</returns>
        Composite<Member, MemberExtensionData> Add(Member member, MemberExtensionData memberExtension);

        /// <summary>
        /// Retrieves a list of members to the underlying member repository.
        /// </summary>
        /// <param name="groupId">The groupId of the group the members are a part of.</param>
        /// <returns>The list of members that are part of the specified group.</returns>
        IEnumerable<Composite<Member, MemberExtensionData>> Get(GroupId groupId);
    }
}
