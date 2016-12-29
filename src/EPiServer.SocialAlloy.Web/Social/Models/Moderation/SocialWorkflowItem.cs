using EPiServer.Social.Common;
using EPiServer.Social.Moderation.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPiServer.SocialAlloy.Web.Social.Models.Moderation
{
    /// <summary>
    /// The SocialWorkflowItem defines the workflow item details necessary to add and retrieve workflow items within the SocialAlloy sample
    /// </summary>
    public class SocialWorkflowItem
    {
        public SocialWorkflowItem(WorkflowId workflowId, WorkflowState workflowState, Reference target)
        {
            WorkflowId = workflowId;
            WorkflowState = workflowState;
            Target = target;
        }

        /// <summary>
        /// The id of the workflow that the SocialWorkflowItem is associated with
        /// </summary>
        public WorkflowId WorkflowId { get; set; }

        /// <summary>
        /// The current state that a workflow item is in within the workflow
        /// </summary>
        public WorkflowState WorkflowState { get; set; }

        /// <summary>
        /// A reference to the member and group data for this workflow item
        /// </summary>
        public Reference Target { get; set; }
    }
}