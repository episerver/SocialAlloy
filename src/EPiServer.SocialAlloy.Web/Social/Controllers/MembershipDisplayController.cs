using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Social.Common;
using EPiServer.Social.Groups.Core;
using EPiServer.SocialAlloy.Web.Social.Blocks.Groups;
using EPiServer.SocialAlloy.Web.Social.Common.Controllers;
using EPiServer.SocialAlloy.Web.Social.Models;
using EPiServer.SocialAlloy.Web.Social.Models.Groups;
using EPiServer.SocialAlloy.Web.Social.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace EPiServer.SocialAlloy.Web.Social.Controllers
{
    /// <summary>
    /// The MembershipDisplayController handles the rendering of the list of members  from the designated group set in the admin view
    /// </summary>
    public class MembershipDisplayController : SocialBlockController<MembershipDisplayBlock>
    {
        private readonly ISocialGroupRepository groupRepository;
        private readonly ISocialMemberRepository memberRepository;

        public MembershipDisplayController()
        {
            groupRepository = ServiceLocator.Current.GetInstance<ISocialGroupRepository>();
            memberRepository = ServiceLocator.Current.GetInstance<ISocialMemberRepository>();
        }

        /// <summary>
        /// Render the Membership Display block view.
        /// </summary>
        /// <param name="currentBlock">The current block instance.</param>
        /// <returns></returns>
        public override ActionResult Index(MembershipDisplayBlock currentBlock)
        {
            var currentBlockLink = ((IContent)currentBlock).ContentLink;

            //populate model to pass to view
            var membershipDisplayBlockModel = new MembershipDisplayBlockViewModel()
            {
                Heading = currentBlock.Heading,
                ShowHeading = currentBlock.ShowHeading,
                GroupName = currentBlock.GroupName
            };

            //retieve the group id assigned to the block
            var groupId = groupRepository.Get(currentBlock.GroupName).Id;

            if (GroupId.IsNullOrEmpty(groupId))
            {
                membershipDisplayBlockModel.SubmitErrorMessage = "The group name provided does does not exist. An existing group is required for members to join.";
                membershipDisplayBlockModel.MemberList = new List<Composite<Member, MemberExtensionData>>();
            }
            else
            {
                RetrieveMemberList(currentBlock, membershipDisplayBlockModel, groupId);
            }

            //return block view
            return PartialView("~/Views/Social/MembershipDisplayBlock/Index.cshtml", membershipDisplayBlockModel);
        }

        //Retrieves a list of members to populates the view model with. 
        private void RetrieveMemberList(MembershipDisplayBlock currentBlock, MembershipDisplayBlockViewModel membershipDisplayBlockModel, GroupId groupId)
        {
            var memberFilter = new SocialMemberFilter
            {
                GroupId = groupId,
                PageOffset = currentBlock.DisplayPageOffset,
                PageSize = currentBlock.DisplayPageSize
            };

            var memberList = memberRepository.Get(memberFilter);

            ValidateMemberList(membershipDisplayBlockModel, memberList);
        }

        private static void ValidateMemberList(MembershipDisplayBlockViewModel membershipDisplayBlockModel, IEnumerable<Composite<Member, MemberExtensionData>> memberList)
        {
            membershipDisplayBlockModel.MemberList = memberList != null && memberList.Any() ? memberList.ToList() : new List<Composite<Member, MemberExtensionData>>();
        }

        private bool ValidateMemberInputs(string userName, string userEmail)
        {
            return !string.IsNullOrWhiteSpace(userName) && !string.IsNullOrWhiteSpace(userEmail);
        }
    }
}