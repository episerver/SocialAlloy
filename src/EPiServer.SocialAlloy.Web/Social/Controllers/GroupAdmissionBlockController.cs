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

            //populate model to pass to block view
            var blockModel = new GroupAdmissionBlockViewModel()
            {
                Heading = currentBlock.Heading,
                ShowHeading = currentBlock.ShowHeading,
                CurrentBlockLink = currentBlockLink,
                CurrentPageLink = pageRouteHelper.PageLink,
            };

            //retrieving moderation infromation for the model to display in the view
            try
            {
                var groupId = groupRepository.Get(currentBlock.GroupName).Id;
                blockModel.GroupName = currentBlock.GroupName;
                blockModel.GroupId = groupId.ToString();
                blockModel.IsModerated = moderationRepository.IsModerated(groupId);
                blockModel.Messages = PopulateMessages();
            }
            catch (SocialRepositoryException ex)
            {
                AddToTempData("GroupAdmissionErrorMessage", ex.Message);
            }
            catch (NullReferenceException)
            {
                var errorMessage = "The group configured for this block cannot be found. Please update the block to use an existing group.";
                AddToTempData("GroupAdmissionErrorMessage", errorMessage);
            }

            //remove existing values from input fields
            ModelState.Clear();

            //return block view
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
                AddMember(blockModel, GroupId.Create(blockModel.GroupId));
            }
            catch (SocialRepositoryException ex)
            {
                AddToTempData("GroupAdmissionErrorMessage", ex.Message);
            }
            catch (NullReferenceException)
            {
                var errorMessage = "The group configured for this block cannot be found. Please update the block to use an existing group.";
                AddToTempData("GroupAdmissionErrorMessage", errorMessage);
            }

            return Redirect(UrlResolver.Current.GetUrl(blockModel.CurrentPageLink));
        }

        private void AddMember(GroupAdmissionBlockViewModel blockModel, GroupId groupId)
        {
            var validatedInputs = ValidateMemberInputs(blockModel.MemberName, blockModel.MemberEmail);
            if (validatedInputs)
            {
                try
                {
                    //populated the SocialMember and extension data
                    var member = new SocialMember(Reference.Create(blockModel.MemberName), groupId);
                    var extensionData = new MemberExtensionData(blockModel.MemberEmail, blockModel.MemberCompany);

                    if (blockModel.IsModerated)
                    {
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
        /// <param name="groupId">Group to which a member should be added</param>
        /// <param name="userReference">User who is pending approval for membership</param>
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
                targetReference             // ...identified with this reference
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