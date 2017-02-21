namespace EPiServer.SocialAlloy.Web.Social.Models
{
    /// <summary>
    /// The CommunityMemberFilter  describes the specifications by which
    /// member data should be retrieved.
    /// </summary>
    public class CommunityMemberFilter
    {
        /// <summary>
        /// Gets or sets the community id to filter on.
        /// </summary>
        public string CommunityId { get; set; }

        /// <summary>
        /// Gets or sets the number of members to retrieve.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets or sets the the Episerver users id to filter on.
        /// </summary>
        public string UserId { get; set; }
    }
}