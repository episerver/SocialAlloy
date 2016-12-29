using EPiServer.Social.Groups.Core;

namespace EPiServer.SocialAlloy.Web.Social.Models.Moderation
{
    /// <summary>
    /// The MembershipModeration class is intended to identify
    /// the relationship between a moderation workflow and
    /// the group which is supports for moderating membership
    /// requests.
    /// </summary>
    public class MembershipModeration
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MembershipModeration() { }

        /// <summary>
        /// Gets or sets the ID of the group which a moderation
        /// workflow is intended to support.
        /// </summary>
        public GroupId Group { get; set; }
    }
}
