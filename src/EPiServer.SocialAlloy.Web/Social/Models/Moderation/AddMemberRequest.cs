using EPiServer.ServiceLocation;
using EPiServer.Social.Common;
using EPiServer.Social.Groups.Core;
using EPiServer.SocialAlloy.Web.Social.Repositories;

namespace EPiServer.SocialAlloy.Web.Social.Models.Moderation
{
    /// <summary>
    /// The AddMemberRequest is a serializable class representing 
    /// a request for membership to a group. It is intended to
    /// support the moderation process around group membership.
    /// </summary>
    public class AddMemberRequest : IApprovable
    {
        private readonly ISocialMemberRepository memberRepository;
        /// <summary>
        /// Constructor
        /// </summary>
        public AddMemberRequest()
        {
            memberRepository = ServiceLocator.Current.GetInstance<ISocialMemberRepository>();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="group">Group to which a member should be added</param>
        /// <param name="user">User which is pending approval for membership within the group</param>
        public AddMemberRequest(SocialMember member, MemberExtensionData extensionData)
        {
            this.Member = member;
            this.ExtensionData = extensionData;
        }

        /// <summary>
        /// Membership data needed for members to be added to a group
        /// </summary>
        public SocialMember Member { get; set; }

        /// <summary>
        /// Membership extension data needed for members to be added to a group
        /// </summary>
        public MemberExtensionData ExtensionData { get; set; }


        /// <summary>
        /// Approves the membership request represented by this instance,
        /// commiting the user as a member of the target group.
        /// </summary>
        public void Approve()
        {
            memberRepository.Add(this.Member, this.ExtensionData);
        }

        /// <summary>
        /// Returns a reference identifying this membership request.
        /// </summary>
        /// <returns>Reference identifying this membership request</returns>
        public Reference ToReference()
        {
            return Reference.Create(
                string.Format(
                    "members://{0}/{1}",
                    this.Member.GroupId.ToString(),
                    this.Member.UserReference.ToString()
                )
            );
        }
    }
}