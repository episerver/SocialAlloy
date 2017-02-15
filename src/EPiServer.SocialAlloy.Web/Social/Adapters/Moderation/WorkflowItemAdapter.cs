using EPiServer.ServiceLocation;
using EPiServer.Social.Common;
using EPiServer.Social.Moderation.Core;
using EPiServer.SocialAlloy.ExtensionData.Membership;
using EPiServer.SocialAlloy.Web.Social.Models.Groups;
using EPiServer.SocialAlloy.Web.Social.Models.Moderation;
using EPiServer.SocialAlloy.Web.Social.Repositories;
using System.Linq;

namespace EPiServer.SocialAlloy.Web.Social.Adapters.Moderation
{
    /// <summary>
    /// Adapter used to easily translate SocialAlloy WorkflowItem types with EPi.Social WorkflowItme types
    /// </summary>
    public class WorkflowItemAdapter
    {
        private Workflow workflow;
        private readonly IUserRepository userRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="workflow">The workflow that will be adapted</param>
        public WorkflowItemAdapter(Workflow workflow, IUserRepository userRepository )
        {
            this.workflow = workflow;
            this.userRepository = userRepository;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public WorkflowItemAdapter(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        /// <summary>
        /// Converts a composite WorkflowItem with extension data of type AddMemberRequestModel into a MembershipRequestModel
        /// </summary>
        /// <param name="item">Composite item to be adapted into a MembershipRequestModel</param>
        /// <returns>MembershipRequestModel</returns>
        public MembershipRequestModel Adapt(Composite<WorkflowItem, AddMemberRequest> item)
        {
            var user = item.Extension.User;
            var userName = userRepository.ParseUserUri(user);

            return new MembershipRequestModel
            {
                User = user,
                Group = item.Extension.Group,
                WorkflowId = item.Data.Workflow.ToString(),
                Created = item.Data.Created.ToLocalTime(),
                State = item.Data.State.Name,
                Actions = workflow.ActionsFor(item.Data.State).Select(a => a.Name),
                UserName = userName
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
                var id = new WorkflowId(socialWorkflowItem.Id);
                var state = new WorkflowState(socialWorkflowItem.State);
                var target = Reference.Create(socialWorkflowItem.Target);

                workflowItem = new WorkflowItem(id, state, target);
            }

            return workflowItem;
        }
    }
}
