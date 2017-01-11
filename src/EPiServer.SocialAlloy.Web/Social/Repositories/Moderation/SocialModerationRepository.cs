using EPiServer.Social.Common;
using EPiServer.Social.Groups.Core;
using EPiServer.Social.Moderation.Core;
using EPiServer.SocialAlloy.Web.Social.Adapters.Moderation;
using EPiServer.SocialAlloy.Web.Social.Common.Exceptions;
using EPiServer.SocialAlloy.Web.Social.Models;
using EPiServer.SocialAlloy.Web.Social.Models.Groups;
using EPiServer.SocialAlloy.Web.Social.Models.Moderation;
using System.Collections.Generic;
using System.Linq;

namespace EPiServer.SocialAlloy.Web.Social.Repositories.Moderation
{
    /// <summary>
    /// The SocialModerationRepository class defines the operations that can be issued
    /// against the EPiServer Social moderation repository.
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
        /// <param name="group">The group that will be associated with the workflow</param>
        public void Add(SocialGroup group)
        {
            // Define the transitions for workflow:            
            // Pending -> (Accept) -> Accepted
            //     |                      |-- (Approve) -> Approved
            //     |                       `- (Reject)  -> Rejected
            //      `---> (Ignore) -> Rejected

            var workflowTransitions = new List<WorkflowTransition>
            {
                new WorkflowTransition(new WorkflowState("Pending"),  new WorkflowState("Accepted"), new WorkflowAction("Accept")),
                new WorkflowTransition(new WorkflowState("Pending"),  new WorkflowState("Rejected"), new WorkflowAction("Ignore")),
                new WorkflowTransition(new WorkflowState("Accepted"), new WorkflowState("Approved"), new WorkflowAction("Approve")),
                new WorkflowTransition(new WorkflowState("Accepted"), new WorkflowState("Rejected"), new WorkflowAction("Reject"))
            };

            // Save the new workflow with custom extension data which 
            // identifies the group it is intended to be associated with.

            var membershipWorkflow = new Workflow(
                "Membership: " + group.Name,
                workflowTransitions,
                new WorkflowState("Pending")
            );

            var workflowExtension = new MembershipModeration { Group = GroupId.Create(group.Id) };


            if (membershipWorkflow != null)
            {
                try
                {
                    this.workflowService.Add(membershipWorkflow, workflowExtension);
                }
                catch (SocialAuthenticationException ex)
                {
                    throw new SocialRepositoryException("The application failed to authenticate with EPiServer social.", ex);
                }
                catch (MaximumDataSizeExceededException ex)
                {
                    throw new SocialRepositoryException("The application request was deemed too large for EPiServer Social.", ex);
                }
                catch (SocialCommunicationException ex)
                {
                    throw new SocialRepositoryException("The application failed to communicate with EPiServer Social.", ex);
                }
                catch (SocialException ex)
                {
                    throw new SocialRepositoryException("EPiServer Social failed to process the application request.", ex);
                }
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
                try
                {
                    this.workflowItemService.Add(workflowItem, membershipRequest);
                }
                catch (SocialAuthenticationException ex)
                {
                    throw new SocialRepositoryException("The application failed to authenticate with EPiServer social.", ex);
                }
                catch (MaximumDataSizeExceededException ex)
                {
                    throw new SocialRepositoryException("The application request was deemed too large for EPiServer Social.", ex);
                }
                catch (SocialCommunicationException ex)
                {
                    throw new SocialRepositoryException("The application failed to communicate with EPiServer Social.", ex);
                }
                catch (SocialException ex)
                {
                    throw new SocialRepositoryException("EPiServer Social failed to process the application request.", ex);
                }
            }
        }

