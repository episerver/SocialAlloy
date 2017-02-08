namespace EPiServer.SocialAlloy.ExtensionData.Membership
{
    /// <summary>
    /// The AddMemberRequest is a serializable class representing 
    /// a request for membership to a group. It is intended to
    /// support the moderation process around group membership.
    /// </summary>
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
        /// 
        public AddMemberRequest(string group, string user, string email, string company, string loggedInUserId)
        {
            Group = group;
            User = user;
            Company = company;
            Email = email;
            LoggedInUserId = loggedInUserId;
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
