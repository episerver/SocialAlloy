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
        /// Converts a Worflow into a WorkflowViewModel
        /// </summary>
        /// <param name="workflow">Workflow to be adapted</param>
        /// <returns>WorkflowViewModel</returns>
        public WorkflowViewModel Adapt(Workflow workflow)
        {
            WorkflowViewModel viewModel = null;

            if (workflow != null)
            {
                viewModel = new WorkflowViewModel
                {
                    Id = workflow.Id.ToString(),
                    Name = workflow.Name
                };
            }

            return viewModel;
        }
    }
}