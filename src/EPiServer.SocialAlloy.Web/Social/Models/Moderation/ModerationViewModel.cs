using EPiServer.SocialAlloy.Web.Social.Models.Groups;
using System.Collections.Generic;

namespace EPiServer.SocialAlloy.Web.Social.Models.Moderation
{
    /// <summary>
    /// The ModerationViewModel describes the information necessary
    /// to present the state of membership requests under moderation
    /// within this application.
    /// </summary>
    public class ModerationViewModel
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ModerationViewModel()
        {
            this.Workflows = new List<WorkflowViewModel>();
            this.Items = new List<MembershipRequestModel>();
        }

        /// <summary>
        /// Gets or sets a collection of workflows available within
        /// the application.
        /// </summary>
        public IEnumerable<WorkflowViewModel> Workflows { get; set; }

        /// <summary>
        /// Gets or sets the workflow selected as the "current" workflow.
        /// </summary>
        public WorkflowViewModel SelectedWorkflow { get; set; }

        /// <summary>
        /// Gets or sets a collection of items, associated with the
        /// selected workflow, which are under moderation.
        /// </summary>
        public IEnumerable<MembershipRequestModel> Items { get; set; }
    }
}