using System.Collections.Generic;

namespace EPiServer.SocialAlloy.Web.Social.Models.Moderation
{
    /// <summary>
    /// The SocialWorkflow defines the workflow details necessary to add and retrieve workflows within the SocialAlloy sample
    /// </summary>
    public class SocialWorkflow
    {
        public SocialWorkflow(string workflowName, List<SocialWorkflowTransition> workflowTransitions, SocialWorkflowState workflowState )
        {
            this.workflowName = workflowName;
            this.workflowTransitions = workflowTransitions;
            this.workflowState = workflowState;
        }
        /// <summary>
        /// The name of the workflow
        /// </summary>
        public string workflowName { get; set; }

        /// <summary>
        /// The transitions that a social workflow can go through
        /// </summary>
        public List<SocialWorkflowTransition> workflowTransitions { get; set; }

        /// <summary>
        /// The initial state of a workflow item that enters this social workflow
        /// </summary>
        public SocialWorkflowState workflowState { get; set; }

    }
}