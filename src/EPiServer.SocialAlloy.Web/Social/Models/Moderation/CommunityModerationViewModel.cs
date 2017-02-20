using EPiServer.SocialAlloy.Web.Social.Models.Groups;
using System.Collections.Generic;

namespace EPiServer.SocialAlloy.Web.Social.Models.Moderation
{
    /// <summary>
    /// The CommunityModerationViewModel describes the information necessary
    /// to present the state of membership requests under moderation
    /// within this application.
    /// </summary>
    public class CommunityModerationViewModel
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public CommunityModerationViewModel()
        {
            this.Workflows = new List<CommunityMembershipWorkflow>();
            this.Items = new List<CommunityMembershipRequest>();
        }

        /// <summary>
        /// Gets or sets a collection of CommunityMembership workflows available within
        /// the application.
        /// </summary>
        public IEnumerable<CommunityMembershipWorkflow> Workflows { get; set; }

        /// <summary>
        /// Gets or sets the workflow selected as the "current" workflow.
        /// </summary>
        public CommunityMembershipWorkflow SelectedWorkflow { get; set; }

        /// <summary>
        /// Gets or sets a collection of items, associated with the
        /// selected workflow, which are under moderation.
        /// </summary>
        public IEnumerable<CommunityMembershipRequest> Items { get; set; }
    }
}