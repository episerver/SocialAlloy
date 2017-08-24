namespace EPiServer.SocialAlloy.Web.Social.Models
{
    /// <summary>
    /// The CommunityMember describes a relationship
    /// associating a user with a Community.
    /// </summary>
    public class CommunityMember
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="user">Reference which uniquely identifies the member assigned to a group</param>
        /// <param name="groupId">ID of the group to which the member is assigned</param>
        /// <param name="email">The email of the member</param>
        /// <param name="company">The company that a member is associated with</param>
        public CommunityMember(string user, string groupId, string email, string company)
        {
            User = user;
            GroupId = groupId;
            Email = email;
            Company = company;
        }

        /// <summary>
        /// Gets or sets the reference which uniquely identifies the user who
        /// is assigned as a member of the group.
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// Gets or sets the ID of the group to which the user has been assigned
        /// as a member.
        /// </summary>
        public string GroupId { get; set; }

        /// <summary>
        /// Gets or sets the email of the member 
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the company the member works for.
        /// </summary>
        public string Company { get; set; }
    }
}