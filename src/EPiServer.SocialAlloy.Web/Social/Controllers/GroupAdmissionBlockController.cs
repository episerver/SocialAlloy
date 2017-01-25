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
using EPiServer.Web.Routing;
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
        private readonly ISocialModerationRepository moderationRepository;
        private const string MessageKey = "GroupAdmissionBlock";
        private const string ErrorMessage = "Error";
        private const string SuccessMessage = "Success";

        /// <summary>
        /// Constructor for admission block
        /// </summary>
        public GroupAdmissionBlockController()
        {
            groupRepository = ServiceLocator.Current.GetInstance<ISocialGroupRepository>();
            memberRepository = ServiceLocator.Current.GetInstance<ISocialMemberRepository>();
            moderationRepository = ServiceLocator.Current.GetInstance<ISocialModerationRepository>();
        }

        /// <summary>
        /// Render the Group Admission block view.
        /// </summary>
        /// <param name="currentBlock">The current block instance.</param>
        /// <returns></returns>
        public override ActionResult Index(GroupAdmissionBlock currentBlock)
        {
            var currentBlockLink = ((IContent)currentBlock).ContentLink;

            //Populate model to pass to block view
            var blockModel = new GroupAdmissionBlockViewModel()
            {
                Heading = currentBlock.Heading,
                ShowHeading = currentBlock.ShowHeading,
                CurrentBlockLink = currentBlockLink,
                CurrentPageLink = pageRouteHelper.PageLink,
            };

            //Retrieves moderation information for the model to display in the view
            try
            {
                var group = groupRepository.Get(currentBlock.GroupName);
                //Validate that the group exists 
                if (group != null)
                {
                    var groupId = group.Id;
                    blockModel.GroupName = currentBlock.GroupName;
                    blockModel.GroupId = groupId.ToString();
                    blockModel.IsModerated = moderationRepository.IsModerated(groupId);
                }
                else
                {
                    var errorMessage = "The group configured for this block cannot be found. Please update the block to use an existing group.";
                    AddMessage(MessageKey, new MessageViewModel(errorMessage, ErrorMessage));
                }
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
            var validatedInputs = ValidateMemberInputs(blockModel.MemberName, blockModel.MemberEmail);
            if (validatedInputs)
            {
                try
                {
                    //Populated the SocialMember and extension data
                    var member = new SocialMember(blockModel.MemberName, blockModel.GroupId, blockModel.MemberEmail, blockModel.MemberCompany);
                    if (blockModel.IsModerated)
                    {
                        //Adds request for membership into moderation workflow
                        this.moderationRepository.AddAModeratedMember(member);
                    }
                    else
                    {
                        //Add the new member with extension data and persist the group name in temp data to be used in the success message
                        memberRepository.Add(member);
                        var message = blockModel.MemberName + " was added successfully to the group.";
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
    }
}