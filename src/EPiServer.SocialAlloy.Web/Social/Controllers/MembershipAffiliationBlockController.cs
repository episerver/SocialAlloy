using EPiServer.ServiceLocation;
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
    public class MembershipAffiliationBlockController : SocialBlockController<MembershipAffiliationBlock>
    {
        private readonly IUserRepository userRepository;
        private readonly ICommunityRepository communityRepository;
        private readonly ICommunityMemberRepository memberRepository;
        private const string ErrorMessage = "Error";

        /// <summary>
        /// Constructor
        /// </summary>
        public MembershipAffiliationBlockController()
        {
            userRepository = ServiceLocator.Current.GetInstance<IUserRepository>();
            communityRepository = ServiceLocator.Current.GetInstance<ICommunityRepository>();
            memberRepository = ServiceLocator.Current.GetInstance<ICommunityMemberRepository>();
        }

        /// <summary>
        /// Render the membership display block view.
        /// </summary>
        /// <param name="currentBlock">The current block instance.</param>
        public override ActionResult Index(MembershipAffiliationBlock currentBlock)
        {
            //Populate model to pass to the membership affiliation view
            var membershipAffiliationBlockModel = new MembershipAffiliationBlockViewModel(currentBlock);

            try
            {
                //Retrieve the groups that are associated with the currently loogged in user.
                var userId = userRepository.GetUserId(this.User);
                if (!String.IsNullOrWhiteSpace(userId))
                {
                    var memberFilter = new CommunityMemberFilter
                    {
                        UserId = userRepository.CreateAuthenticatedUri(userId),
                        PageSize = currentBlock.DisplayPageSize
                    };
                    var listOfSocialMembers = this.memberRepository.Get(memberFilter);
                    GetAffiliatedGroups(membershipAffiliationBlockModel, listOfSocialMembers);
                }
                //If the user is not logged in let them know they will need to log in to see the groups they are affiliated with
                else
                {
                    var message = "Login to see the list of groups you are affiliated with.";
                    membershipAffiliationBlockModel.Messages.Add(new MessageViewModel(message, ErrorMessage));
                }
            }
            catch (SocialRepositoryException ex)
            {
                membershipAffiliationBlockModel.Messages.Add(new MessageViewModel(ex.Message, ErrorMessage));
            }

            //Return block view with populated model
            return PartialView("~/Views/Social/MembershipAffiliationBlock/Index.cshtml", membershipAffiliationBlockModel);
        }

        /// <summary>
        /// Populate the viewmodel with the list of social groups that a user is assoicated with
        /// </summary>
        /// <param name="membershipAffiliationBlockModel">The block viewmodel</param>
        /// <param name="listOfSocialMembers">The list of social members</param>
        private void GetAffiliatedGroups(MembershipAffiliationBlockViewModel membershipAffiliationBlockModel, IEnumerable<CommunityMember> listOfSocialMembers)
        {
            if (listOfSocialMembers != null && listOfSocialMembers.Any())
            {
                var listOfSocialGroups = this.communityRepository.Get(listOfSocialMembers.Select(x => x.GroupId).ToList());
                if (listOfSocialGroups != null && listOfSocialGroups.Any())
                {
                    membershipAffiliationBlockModel.Groups = listOfSocialGroups;
                }
            }
            else
            {
                var message = "You are not affiliated with any existing groups.";
                membershipAffiliationBlockModel.Messages.Add(new MessageViewModel(message, ErrorMessage));
            }
        }
    }
}