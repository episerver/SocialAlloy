using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Social.Common;
using EPiServer.Social.Groups.Core;
using EPiServer.SocialAlloy.Web.Social.Blocks.Groups;
using EPiServer.SocialAlloy.Web.Social.Common.Controllers;
using EPiServer.SocialAlloy.Web.Social.Common.Exceptions;
using EPiServer.SocialAlloy.Web.Social.Models;
using EPiServer.SocialAlloy.Web.Social.Models.Groups;
using EPiServer.SocialAlloy.Web.Social.Repositories;
using EPiServer.Web.Routing;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;

namespace EPiServer.SocialAlloy.Web.Social.Controllers
{
    /// <summary>
    /// The GroupAdmissionBlockController handles the rendering the group admission  block frontend view.
    /// </summary>
    public class GroupAdmissionBlockController : SocialBlockController<GroupAdmissionBlock>
    {
        private readonly ISocialGroupRepository groupRepository;
        private readonly ISocialMemberRepository memberRepository;
        private const string SubmitSuccessMessage = "SubmitSuccessMessage";
        private const string SubmitErrorMessage = "SubmitErrorMessage";

        public GroupAdmissionBlockController()
        {
            groupRepository = ServiceLocator.Current.GetInstance<ISocialGroupRepository>();
            memberRepository = ServiceLocator.Current.GetInstance<ISocialMemberRepository>();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="groupService"></param>
        /// <param name="contentRepository"></param>
        public GroupAdmissionBlockController(ISocialGroupRepository groupRepository)
        {
            this.groupRepository = groupRepository;
        }

        /// <summary>
        /// Render the comment block frontend view.
        /// </summary>
        /// <param name="currentBlock">The current frontend block instance.</param>
        /// <returns></returns>
        public override ActionResult Index(GroupAdmissionBlock currentBlock)
        {
            var currentBlockLink = ((IContent)currentBlock).ContentLink;
            var successMessage = TempData["SubmitSuccessMessage"] == null ? null : TempData["SubmitSuccessMessage"].ToString();
            var errorMessage = TempData["SubmitErrorMessage"] == null ? null : TempData["SubmitErrorMessage"].ToString();

            //populate model to pass to block view
            var groupAdmissionBlockModel = new GroupAdmissionBlockViewModel()
            {
                Heading = currentBlock.Heading,
                CurrentBlockLink = currentBlockLink,
                PageId = pageRouteHelper.Page.ContentGuid.ToString(),
                CurrentPageLink = pageRouteHelper.PageLink,
                SubmitSuccessMessage = successMessage,
                SubmitErrorMessage = errorMessage,
                GroupName = currentBlock.GroupName
            };

            //remove existing values from input fields
            ModelState.Clear();

            //return block view
            return PartialView("~/Views/Social/GroupAdmissionBlock/Index.cshtml", groupAdmissionBlockModel);
        }

        /// <summary>
        /// Submit handles the submitting of new groups.  It accepts a group creation form model,
        /// stores the submitted group, and redirects back to the current page.
        /// </summary>
        /// <param name="groupCreationForm">The group form being submitted.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Submit(GroupAdmissionBlockViewModel model)
        {
            var groupId = groupRepository.Get(model.GroupName).Id;

            if (GroupId.IsNullOrEmpty(groupId))
            {
                TempData["SubmitErrorMessage"] = "The group name provided does does not exist. An existing group is required for members to join.";
            }
            else
            {
                var validatedInputs = ValidateMemberInputs(model.MemberName, model.MemberEmail);
                if (validatedInputs)
                {
                    AddMember(model, groupId);
                }
                else
                {
                    TempData["SubmitErrorMessage"] = "The member name, email and company cannot be empty.";
                }
            }
            return Redirect(UrlResolver.Current.GetUrl(model.CurrentPageLink));
        }

        private void AddMember(GroupAdmissionBlockViewModel model, GroupId groupId)
        {
            try
            {
                var member = new Member(Reference.Create(model.MemberName), groupId);
                var extensionData = new MemberExtensionData(model.MemberEmail, model.MemberCompany);

                memberRepository.Add(member, extensionData);
                TempData["SubmitSuccessMessage"] = model.MemberName + " was added successfully to the" + model.GroupName + "group!";
            }
            catch (SocialRepositoryException ex)
            {
                TempData["SubmitErrorMessage"] = ex.Message;
            }
        }

        private bool ValidateMemberInputs(string userName, string userEmail)
        {
            return !string.IsNullOrWhiteSpace(userName) && !string.IsNullOrWhiteSpace(userEmail);
        }
    }
}