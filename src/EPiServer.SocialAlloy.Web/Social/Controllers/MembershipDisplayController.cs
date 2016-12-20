using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Social.Common;
using EPiServer.Social.Groups.Core;
using EPiServer.SocialAlloy.Web.Social.Blocks.Groups;
using EPiServer.SocialAlloy.Web.Social.Common.Controllers;
using EPiServer.SocialAlloy.Web.Social.Common.Models;
using EPiServer.SocialAlloy.Web.Social.Models;
using EPiServer.SocialAlloy.Web.Social.Models.Groups;
using EPiServer.SocialAlloy.Web.Social.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace EPiServer.SocialAlloy.Web.Social.Controllers
{
    /// <summary>
    /// The MembershipDisplayController handles the rendering of the list of members from the designated group configured in the admin view
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
        /// Render the membership display block view.
        /// </summary>
        /// <param name="currentBlock">The current block instance.</param>
        public override ActionResult Index(MembershipDisplayBlock currentBlock)
        {
            var currentBlockLink = ((IContent)currentBlock).ContentLink;

            //Populate model to pass to the membership display view
            var membershipDisplayBlockModel = new MembershipDisplayBlockViewModel()
            {
                Heading = currentBlock.Heading,
                ShowHeading = currentBlock.ShowHeading,
                GroupName = currentBlock.GroupName
            };

            //Retrieve the group id assigned to the block
            var groupId = groupRepository.Get(currentBlock.GroupName).Id;

            if (GroupId.IsNullOrEmpty(groupId))
            {
                membershipDisplayBlockModel.Messages = PopulateErrorMessage();
                membershipDisplayBlockModel.MemberList = new List<SocialCompositeMember>();
            }
            else
            {
                membershipDisplayBlockModel.Messages = new List<MessageViewModel>();
                membershipDisplayBlockModel.MemberList = RetrieveMemberList(currentBlock, groupId);
            }

            //Return block view with populated model
            return PartialView("~/Views/Social/MembershipDisplayBlock/Index.cshtml", membershipDisplayBlockModel);
        }

        /// <summary>
        /// Populates the messages that will be displayed to the user in the membership display view.
        /// </summary>
        /// <returns>A list of messages used to convey statuses to the user</returns>
        private List<MessageViewModel> PopulateErrorMessage()
        {
            var errorMessageBody = "The group name provided does does not exist. An existing group is required for members to join.";
            var errorMessage = new MessageViewModel { Body = errorMessageBody, Type = "error" };

            return new List<MessageViewModel> { errorMessage };
        }

        //Retrieves a list of members to populate the view model with. 
        private List<SocialCompositeMember> RetrieveMemberList(MembershipDisplayBlock currentBlock, GroupId groupId)
        {
            //Constructs a social member filter with groupId from the model and paging information that was configured in admin view
            var memberFilter = new SocialMemberFilter
            {
                GroupId = groupId,
                PageSize = currentBlock.DisplayPageSize
            };

            //Retrieves the list of members
            var memberList = memberRepository.Get(memberFilter);
            return ValidateMemberList(memberList);
        }

        //Validates the list of members
        private static List<SocialCompositeMember> ValidateMemberList(IEnumerable<SocialCompositeMember> memberList)
        {
            return memberList != null && memberList.Any() ? memberList.ToList() : new List<SocialCompositeMember>();
        }
    }
}