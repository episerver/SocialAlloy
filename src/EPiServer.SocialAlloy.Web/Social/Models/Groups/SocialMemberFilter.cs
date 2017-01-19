using EPiServer.Social.Groups.Core;

namespace EPiServer.SocialAlloy.Web.Social.Models
{
    /// <summary>
    /// The SocialMemberFilter  describes the specifications by which
    /// member data should be retrieved.
    /// </summary>
    public class SocialMemberFilter
    {
        /// <summary>
        /// Gets or sets the group id to filter on.
        /// </summary>
        public string GroupId { get; set; }

        /// <summary>
        /// Gets or sets the number of members to retrieve.
        /// </summary>
        public int PageSize { get; set; }
    }
}