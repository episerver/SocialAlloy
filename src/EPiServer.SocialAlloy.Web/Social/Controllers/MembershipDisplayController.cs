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
using System;
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
                GroupName = currentBlock.GroupName,
                Messages = new List<MessageViewModel>(),
                MemberList = new List<SocialCompositeMember>()
            };

            GroupId groupId = GroupId.Empty;
            
            //Retrieve the group id assigned to the block and populate the memberlist 
            try
            {
                var group = groupRepository.Get(currentBlock.GroupName);
                groupId = group.Id;
                membershipDisplayBlockModel.MemberList = RetrieveMemberList(membershipDisplayBlockModel, groupId, currentBlock.DisplayPageSize);
            }
            catch(SocialRepositoryException ex)
            {
                membershipDisplayBlockModel.Messages.Add(new MessageViewModel { Body = ex.Message, Type = "error" });
            }
            catch(NullReferenceException)
            {
                var errorMessage = "The group configured for this block cannot be found. Please update the block to use an existing group.";
                membershipDisplayBlockModel.Messages.Add(new MessageViewModel { Body = errorMessage, Type = "error" });
            }

            //Return block view with populated model
            return PartialView("~/Views/Social/MembershipDisplayBlock/Index.cshtml", membershipDisplayBlockModel);
        }

        //Retrieves a list of members to populate the view model with. 
        private List<SocialCompositeMember> RetrieveMemberList(MembershipDisplayBlockViewModel viewModel, GroupId groupId, int pageSize)
        {
            //Constructs a social member filter with groupId from the model and paging information that was configured in admin view
            var memberFilter = new SocialMemberFilter
            {
                GroupId = groupId,
                PageSize = pageSize
            };

            //Retrieves the list of members
            IEnumerable<SocialCompositeMember> memberList = memberRepository.Get(memberFilter);
           
            return ValidateMemberList(memberList);
        }

        //Validates the list of members
        private List<SocialCompositeMember> ValidateMemberList(IEnumerable<SocialCompositeMember> memberList)
        {
            return memberList != null && memberList.Any() ? memberList.ToList() : new List<SocialCompositeMember>();
        }
    }
}