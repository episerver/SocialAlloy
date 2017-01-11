using EPiServer.Social.Moderation.Core;
using EPiServer.SocialAlloy.Web.Social.Models.Moderation;
using System.Linq;

namespace EPiServer.SocialAlloy.Web.Social.Adapters.Moderation
{
    /// <summary>
    /// Adapter used to easily translate SocialAlloy Workflow types with EPi.Social Workflow types
    /// </summary>
    public class WorkflowAdapter
    {
        /// <summary>
        /// Converts a Worflow into a SocialWorkflow
        /// </summary>
        /// <param name="workflow">Workflow to be adapted</param>
        /// <returns>SocialWorkflow</returns>
        public SocialWorkflow Adapt(Workflow workflow)
        {
            SocialWorkflow viewModel = null;

            if (workflow != null)
            {
                viewModel = new SocialWorkflow(workflow.Id.ToString(), workflow.Name, workflow.InitialState.Name);
            }

            return viewModel;
        }
    }
}