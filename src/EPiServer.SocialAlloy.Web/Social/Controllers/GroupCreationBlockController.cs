﻿using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Social.Groups.Core;
using EPiServer.SocialAlloy.Web.Social.Blocks.Groups;
using EPiServer.SocialAlloy.Web.Social.Common.Controllers;
using EPiServer.SocialAlloy.Web.Social.Common.Exceptions;
using EPiServer.SocialAlloy.Web.Social.Common.Models;
using EPiServer.SocialAlloy.Web.Social.Models.Groups;
using EPiServer.SocialAlloy.Web.Social.Repositories;
using EPiServer.Web.Routing;
using System.Collections.Generic;
using System.Web.Mvc;

namespace EPiServer.SocialAlloy.Web.Social.Controllers
{
    /// <summary>
    /// The GroupCreationBlockController handles rendering the Group Creation block view for adding new groups
    /// </summary>
    public class GroupCreationBlockController : SocialBlockController<GroupCreationBlock>
    {
        private readonly ISocialGroupRepository groupRepository;

        public GroupCreationBlockController()
        {
            this.groupRepository = ServiceLocator.Current.GetInstance<ISocialGroupRepository>();
        }

        /// <summary>
        /// Render the GroupCreationBlock view.
        /// </summary>
        /// <param name="currentBlock">The current block instance.</param>
        public override ActionResult Index(GroupCreationBlock currentBlock)
        {
            var currentBlockLink = ((IContent)currentBlock).ContentLink;

            //Populate the model to pass to the block view
            var groupCreationBlockModel = new GroupCreationBlockViewModel()
            {
                Heading = currentBlock.Heading,
                ShowHeading = currentBlock.ShowHeading,
                CurrentBlockLink = currentBlockLink,
                CurrentPageLink = pageRouteHelper.PageLink,
                Messages = PopulateMessages(),
            };

            //Remove the existing values from the input fields
            ModelState.Clear();

            //Return the block view with populated model
            return PartialView("~/Views/Social/GroupCreationBlock/Index.cshtml", groupCreationBlockModel);
        }

        /// <summary>
        /// Submit handles the creation of new groups.  It accepts a GroupCreationBlockViewModel,
        /// stores the submitted group, and redirects back to the current page.
        /// </summary>
        /// <param name="model">The model submitted.</param>
        [HttpPost]
        public ActionResult Submit(GroupCreationBlockViewModel model)
        {
            AddGroup(model);
            return Redirect(UrlResolver.Current.GetUrl(model.CurrentPageLink));
        }

        /// <summary>
        /// Adss the group information to the underlying group repository
        /// </summary>
        /// <param name="model"></param>
        private void AddGroup(GroupCreationBlockViewModel model)
        {
            var validatedInputs = ValidateGroupInputs(model.Name, model.Description);
            if (validatedInputs)
            {
                try
                {
                    //Add the group and persist the group name in temp data to be used in the success message
                    var group = new Group(model.Name, model.Description);
                    this.groupRepository.Add(group);
                    var successMessage = "Your group: " + model.Name + " was added successfully!";
                    AddToTempData("GroupCreationSuccessMessage", successMessage);
                }
                catch (SocialRepositoryException ex)
                {
                    //Persist the exception message in temp data to be used in the error message
                    AddToTempData("GroupCreationErrorMessage", ex.Message);
                }
            }
            else
            {   //Persist the exception message in temp data to be used in the error message
                var errorMessage = "Group name and description cannot be empty";
                AddToTempData("GroupCreationErrorMessage", errorMessage);
            }
        }

        /// <summary>
        /// Populates the messages that will be displayed to the user in the group creation view.
        /// </summary>
        /// <returns>A list of messages used to convey statuses to the user</returns>
        private List<MessageViewModel> PopulateMessages()
        {
            var successMessageBody = GetFromTempData<string>("GroupCreationSuccessMessage");
            var successMessage = new MessageViewModel { Body = successMessageBody, Type = "success" };

            var errorMessageBody = GetFromTempData<string>("GroupCreationErrorMessage");
            var errorMessage = new MessageViewModel { Body = errorMessageBody, Type = "error" };

            return new List<MessageViewModel> { successMessage, errorMessage };
        }

        //Validates the group name and group description properties
        private bool ValidateGroupInputs(string groupName, string groupDescription)
        {
            return !string.IsNullOrWhiteSpace(groupName) && !string.IsNullOrWhiteSpace(groupDescription);
        }
    }
}