        /// <summary>
        /// Retrieves specific workflowitem extension data from the underlying repository
        /// </summary>
        /// <param name="user">The user under moderation</param>
        /// <param name="group">The group that membership is being moderated</param>
        /// <returns>AddMemberRequest: the workflowItem extension data</returns>
        public AddMemberRequest Get(string user, string group)
        {
            AddMemberRequest memberRequest = null;

            //Construct a filter to return the desired target under moderation
            var filter = new CompositeCriteria<WorkflowItemFilter, AddMemberRequest>();
            filter.Filter.Target = Reference.Create(CreateUri(group, user));

            try
            {
                //retrieve the first workflow that matches the target filter 
                var workflowItem = this.workflowItemService.Get(filter).Results.FirstOrDefault();
                memberRequest = workflowItem.Extension;
            }
            catch (SocialAuthenticationException ex)
            {
                throw new SocialRepositoryException("The application failed to authenticate with EPiServer social.", ex);
            }
            catch (MaximumDataSizeExceededException ex)
            {
                throw new SocialRepositoryException("The application request was deemed too large for EPiServer Social.", ex);
            }
            catch (SocialCommunicationException ex)
            {
                throw new SocialRepositoryException("The application failed to communicate with EPiServer Social.", ex);
            }
            catch (SocialException ex)
            {
                throw new SocialRepositoryException("EPiServer Social failed to process the application request.", ex);
            }

            return memberRequest;
        }

