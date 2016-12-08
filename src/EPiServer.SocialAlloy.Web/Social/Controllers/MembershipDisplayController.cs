using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Social.Groups.Core;
using EPiServer.SocialAlloy.Web.Social.Blocks.Groups;
using EPiServer.SocialAlloy.Web.Social.Common.Controllers;
using EPiServer.SocialAlloy.Web.Social.Models.Groups;
using EPiServer.SocialAlloy.Web.Social.Repositories;
using EPiServer.Web.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EPiServer.SocialAlloy.Web.Social.Controllers
{
    /// <summary>
    /// The MembershipDisplayController handles the rendering the list of members of a pre-designated group in the MembershipDisplayBlockView
    /// </summary>
    public class MembershipDisplayController : SocialBlockController<MembershipDisplayBlock>
    {
        private readonly ISocialGroupRepository groupRepository;
        private readonly ISocialMemberRepository memberRepository;
        private const string SubmitSuccessMessage = "SubmitSuccessMessage";
        private const string SubmitErrorMessage = "SubmitErrorMessage";

        public MembershipDisplayController()
        {
            groupRepository = ServiceLocator.Current.GetInstance<ISocialGroupRepository>();
            memberRepository = ServiceLocator.Current.GetInstance<ISocialMemberRepository>();
        }

        /// <summary>
        /// Render the comment block frontend view.
        /// </summary>
        /// <param name="currentBlock">The current frontend block instance.</param>
        /// <returns></returns>
        public override ActionResult Index(MembershipDisplayBlock currentBlock)
        {
            var currentBlockLink = ((IContent)currentBlock).ContentLink;
            var successMessage = TempData["SubmitSuccessMessage"] == null ? null : TempData["SubmitSuccessMessage"].ToString();
            var errorMessage = TempData["SubmitErrorMessage"] == null ? null : TempData["SubmitErrorMessage"].ToString();

            //populate model to pass to block view
            var membershipDisplayBlockModel = new MembershipDisplayBlockViewModel()
            {
                Heading = currentBlock.Heading,
                CurrentBlockLink = currentBlockLink,
                PageId = pageRouteHelper.Page.ContentGuid.ToString(),
                CurrentPageLink = pageRouteHelper.PageLink,
                SubmitSuccessMessage = successMessage,
                SubmitErrorMessage = errorMessage,
                GroupName = currentBlock.GroupName
            };

            //retieve the group id assigned to the block
            var groupId = groupRepository.Get(currentBlock.GroupName).Id;

            //validate the groupId
            if (GroupId.IsNullOrEmpty(groupId))
            {
                TempData["SubmitErrorMessage"] = "The group name provided does does not exist. An existing group is required for members to join.";
            }
            else
            {
                var memberList = memberRepository.Get(groupId).ToList();
                if (memberList != null && memberList.Any())
                {
                    membershipDisplayBlockModel.MemberList = memberList;
                }
            }

            //return block view
            return PartialView("~/Views/Social/MembershipDisplayBlock/Index.cshtml", membershipDisplayBlockModel);
        }


        private bool ValidateMemberInputs(string userName, string userEmail)
        {
            return !string.IsNullOrWhiteSpace(userName) && !string.IsNullOrWhiteSpace(userEmail);
        }
    }
}