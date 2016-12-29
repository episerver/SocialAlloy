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

        /// <summary>
        /// Converts a SocialWorkflow into a Workflow
        /// </summary>
        /// <param name="socialWorkflow">The SocialWorkflow to be adapted</param>
        /// <returns>Workflow</returns>
        public Workflow Adapt(SocialWorkflow socialWorkflow)
        {
            Workflow workflow = null;

            if (socialWorkflow != null)
            {
                var workflowName = socialWorkflow.workflowName;
                var workflowTransitions = socialWorkflow.workflowTransitions.Select(x => Adapt(x)).ToList();
                var workflowState = new WorkflowState(socialWorkflow.workflowState.Name);
                workflow = new Workflow(workflowName, workflowTransitions, workflowState);
            }

            return workflow;
        }

        /// <summary>
        /// Converts a SocialWorkflowTransition into a WorkflowTransition
        /// </summary>
        /// <param name="socialWorkflowTransition">The socialWorkflowTransition to be adapted</param>
        /// <returns>WorkflowTransition</returns>
        public WorkflowTransition Adapt(SocialWorkflowTransition socialWorkflowTransition)
        {
            WorkflowTransition workflowTransition = null;

            if (socialWorkflowTransition != null)
            {
                var from = new WorkflowState(socialWorkflowTransition.InitialState.Name);
                var to = new WorkflowState(socialWorkflowTransition.SecondaryState.Name);
                var action = new WorkflowAction(socialWorkflowTransition.Action.Name);
                workflowTransition = new WorkflowTransition(from, to, action);
            }

            return workflowTransition;
        }
    }
}