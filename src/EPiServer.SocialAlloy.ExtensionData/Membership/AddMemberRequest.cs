using EPiServer.Social.Common;

namespace EPiServer.SocialAlloy.ExtensionData.Membership
{
    /// <summary>
    /// The AddMemberRequest is a serializable class representing 
    /// a request for membership to a group. It is intended to
    /// support the moderation process around group membership.
    /// </summary>
    [ExtensionData("f38f2d16-75b4-4e7b-a580-c0c03cd8a596")]
    public class AddMemberRequest : MemberExtensionData
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AddMemberRequest()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="group">The group to which a member will be added</param>
        /// <param name="user">User which is pending approval for membership within the group</param>
        /// <param name="email">The email address of the member</param>
        /// <param name="company">The company that the member is associated with</param>
        public AddMemberRequest(string group, string user, string email, string company)
        {
            Group = group;
            User = user;
            Company = company;
            Email = email;
        }

        /// <summary>
        /// The group to which a member will be added
        /// </summary>
        public string Group { get; set; }

        /// <summary>
        /// User which is pending approval for membership within the group
        /// </summary>
        public string User { get; set; }
    }
}
