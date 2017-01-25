using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Social.Groups.Core;
using EPiServer.SocialAlloy.Web.Social.Blocks.Groups;
using EPiServer.SocialAlloy.Web.Social.Common.Controllers;
using EPiServer.SocialAlloy.Web.Social.Common.Exceptions;
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
        private const string ErrorMessage = "Error";

        /// <summary>
        /// Constructor
        /// </summary>
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
                Members = new List<SocialMember>()
            };

            //Retrieve the group id assigned to the block and populate the memberlist 
            try
            {
                var group = groupRepository.Get(currentBlock.GroupName);

                //Validate that the group exists 
                if (group != null)
                {
                    var groupId = group.Id;
                    var memberFilter = new SocialMemberFilter
                    {
                        GroupId = groupId,
                        PageSize = currentBlock.DisplayPageSize
                    };
                    membershipDisplayBlockModel.Members = memberRepository.Get(memberFilter).ToList();
                }
                else
                {
                    var message = "The group configured for this block cannot be found. Please update the block to use an existing group.";
                    membershipDisplayBlockModel.Messages.Add(new MessageViewModel(message, ErrorMessage));
                }
            }
            catch (SocialRepositoryException ex)
            {
                membershipDisplayBlockModel.Messages.Add(new MessageViewModel(ex.Message, ErrorMessage));
            }
            catch (GroupDoesNotExistException ex)
            {
                membershipDisplayBlockModel.Messages.Add(new MessageViewModel(ex.Message, ErrorMessage));
            }

            //Return block view with populated model
            return PartialView("~/Views/Social/MembershipDisplayBlock/Index.cshtml", membershipDisplayBlockModel);
        }
    }
}