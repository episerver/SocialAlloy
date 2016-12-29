using EPiServer.Social.Common;
using EPiServer.Social.Moderation.Core;
using EPiServer.SocialAlloy.Web.Social.Models.Groups;
using EPiServer.SocialAlloy.Web.Social.Models.Moderation;
using System.Linq;

namespace EPiServer.SocialAlloy.Web.Social.Adapters.Moderation
{
    /// <summary>
    /// Adapter used to easily translate SocialAlloy WorkflowItem types with EPi.Social WorkflowItme types
    /// </summary>
    public class WorkflowItemAdapter
    {
        private Workflow workflow;

        public WorkflowItemAdapter(Workflow workflow)
        {
            this.workflow = workflow;
        }

        public WorkflowItemAdapter()
        {

        }

        /// <summary>
        /// Converts a composite WorkflowItem with extension data of type AddMemberRequestModel into a MembershipRequestModel
        /// </summary>
        /// <param name="item">Composite item to be adapted into a MembershipRequestModel</param>
        /// <returns>MembershipRequestModel</returns>
        public MembershipRequestModel Adapt(Composite<WorkflowItem, AddMemberRequest> item)
        {
            return new MembershipRequestModel
            {
                User = item.Extension.Member.UserReference.ToString(),
                Group = item.Extension.Member.GroupId.ToString(),
                WorkflowId = item.Data.Workflow.ToString(),
                Created = item.Data.Created,
                State = item.Data.State.Name,
                Actions = workflow.ActionsFor(item.Data.State).Select(a => a.Name)
            };
        }

        /// <summary>
        /// Converts a SocialWorkflowItem into a WorkflowItem
        /// </summary>
        /// <param name="socialWorkflowItem">The SocialWorkflowItem to be adapted</param>
        /// <returns>WorkflowItem</returns>
        public WorkflowItem Adapt(SocialWorkflowItem socialWorkflowItem)
        {
            WorkflowItem workflowItem = null;

            if (socialWorkflowItem != null)
            {
                workflowItem = new WorkflowItem(socialWorkflowItem.WorkflowId, socialWorkflowItem.WorkflowState, socialWorkflowItem.Target);
            }

            return workflowItem;
        }

        /// <summary>
        /// Converts a WorkflowItem into a SocialWorkflowItem
        /// </summary>
        /// <param name="workflowItem">The WorkflowItem to be adapted</param>
        /// <returns>SocialWorkflowItem</returns>
        public SocialWorkflowItem Adapt(WorkflowItem workflowItem)
        {
            SocialWorkflowItem socialWorkflowItem = null;

            if (workflowItem != null)
            {
                socialWorkflowItem = new SocialWorkflowItem(workflowItem.Workflow, workflowItem.State, workflowItem.Target);
            }

            return socialWorkflowItem;
        }
    }
}
