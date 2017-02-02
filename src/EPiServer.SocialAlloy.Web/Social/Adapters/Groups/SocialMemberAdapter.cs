using EPiServer.Social.Groups.Core;
using EPiServer.SocialAlloy.ExtensionData.Membership;
using EPiServer.SocialAlloy.Web.Social.Models;

namespace EPiServer.SocialAlloy.Web.Social.Adapters.Groups
{
    /// <summary>
    /// Adapter used to easily translate SocialAlloy WorkflowItem types with EPi.Social WorkflowItme types
    /// </summary>
    public class SocialMemberAdapter
    {
        /// <summary>
        /// Adapts SocialMember into an AddMemberRequest
        /// </summary>
        /// <param name="member">The SocialMember to be adapted</param>
        /// <returns>AddMemberRequest</returns>
        public AddMemberRequest Adapt(SocialMember member)
        {
            return new AddMemberRequest(member.GroupId, member.UserReference, member.Email, member.Company, member.LoggedInUserId);
        }

        /// <summary>
        /// Adapts an AddMemberRequest into a SocialMember
        /// </summary>
        /// <param name="memberRequest">The AddMemberRequest to be adapted</param>
        /// <returns>SocialMember</returns>
        public SocialMember Adapt(AddMemberRequest memberRequest)
        {
            return new SocialMember(memberRequest.User, memberRequest.Group, memberRequest.Email, memberRequest.Company, memberRequest.LoggedInUserId);
        }

        /// <summary>
        /// Adapts a Member into a SocialMember
        /// </summary>
        /// <param name="member">The member to be adapted </param>
        /// <returns>SocialMember</returns>
        public SocialMember Adapt(Member member, MemberExtensionData extension)
        {
            return new SocialMember(member.User.Id, member.Group.Id, extension.Email, extension.Company, extension.LoggedInUserId);
        }
    }
}