using EPiServer.Social.Common;
using EPiServer.Social.Groups.Core;

namespace EPiServer.SocialAlloy.Web.Social.Models
{
    /// <summary>
    /// The SocialMember describes a relationship
    /// associating a user with a social group.
    /// </summary>
    public class SocialMember
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userReference">Reference which uniquely identifies the user assigned to a group</param>
        /// <param name="groupId">ID of the group to which the user is assigned</param>
        public SocialMember( string userReference, string groupId)
        {
            UserReference = userReference;
            GroupId = groupId;
        }

        /// <summary>
        /// Gets or sets the reference which uniquely identifies the user who
        /// is assigned as a member of the group.
        /// </summary>
        public string UserReference { get; set; }

        /// <summary>
        /// Gets or sets the ID of the group to which the user has been assigned
        /// as a member.
        /// </summary>
        public string GroupId { get; set; }
    }
}