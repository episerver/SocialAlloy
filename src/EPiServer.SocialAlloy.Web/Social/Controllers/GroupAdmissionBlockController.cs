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
            var successMessage = TempData["GroupAdmissionSuccessMessage"] == null ? null : TempData["GroupAdmissionSuccessMessage"].ToString();
            var errorMessage = TempData["GroupAdmissionErrorMessage"] == null ? null : TempData["GroupAdmissionErrorMessage"].ToString();

            //populate model to pass to block view
            var groupAdmissionBlockModel = new GroupAdmissionBlockViewModel()
            {
                Heading = currentBlock.Heading,
                ShowHeading = currentBlock.ShowHeading,
                CurrentBlockLink = currentBlockLink,
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
        /// Submit handles the submission of new groups.  It accepts a GroupAdmissionBlockViewModel,
        /// stores the submitted group, and redirects back to the current page.
        /// </summary>
        /// <param name="groupCreationForm">The group being submitted.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Submit(GroupAdmissionBlockViewModel model)
        {
            var groupId = groupRepository.Get(model.GroupName).Id;

            if (GroupId.IsNullOrEmpty(groupId))
            {
                TempData["GroupAdmissionErrorMessage"] = "The group name provided does does not exist. An existing group is required for members to join.";
            }
            else
            {
                var validatedInputs = ValidateMemberInputs(model.MemberName, model.MemberEmail);
                AddMember(model, groupId, validatedInputs);
            }
            return Redirect(UrlResolver.Current.GetUrl(model.CurrentPageLink));
        }

        private void AddMember(GroupAdmissionBlockViewModel model, GroupId groupId, bool validatedInputs)
        {
            if (validatedInputs)
            {
                try
                {
                    var member = new SocialMember(Reference.Create(model.MemberName), groupId);
                    var extensionData = new MemberExtensionData(model.MemberEmail, model.MemberCompany);

                    memberRepository.Add(member, extensionData);
                    TempData["GroupAdmissionSuccessMessage"] = model.MemberName + " was added successfully to the group.";
                }
                catch (SocialRepositoryException ex)
                {
                    TempData["GroupAdmissionErrorMessage"] = ex.Message;
                }
            }
            else
            {
                TempData["GroupAdmissionErrorMessage"] = "The member name, email and company cannot be empty.";
            }
        }

        private bool ValidateMemberInputs(string userName, string userEmail)
        {
            return !string.IsNullOrWhiteSpace(userName) && !string.IsNullOrWhiteSpace(userEmail);
        }
    }
}