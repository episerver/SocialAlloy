using EPiServer.Social.Groups.Core;
using EPiServer.SocialAlloy.Web.Social.Models;
using EPiServer.SocialAlloy.Web.Social.Models.Groups;

namespace EPiServer.SocialAlloy.Web.Social.Adapters.Groups
{
    /// <summary>
    /// Adapter used to easily translate SocialAlloy Workflow types with EPi.Social Workflow types
    /// </summary>
    public interface ISocialMemberAdapter
    {
        /// <summary>
        /// Adapts a SocialMember with its corresponding extension data.
        /// </summary>
        /// <param name="member">The SocialMember to be adapted</param>
        /// <param name="extension">The MemberExtensionData to be adapted</param>
        /// <returns>The adapted SocialCompositeMember</returns>
        SocialCompositeMember Adapt(SocialMember member, MemberExtensionData extension);

        /// <summary>
        /// Adapts a Member into a SocialMember
        /// </summary>
        /// <param name="member">The member to be adapted</param>
        /// <returns>The adapted SocialMember</returns>
        SocialMember Adapt(Member member);
    }
}
