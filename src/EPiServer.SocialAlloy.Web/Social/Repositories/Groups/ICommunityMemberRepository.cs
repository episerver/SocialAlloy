using EPiServer.SocialAlloy.Web.Social.Models;
using System.Collections.Generic;

namespace EPiServer.SocialAlloy.Web.Social.Repositories
{
    /// <summary>
    /// The ICommunityMemberRepository interface describes a component capable
    /// of persisting, and retrieving social member data from
    /// an underlying data store.
    /// </summary>
    public interface ICommunityMemberRepository
    {
        /// <summary>
        /// Adds a member to the underlying member repository.
        /// </summary>
        /// <param name="member">The member to add.</param>
        /// <returns>The added member.</returns>
        CommunityMember Add(CommunityMember member);

        /// <summary>
        /// Retrieves a list of members to the underlying member repository.
        /// </summary>
        /// <param name="memberFilter">The filter by which to retrieve members by.</param>
        /// <returns>The list of members that are part of the specified group.</returns>
        IEnumerable<CommunityMember> Get(CommunityMemberFilter memberFilter);
    }
}
