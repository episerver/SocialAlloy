using EPiServer.Social.Common;
using EPiServer.Social.Groups.Core;
using EPiServer.Social.Moderation.Core;
using EPiServer.SocialAlloy.Web.Social.Adapters.Moderation;
using EPiServer.SocialAlloy.Web.Social.Models;
using EPiServer.SocialAlloy.Web.Social.Models.Groups;
using EPiServer.SocialAlloy.Web.Social.Models.Moderation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPiServer.SocialAlloy.Web.Social.Repositories.Moderation
{
    /// <summary>
    /// Defines the operations that can be issued to use moderation within the SocialAlloy sample
    /// </summary>
    public class SocialModerationRepository : ISocialModerationRepository
    {
        private readonly IWorkflowService workflowService;
        private readonly IWorkflowItemService workflowItemService;
        private readonly IMemberService memberservice;
        private WorkflowItemAdapter workflowItemAdapter;
        private WorkflowAdapter workflowAdapter;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="workflowService">Moderation workflow service supporting this application</param>
        /// <param name="workflowItemService">Moderation workflow item service supporting this application</param>
        /// <param name="memberService">Member service supporting this application</param>
        public SocialModerationRepository(IWorkflowService workflowService, IWorkflowItemService workflowItemService, IMemberService memberservice)
        {
            this.workflowService = workflowService;
            this.workflowItemService = workflowItemService;
            this.memberservice = memberservice;
            this.workflowItemAdapter = new WorkflowItemAdapter();
            this.workflowAdapter = new WorkflowAdapter();
        }

        /// <summary>
        /// Adds a workflow to the underlying repository
        /// </summary>
        /// <param name="socialWorkflow">The workflow to be added</param>
        /// <param name="membershipModeration">Extenstion data used to reflect relationship between group and workflow to be added</param>
        public void Add(SocialWorkflow socialWorkflow, MembershipModeration membershipModeration)
        {
            var workflow = this.workflowAdapter.Adapt(socialWorkflow);
            if (workflow != null)
            {
                this.workflowService.Add(workflow, membershipModeration);
            }
        }

        /// <summary>
        /// Adds a workflowitem to the underlying repository
        /// </summary>
        /// <param name="socialWorkflowItem">The workflowitem to be added</param>
        /// <param name="membershipRequest">Extension data containing member details that will be needed if member admission is approved</param>
        public void Add(SocialWorkflowItem socialWorkflowItem, AddMemberRequest membershipRequest)
        {
            var workflowItem = this.workflowItemAdapter.Adapt(socialWorkflowItem);

            if (workflowItem != null)
            {
                this.workflowItemService.Add(workflowItem, membershipRequest);
            }
        }

        /// <summary>
        /// Retrieves a specific workflowitem and extension data from the underlying repository
        /// </summary>
        /// <param name="user">The user under moderation</param>
        /// <param name="group">The group that membership is being moderated</param>
        /// <returns></returns>
        public Composite<SocialWorkflowItem, AddMemberRequest> Get(string user, string group)
        {
            Composite<SocialWorkflowItem, AddMemberRequest> compositeSocialWorkflowItem = null;

            //Construct a filter to return the desired target under moderation
            var filter = new CompositeCriteria<WorkflowItemFilter, AddMemberRequest>();
            filter.Filter.Target = Reference.Create(string.Format("members://{0}/{1}", group, user));

            //retrive the first workflow that matches the target filter
            var workflowItem = this.workflowItemService.Get(filter).Results.FirstOrDefault();

            //return the composite SocialWorkflowItem and AddMemberRequest
            var socialWorkflowItem = this.workflowItemAdapter.Adapt(workflowItem.Data);
            var socialworkflowItemExtension = workflowItem.Extension;
            compositeSocialWorkflowItem = new Composite<SocialWorkflowItem, AddMemberRequest>(socialWorkflowItem, socialworkflowItemExtension);

            return compositeSocialWorkflowItem;
        }

        /// <summary>
        /// Returns a view model supporting the presentation of group
        /// membership moderation information.
        /// </summary>
        /// <param name="workflowId">Identifier for the selected membership moderation workflow</param>
        /// <returns>View model of moderation information</returns>
        public ModerationViewModel Get(string workflowId)
        {
            // Retrieve a collection of all workflows in the system.

            var allWorkflows = this.GetWorkflows();

            // Retrieve the workflow specified as the selected one.
            // If no workflow is selected, default to the first
            // available workflow.

            var selectedWorkflow = string.IsNullOrWhiteSpace(workflowId)
                ? allWorkflows.FirstOrDefault()
                : allWorkflows.FirstOrDefault(w => w.Id.ToString() == workflowId);

            // Retrieve the current state for all membership requests
            // under the selected moderation workflow.

            var currentWorkflowItems = this.GetWorkflowItemsFor(selectedWorkflow);

            var workflowAdapter = new WorkflowAdapter();
            var workflowItemAdapter = new WorkflowItemAdapter(selectedWorkflow);

            return new ModerationViewModel
            {
                Workflows = allWorkflows.Select(workflowAdapter.Adapt),
                SelectedWorkflow = workflowAdapter.Adapt(selectedWorkflow),
                Items = currentWorkflowItems.Select(item => workflowItemAdapter.Adapt(item))
            };
        }

        /// <summary>
        /// Takes action on the specified workflow item, representing a
        /// membership request.
        /// </summary>
        /// <param name="itemToActUpon">Membership request item to act upon</param>
        /// <param name="action">Moderation action to be taken</param>
        public void Moderate(MembershipRequestModel itemToActUpon, string action)
        {
            var workflowId = WorkflowId.Create(itemToActUpon.WorkflowId);

            var membershipRequest = new AddMemberRequest(itemToActUpon.Member, itemToActUpon.ExtensionData);

            var requestReference = membershipRequest.ToReference();

            var transitionToken = this.workflowService.BeginTransitionSession(workflowId, requestReference);

            try
            {
                // Retrieve the moderation workflow associated with
                // the item to be acted upon.

                var workflow = this.workflowService.Get(workflowId);

                // Leverage the workflow to determine what the
                // resulting state of the item will be upon taking 
                // the specified action.

                // Example: Current State: "Pending", Action: "Approve" => Transitioned State: "Approved"

                var transitionedState = workflow.Transition(
                    new WorkflowState(itemToActUpon.State),
                    new WorkflowAction(action)
                );

                var subsequentWorkflowItem = new WorkflowItem(
                    workflow.Id,
                    transitionedState,
                    membershipRequest.ToReference()
                );

                this.workflowItemService.Add(subsequentWorkflowItem, membershipRequest, transitionToken);

                // Perform any application logic given the item's
                // new state.

                if (this.IsApproved(subsequentWorkflowItem.State))
                {
                    memberservice.Add(new Member(membershipRequest.Member.UserReference, membershipRequest.Member.GroupId) , membershipRequest.ExtensionData);
                }
            }
            finally
            {
                this.workflowService.EndTransitionSession(transitionToken);
            }
        }

        /// <summary>
        /// Returns the moderation workflow supporting the specified group.
        /// </summary>
        /// <param name="group">ID of the group</param>
        /// <returns>Moderation workflow supporting the specified group</returns>
        public Workflow GetWorkflowFor(GroupId group)
        {
            var filterWorkflowsByGroup =
                FilterExpressionBuilder<MembershipModeration>.Field(m => m.Group.Id)
                                                             .EqualTo(group.Id);

            var criteria = new CompositeCriteria<WorkflowFilter, MembershipModeration>
            {
                PageInfo = new PageInfo { PageSize = 1 },
                ExtensionFilter = filterWorkflowsByGroup
            };

            var compositeWorkflows = this.workflowService.Get(criteria).Results;

            return compositeWorkflows.Count() > 0
                ? compositeWorkflows.First().Data
                : null;
        }

        /// <summary>
        /// Returns true if the specified group has a moderation workflow,
        /// false otherwise.
        /// </summary>
        /// <param name="group">ID of the group</param>
        /// <returns>True if the specified group has a moderation workflow, false otherwise</returns>
        public bool IsModerated(GroupId group)
        {
            return this.GetWorkflowFor(group) != null;
        }

        /// <summary>
        /// Retrieves a collection of the first 30 workflows in
        /// the system.
        /// </summary>
        /// <returns>Collection of workflows</returns>
        private IEnumerable<Workflow> GetWorkflows()
        {
            var criteria = new Criteria<WorkflowFilter>
            {
                PageInfo = new PageInfo { PageSize = 30 }
            };

            return this.workflowService.Get(criteria).Results;
        }

        /// <summary>
        /// Retrieves the first 30 current workflow items, associated with the
        /// specified workflow, which represent group membership requests.
        /// </summary>
        /// <param name="workflow">Workflow from which to retrieve items</param>
        /// <returns>Collection of workflow items</returns>
        private IEnumerable<Composite<WorkflowItem, AddMemberRequest>> GetWorkflowItemsFor(Workflow workflow)
        {
            IEnumerable<Composite<WorkflowItem, AddMemberRequest>> items;

            if (workflow == null)
            {
                items = new List<Composite<WorkflowItem, AddMemberRequest>>();
            }
            else
            {
                var criteria = new CompositeCriteria<WorkflowItemFilter, AddMemberRequest>
                {
                    Filter = new WorkflowItemFilter
                    {
                        ExcludeHistoricalItems = true,      // Include only the current state for the requests
                        Workflow = workflow.Id,             // Include only items for the selected group's workflow
                    },
                    PageInfo = new PageInfo { PageSize = 30 },   // Limit to 30 items                    
                };

                // Order the results alphabetically by their state and then
                // by the date on which they were created.

                criteria.OrderBy.Add(new SortInfo(WorkflowItemSortFields.State, true));
                criteria.OrderBy.Add(new SortInfo(WorkflowItemSortFields.Created, true));

                items = this.workflowItemService.Get(criteria).Results;
            }

            return items;
        }

        /// <summary>
        /// Returns true if the specified WorkflowState instance represents
        /// the "approved" state, false otherwise.
        /// </summary>
        /// <param name="state">State to verify</param>
        /// <returns>True if the specified WorkflowState instance represents the "Approved" state, false otherwise</returns>
        private bool IsApproved(WorkflowState state)
        {
            return state.Name.ToLower() == "approved";
        }
    }
}
