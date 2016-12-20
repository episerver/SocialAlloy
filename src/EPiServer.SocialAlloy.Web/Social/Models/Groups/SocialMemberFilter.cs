using EPiServer.Social.Groups.Core;

namespace EPiServer.SocialAlloy.Web.Social.Models
{
    /// <summary>
    /// The SocialMemberFilter class exposes a set of properties by which social member repo can use to filter
    /// </summary>
    public class SocialMemberFilter
    {
        /// <summary>
        /// The group id to filter on.
        /// </summary>
        public GroupId GroupId { get; set; }

        /// <summary>
        /// The number of members to retrieve.
        /// </summary>
        public int PageSize { get; set; }
    }
}