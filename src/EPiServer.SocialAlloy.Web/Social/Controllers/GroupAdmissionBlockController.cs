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
using EPiServer.SocialAlloy.Web.Social.Repositories;
using EPiServer.Web.Routing;
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

        public GroupAdmissionBlockController()
        {
            groupRepository = ServiceLocator.Current.GetInstance<ISocialGroupRepository>();
            memberRepository = ServiceLocator.Current.GetInstance<ISocialMemberRepository>();
        }

        /// <summary>
        /// Render the Group Admission block view.
        /// </summary>
        /// <param name="currentBlock">The current block instance.</param>
        /// <returns></returns>
        public override ActionResult Index(GroupAdmissionBlock currentBlock)
        {
            var currentBlockLink = ((IContent)currentBlock).ContentLink;
            List<MessageViewModel> listOfMessages = PopulateMessages();


            //populate model to pass to block view
            var groupAdmissionBlockModel = new GroupAdmissionBlockViewModel()
            {
                Heading = currentBlock.Heading,
                ShowHeading = currentBlock.ShowHeading,
                CurrentBlockLink = currentBlockLink,
                CurrentPageLink = pageRouteHelper.PageLink,
                Messages = listOfMessages,
                GroupName = currentBlock.GroupName
            };

            //remove existing values from input fields
            ModelState.Clear();

            //return block view
            return PartialView("~/Views/Social/GroupAdmissionBlock/Index.cshtml", groupAdmissionBlockModel);
        }

        /// <summary>
        /// Submit handles the admission of new members to existing groups.  It accepts a GroupAdmissionBlockViewModel,
        /// stores the submitted group, and redirects back to the current page.
        /// </summary>
        /// <param name="model">The group admission model being submitted.</param>
        [HttpPost]
        public ActionResult Submit(GroupAdmissionBlockViewModel model)
        {
            var groupId = groupRepository.Get(model.GroupName).Id;

            //Validate the group id passed from the model
            if (GroupId.IsNullOrEmpty(groupId))
            {
                //Persist the message in temp data to be used in the error message
                var errorMessage = "The group name provided does does not exist. An existing group is required for members to join.";
                AddToTempData("GroupAdmissionErrorMessage", errorMessage);
            }
            else
            {
                AddMember(model, groupId);
            }
            return Redirect(UrlResolver.Current.GetUrl(model.CurrentPageLink));
        }

        private void AddMember(GroupAdmissionBlockViewModel model, GroupId groupId)
        {
            var validatedInputs = ValidateMemberInputs(model.MemberName, model.MemberEmail);
            if (validatedInputs)
            {
                try
                {
                    //Add the new member with extension data and persist the group name in temp data to be used in the success message
                    var member = new SocialMember(Reference.Create(model.MemberName), groupId);
                    var extensionData = new MemberExtensionData(model.MemberEmail, model.MemberCompany);

                    memberRepository.Add(member, extensionData);
                    var successMessage = model.MemberName + " was added successfully to the group.";
                    AddToTempData("GroupAdmissionSuccessMessage", successMessage);
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