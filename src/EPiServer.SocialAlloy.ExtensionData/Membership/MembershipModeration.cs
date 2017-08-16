using EPiServer.Social.Common;

namespace EPiServer.SocialAlloy.ExtensionData.Membership
{
    /// <summary>
    /// The MembershipModeration class is intended to identify
    /// the relationship between a moderation workflow and
    /// the group which is supports for moderating membership
    /// requests. This serves as an extension data added to workflows
    /// </summary>
    [ExtensionData("12c0f517-091f-4d8c-bace-a9b7116a0a38")]
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
        public string Group { get; set; }
    }
}
