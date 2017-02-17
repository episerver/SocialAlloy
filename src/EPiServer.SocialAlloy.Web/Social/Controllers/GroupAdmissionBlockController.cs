using EPiServer.ServiceLocation;
using EPiServer.Social.Groups.Core;
using EPiServer.SocialAlloy.Web.Social.Blocks.Groups;
using EPiServer.SocialAlloy.Web.Social.Common.Controllers;
using EPiServer.SocialAlloy.Web.Social.Common.Exceptions;
using EPiServer.SocialAlloy.Web.Social.Common.Models;
using EPiServer.SocialAlloy.Web.Social.Models;
using EPiServer.SocialAlloy.Web.Social.Models.Groups;
using EPiServer.SocialAlloy.Web.Social.Repositories;
using EPiServer.Web.Routing;
using System;
using System.Web.Mvc;

namespace EPiServer.SocialAlloy.Web.Social.Controllers
{
    /// <summary>
    /// The GroupAdmissionBlockController handles rendering the Group Admission block view for adding new members to a group
    /// </summary>
    public class GroupAdmissionBlockController : SocialBlockController<GroupAdmissionBlock>
    {
        private readonly IUserRepository userRepository;
        private readonly ICommunityRepository communityRepository;
        private readonly ICommunityMemberRepository memberRepository;
        private readonly ICommunityMembershipModerationRepository moderationRepository;
        private const string MessageKey = "GroupAdmissionBlock";
        private const string ErrorMessage = "Error";
        private const string SuccessMessage = "Success";

        /// <summary>
        /// Constructor for admission block
        /// </summary>
        public GroupAdmissionBlockController()
        {
            userRepository = ServiceLocator.Current.GetInstance<IUserRepository>();
            communityRepository = ServiceLocator.Current.GetInstance<ICommunityRepository>();
            memberRepository = ServiceLocator.Current.GetInstance<ICommunityMemberRepository>();
            moderationRepository = ServiceLocator.Current.GetInstance<ICommunityMembershipModerationRepository>();
        }

        /// <summary>
        /// Render the Group Admission block view.
        /// </summary>
        /// <param name="currentBlock">The current block instance.</param>
        /// <returns></returns>
        public override ActionResult Index(GroupAdmissionBlock currentBlock)
        {
            var currentPageLink = pageRouteHelper.PageLink;

            //Populate model to pass to block view
            var blockModel = new GroupAdmissionBlockViewModel(currentBlock, currentPageLink);

            //Retrieves moderation information for the model to display in the view
            try
            {
                var group = communityRepository.Get(currentBlock.GroupName);
                ValidateGroup(blockModel, group);
                PopulateMemberDetails(blockModel);
            }
            catch (SocialRepositoryException ex)
            {
                AddMessage(MessageKey, new MessageViewModel(ex.Message, ErrorMessage));
            }
            catch (GroupDoesNotExistException ex)
            {
                AddMessage(MessageKey, new MessageViewModel(ex.Message, ErrorMessage));
            }
            blockModel.Messages = RetrieveMessages(MessageKey);
            //Remove existing values from input fields
            ModelState.Clear();

            //Return block view
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
                AddMember(blockModel);
            }
            catch (SocialRepositoryException ex)
            {
                AddMessage(MessageKey, new MessageViewModel(ex.Message, ErrorMessage));
            }

            return Redirect(UrlResolver.Current.GetUrl(blockModel.CurrentPageLink));
        }

        /// <summary>
        /// Determines how a member is added to a group. 
        /// If the group is moderated a request for membership is added into the group moderation workflow. 
        /// If the group is not moderated the member is  added to the underlying membership repository
        /// </summary>
        /// <param name="blockModel">The viewmodel for the GroupAdmission view</param>
        private void AddMember(GroupAdmissionBlockViewModel blockModel)
        {
            //Construct friendly names for messaging
            var userName = "";
            if (blockModel.UserIsLoggedIn)
            {
                var userId = userRepository.GetAuthenticatedId(blockModel.MemberName);
                userName = userRepository.GetUserName(userId);
            }
            else
            {
                userName = blockModel.MemberName;
                blockModel.MemberName = userRepository.CreateAnonymousUri(blockModel.MemberName);
            }

            if (ValidateMemberInputs(blockModel.MemberName, blockModel.MemberEmail))
            {
                try
                {
                    //Populated the CommunityMember and extension data
                    var member = new CommunityMember(blockModel.MemberName, blockModel.GroupId, blockModel.MemberEmail, blockModel.MemberCompany);
                    if (blockModel.IsModerated)
                    {
                        //Adds request for membership into moderation workflow
                        this.moderationRepository.AddAModeratedMember(member);
                    }
                    else
                    {
                        //Add the new member with extension data and persist the success message in temp data
                        memberRepository.Add(member);
                        var message = userName + " was added successfully to the group.";
                        AddMessage(MessageKey, new MessageViewModel(message, SuccessMessage));
                    }
                }
                catch (SocialRepositoryException ex)
                {
                    //Persist the exception message in temp data to be used in the error message
                    AddMessage(MessageKey, new MessageViewModel(ex.Message, ErrorMessage));
                }
            }
            else
            {
                //Persist the message in temp data to be used in the error message
                var message = "The member name, email and company cannot be empty.";
                AddMessage(MessageKey, new MessageViewModel(message, ErrorMessage));
            }
        }

        /// <summary>
        /// Validates the user name and user email
        /// </summary>
        /// <param name="userName">The username of the member</param>
        /// <param name="userEmail">Ther email of the  member</param>
        /// <returns>Returns bool for if the username and email are populated</returns>
        private bool ValidateMemberInputs(string userName, string userEmail)
        {
            return !string.IsNullOrWhiteSpace(userName) && !string.IsNullOrWhiteSpace(userEmail);
        }

        /// <summary>
        /// Validates that the group returned exists
        /// </summary>
        /// <param name="blockModel">The view model for the GroupAdmissionBlock</param>
        /// <param name="group">The group that was retrieved</param>
        private void ValidateGroup(GroupAdmissionBlockViewModel blockModel, Community group)
        {
            if (group != null)
            {
                var groupId = group.Id;
                blockModel.GroupName = group.Name;
                blockModel.GroupId = groupId.ToString();
                blockModel.IsModerated = moderationRepository.IsModerated(groupId);
            }
            else
            {
                var errorMessage = "The group configured for this block cannot be found. Please update the block to use an existing group.";
                AddMessage(MessageKey, new MessageViewModel(errorMessage, ErrorMessage));
            }
        }

        /// <summary>
        /// Populates the member related properties on the viewmodel
        /// </summary>
        /// <param name="blockModel">The view model for the GroupAdmissionBlock</param>
        private void PopulateMemberDetails(GroupAdmissionBlockViewModel blockModel)
        {
            var userId = userRepository.GetUserId(this.User);
            var loggedIn = !String.IsNullOrWhiteSpace(userId);
            blockModel.UserIsLoggedIn = loggedIn;
            blockModel.MemberName = loggedIn ? userRepository.CreateAuthenticatedUri(userId): "";
            blockModel.ModeratedUserAdmissionState = loggedIn ? moderationRepository.GetMembershipRequestState(blockModel.MemberName, blockModel.GroupId) : "";
        }
    }
}