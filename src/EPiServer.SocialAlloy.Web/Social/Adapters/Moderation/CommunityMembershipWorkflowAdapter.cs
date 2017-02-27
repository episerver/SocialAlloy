using EPiServer.Social.Moderation.Core;
using EPiServer.SocialAlloy.Web.Social.Models.Moderation;

namespace EPiServer.SocialAlloy.Web.Social.Adapters.Moderation
{
    /// <summary>
    /// Adapts data into a CommunityMembershipWorkflow
    /// </summary>
    public class CommunityMembershipWorkflowAdapter
    {
        /// <summary>
        /// Converts a Worflow into a CommunityMembershipWorkflow
        /// </summary>
        /// <param name="workflow">Workflow to be adapted</param>
        /// <returns>CommunityMembershipWorkflow</returns>
        public CommunityMembershipWorkflow Adapt(Workflow workflow)
        {
            CommunityMembershipWorkflow viewModel = null;

            if (workflow != null)
            {
                viewModel = new CommunityMembershipWorkflow(workflow.Id.ToString(), workflow.Name, workflow.InitialState.Name);
            }

            return viewModel;
        }
    }
}