        /// <summary>
        /// Returns a view model supporting the presentation of group
        /// membership moderation information.
        /// </summary>
        /// <param name="workflowId">Identifier for the selected membership moderation workflow</param>
        /// <returns>View model of moderation information</returns>
        public ModerationViewModel Get(string workflowId)
        {
            try
            {
                // Retrieve a collection of all workflows in the system with MembershipModeration extension data.
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
            catch (SocialAuthenticationException ex)
            {
                throw new SocialRepositoryException("The application failed to authenticate with EPiServer social.", ex);
            }
            catch (MaximumDataSizeExceededException ex)
            {
                throw new SocialRepositoryException("The application request was deemed too large for EPiServer Social.", ex);
            }
            catch (SocialCommunicationException ex)
            {
                throw new SocialRepositoryException("The application failed to communicate with EPiServer Social.", ex);
            }
            catch (SocialException ex)
            {
                throw new SocialRepositoryException("EPiServer Social failed to process the application request.", ex);
            }
        }

        /// <summary>
        /// Takes action on the specified workflow item, representing a
        /// membership request.
        /// </summary>
        /// <param name="workflowId">The id of the workflow </param>
        /// <param name="socialMember">The member that under moderation for group admission</param>
        /// <param name="memberExtensionData">The extension data for a member under moderation  </param>
        /// <param name="action">The moderation action to be taken</param>
        public void Moderate(string workflowId, SocialMember socialMember, MemberExtensionData memberExtensionData, string action)
        {
            var populatedWorkflowId = WorkflowId.Create(workflowId);

            var requestReference = Reference.Create(CreateUri(socialMember.GroupId, socialMember.UserReference));

            var membershipRequest = new AddMemberRequest(socialMember, memberExtensionData);

            try
            {
                var transitionToken = this.workflowService.BeginTransitionSession(populatedWorkflowId, requestReference);

                try
                {
                    // Retrieve the moderation workflow associated with
                    // the item to be acted upon.

                    var workflow = this.workflowService.Get(populatedWorkflowId);

                    // Leverage the workflow to determine what the
                    // resulting state of the item will be upon taking 
                    // the specified action.

                    //retrieve the current state of the workflow item once the begintransitionsession begins.
                    var filter = new WorkflowItemFilter { Target = requestReference };
                    var criteria = new Criteria<WorkflowItemFilter> { Filter = filter };
                    var workflowItem = this.workflowItemService.Get(criteria).Results.Last();

                    // Example: Current State: "Pending", Action: "Approve" => Transitioned State: "Approved"
                    var transitionedState = workflow.Transition(workflowItem.State, new WorkflowAction(action));

                    var subsequentWorkflowItem = new WorkflowItem(
                        workflow.Id,
                        transitionedState,
                        requestReference
                    );

                    this.workflowItemService.Add(subsequentWorkflowItem, membershipRequest, transitionToken);

                    // Perform any application logic given the item's
                    // new state.

                    if (this.IsApproved(subsequentWorkflowItem.State))
                    {
                        memberservice.Add(new Member(Reference.Create(membershipRequest.Member.UserReference), GroupId.Create(membershipRequest.Member.GroupId)), membershipRequest.ExtensionData);
                    }
                }
                finally
                {
                    this.workflowService.EndTransitionSession(transitionToken);
                }
            }
            catch (SocialAuthenticationException ex)
            {
                throw new SocialRepositoryException("The application failed to authenticate with EPiServer social.", ex);
            }
            catch (MaximumDataSizeExceededException ex)
            {
                throw new SocialRepositoryException("The application request was deemed too large for EPiServer Social.", ex);
            }
            catch (SocialCommunicationException ex)
            {
                throw new SocialRepositoryException("The application failed to communicate with EPiServer Social.", ex);
            }
            catch (SocialException ex)
            {
                throw new SocialRepositoryException("EPiServer Social failed to process the application request.", ex);
            }
        }

        /// <summary>
        /// Returns the moderation workflow supporting the specified group.
        /// </summary>
        /// <param name="group">ID of the group</param>
        /// <returns>Moderation workflow supporting the specified group</returns>
        public SocialWorkflow GetWorkflowFor(string group)
        {
            SocialWorkflow expectedSocialWorkflow = null;
            IEnumerable<Composite<Workflow, MembershipModeration>> listOfWorkflow = Enumerable.Empty<Composite<Workflow, MembershipModeration>>();

            var filterWorkflowsByGroup =
                FilterExpressionBuilder<MembershipModeration>.Field(m => m.Group.Id)
                                                             .EqualTo(group);

            var criteria = new CompositeCriteria<WorkflowFilter, MembershipModeration>
            {
                PageInfo = new PageInfo { PageSize = 1 },
                ExtensionFilter = filterWorkflowsByGroup
            };

            try
            {
                listOfWorkflow = this.workflowService.Get(criteria).Results;
                if (listOfWorkflow.Count() > 0)
                {
                    var workflow = listOfWorkflow.First().Data;
                    expectedSocialWorkflow = new SocialWorkflow(workflow.Id.Id, workflow.Name, workflow.InitialState.Name);
                }
            }
            catch (SocialAuthenticationException ex)
            {
                throw new SocialRepositoryException("The application failed to authenticate with EPiServer social.", ex);
            }
            catch (MaximumDataSizeExceededException ex)
            {
                throw new SocialRepositoryException("The application request was deemed too large for EPiServer Social.", ex);
            }
            catch (SocialCommunicationException ex)
            {
                throw new SocialRepositoryException("The application failed to communicate with EPiServer Social.", ex);
            }
            catch (SocialException ex)
            {
                throw new SocialRepositoryException("EPiServer Social failed to process the application request.", ex);
            }

            return expectedSocialWorkflow;
        }

        /// <summary>
        /// Returns true if the specified group has a moderation workflow,
        /// false otherwise.
        /// </summary>
        /// <param name="groudId">ID of the group</param>
        /// <returns>True if the specified group has a moderation workflow, false otherwise</returns>
        public bool IsModerated(string groudId)
        {
            return this.GetWorkflowFor(groudId) != null;
        }

        /// <summary>
        /// Creates a unique uri to be used with to track the progression of a member being moderated for group admission 
        /// </summary>
        /// <param name="group">Id of the group that a member is trying to join</param>
        /// <param name="user">The name of the member that is trying to join a group</param>
        /// <returns></returns>
        public string CreateUri(string group, string user)
        {
            return
               string.Format(
                   "members://{0}/{1}",
                   group,
                   user
               );
        }

        /// <summary>
        /// Retrieves a collection of the first 30 workflows in
        /// the system.
        /// </summary>
        /// <returns>Collection of workflows</returns>
        private IEnumerable<Workflow> GetWorkflows()
        {
            var criteria = new CompositeCriteria<WorkflowFilter, MembershipModeration>
            {
                PageInfo = new PageInfo { PageSize = 30 }
            };

            return this.workflowService.Get(criteria).Results.Select(x => x.Data);
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
