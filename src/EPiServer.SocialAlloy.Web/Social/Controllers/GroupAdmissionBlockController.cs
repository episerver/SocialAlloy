using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Social.Common;
using EPiServer.Social.Groups.Core;
using EPiServer.SocialAlloy.Web.Social.Blocks.Groups;
using EPiServer.SocialAlloy.Web.Social.Common.Controllers;
using EPiServer.SocialAlloy.Web.Social.Common.Exceptions;
using EPiServer.SocialAlloy.Web.Social.Common.Models;
using EPiServer.SocialAlloy.Web.Social.Models;
using EPiServer.SocialAlloy.Web.Social.Models.Groups;
using EPiServer.SocialAlloy.Web.Social.Models.Moderation;
using EPiServer.SocialAlloy.Web.Social.Repositories;
using EPiServer.Web.Routing;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace EPiServer.SocialAlloy.Web.Social.Controllers
{
    /// <summary>
    /// The GroupAdmissionBlockController handles rendering the Group Admission block view for adding new members to a group
    /// </summary>
    public class GroupAdmissionBlockController : SocialBlockController<GroupAdmissionBlock>
    {
        private readonly ISocialGroupRepository groupRepository;
        private readonly ISocialMemberRepository memberRepository;
        private readonly ISocialModerationRepository moderationRepository;

        /// <summary>
        /// Constructor for admission block
        /// </summary>
        public GroupAdmissionBlockController()
        {
            groupRepository = ServiceLocator.Current.GetInstance<ISocialGroupRepository>();
            memberRepository = ServiceLocator.Current.GetInstance<ISocialMemberRepository>();
            moderationRepository = ServiceLocator.Current.GetInstance<ISocialModerationRepository>();
        }

        /// <summary>
        /// Render the Group Admission block view.
        /// </summary>
        /// <param name="currentBlock">The current block instance.</param>
        /// <returns></returns>
        public override ActionResult Index(GroupAdmissionBlock currentBlock)
        {
            var currentBlockLink = ((IContent)currentBlock).ContentLink;

            //Populate model to pass to block view
            var blockModel = new GroupAdmissionBlockViewModel()
            {
                Heading = currentBlock.Heading,
                ShowHeading = currentBlock.ShowHeading,
                CurrentBlockLink = currentBlockLink,
                CurrentPageLink = pageRouteHelper.PageLink,
            };

            //Retrieves moderation information for the model to display in the view
            try
            {
                var group = groupRepository.Get(currentBlock.GroupName);
                //Validate that the group exists 
                if (group != null)
                {
                    var groupId = group.Id;
                    blockModel.GroupName = currentBlock.GroupName;
                    blockModel.GroupId = groupId.ToString();
                    blockModel.IsModerated = moderationRepository.IsModerated(groupId);
                }
                else
                {
                    var errorMessage = "The group configured for this block cannot be found. Please update the block to use an existing group.";
                    AddToTempData("GroupAdmissionErrorMessage", errorMessage);
                }
            }
            catch (SocialRepositoryException ex)
            {
                AddToTempData("GroupAdmissionErrorMessage", ex.Message);
            }
            catch (GroupDoesNotExistException ex)
            {
                AddToTempData("GroupAdmissionErrorMessage", ex.Message);
            }
            blockModel.Messages = PopulateMessages();
            //Remove existing values from input fields
            ModelState.Clear();

            //Return block view
            return PartialView("~/Views/Social/GroupAdmissionBlock/Index.cshtml", blockModel);
        }

        /// <summary>
        /// Submit handles the admission of new members to existing groups.  It accepts a GroupAdmissionBlockViewModel,
        /// stores the submitted group, and redirects back to the current page.
        /// </summary>
        /// <param name="blockModel">The group admission model being submitted.</param>
        [HttpPost]
        public ActionResult Submit(GroupAdmissionBlockViewModel blockModel)
        {
            try
            {
                AddMember(blockModel, blockModel.GroupId);
            }
            catch (SocialRepositoryException ex)
            {
                AddToTempData("GroupAdmissionErrorMessage", ex.Message);
            }

            return Redirect(UrlResolver.Current.GetUrl(blockModel.CurrentPageLink));
        }

        /// <summary>
        /// Determines how a member is added to a group. 
        /// If the group is moderated a request for membership is added into the group moderation workflow. 
        /// If the group is not moderated the member is  added to the underlying membership repository
        /// </summary>
        /// <param name="blockModel">The viewmodel for the GroupAdmission view</param>
        /// <param name="groupId">The id of the group that the member is being added to</param>
        private void AddMember(GroupAdmissionBlockViewModel blockModel, string groupId)
        {
            var validatedInputs = ValidateMemberInputs(blockModel.MemberName, blockModel.MemberEmail);
            if (validatedInputs)
            {
                try
                {
                    //Populated the SocialMember and extension data
                    var member = new SocialMember(blockModel.MemberName, groupId);
                    var extensionData = new MemberExtensionData(blockModel.MemberEmail, blockModel.MemberCompany);

                    if (blockModel.IsModerated)
                    {
                        //Adds request for membership into moderation workflow
                        this.AddAModeratedMember(member, extensionData);
                    }
                    else
                    {
                        //Add the new member with extension data and persist the group name in temp data to be used in the success message
                        memberRepository.Add(member, extensionData);
                        var successMessage = blockModel.MemberName + " was added successfully to the group.";
                        AddToTempData("GroupAdmissionSuccessMessage", successMessage);
                    }
                }
                catch (SocialRepositoryException ex)
                {
                    //Persist the exception message in temp data to be used in the error message
                    AddToTempData("GroupAdmissionErrorMessage", ex.Message);
                }
            }
            else
            {
                //Persist the message in temp data to be used in the error message
                var errorMessage = "The member name, email and company cannot be empty.";
                AddToTempData("GroupAdmissionErrorMessage", errorMessage);
            }
        }

        /// <summary>
        /// Submits a membership request to the specified group's
        /// moderation workflow for approval.
        /// </summary>
        /// <param name="member">The member information for the membership request</param>
        /// <param name="extensionData">The extension data for the membership request</param>
        private void AddAModeratedMember(SocialMember member, MemberExtensionData extensionData)
        {
            // Prepare an instance of a data object describing the membership
            // request. This will be captured as extension data associated with 
            // our workflow item.

            var membershipRequest = new AddMemberRequest(member, extensionData);

            // Retrieve the workflow supporting moderation of
            // membership for the group to which the user is
            // being added.

            var moderationWorkflow = this.moderationRepository.GetWorkflowFor(member.GroupId);

            // The workflow defines the intial (or 'start') state
            // for moderation.

            var initialState = moderationWorkflow.InitialState;

            // Define a unique reference representing the entity
            // under moderation. Note that this entity may be
            // transient or may not yet have been assigned a
            // unique identifier. Defining an item reference allows
            // you to bridge this gap.

            // For example: "members:/{group-id}/{user-reference}"

            var targetReference = membershipRequest.ToReference();
       
            // Create a new workflow item...

            var membershipRequestWorkflowRecord = new SocialWorkflowItem(
                moderationWorkflow.Id,      // ...under the group's moderation workflow
                initialState,               // ...in the workflow's initial state
                targetReference.Id             // ...identified with this reference
            );

            this.moderationRepository.Add(membershipRequestWorkflowRecord, membershipRequest);
        }

        /// <summary>
        /// Populates the messages that will be displayed to the user in the group admission view.
        /// </summary>
        /// <returns>A list of messages used to convey statuses to the user</returns>
        private List<MessageViewModel> PopulateMessages()
        {
            var successMessageBody = GetFromTempData<string>("GroupAdmissionSuccessMessage");
            var successMessage = new MessageViewModel { Body = successMessageBody, Type = "success" };

            var errorMessageBody = GetFromTempData<string>("GroupAdmissionErrorMessage");
            var errorMessage = new MessageViewModel { Body = errorMessageBody, Type = "error" };

            return new List<MessageViewModel> { successMessage, errorMessage };
        }

        //Validates the user name and user email
        private bool ValidateMemberInputs(string userName, string userEmail)
        {
            return !string.IsNullOrWhiteSpace(userName) && !string.IsNullOrWhiteSpace(userEmail);
        }
    }
}