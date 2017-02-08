using EPiServer.Social.Groups.Core;

namespace EPiServer.SocialAlloy.ExtensionData.Membership
{
    /// <summary>
    /// The MembershipModeration class is intended to identify
    /// the relationship between a moderation workflow and
    /// the group which is supports for moderating membership
    /// requests. This serves as an extension data added to workflows
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
