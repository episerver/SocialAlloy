﻿using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Social.Groups.Core;
using EPiServer.SocialAlloy.Web.Social.Blocks.Groups;
using EPiServer.SocialAlloy.Web.Social.Common.Controllers;
using EPiServer.SocialAlloy.Web.Social.Common.Exceptions;
using EPiServer.SocialAlloy.Web.Social.Models;
using EPiServer.SocialAlloy.Web.Social.Models.Groups;
using EPiServer.SocialAlloy.Web.Social.Repositories;
using EPiServer.Web.Routing;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;

namespace EPiServer.SocialAlloy.Web.Social.Controllers
{
    /// <summary>
    /// The CommentsBlockController handles the rendering the comment block frontend view as well
    /// as the posting of new comments.
    /// </summary>
    public class GroupCreationBlockController : SocialBlockController<GroupCreationBlock>
    {
        private readonly ISocialGroupRepository groupRepository;
        private const string SubmitSuccessMessage = "SubmitSuccessMessage";
        private const string SubmitErrorMessage = "SubmitErrorMessage";

        public GroupCreationBlockController()
        {
            this.groupRepository = ServiceLocator.Current.GetInstance<ISocialGroupRepository>();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="groupService"></param>
        /// <param name="contentRepository"></param>
        public GroupCreationBlockController(ISocialGroupRepository groupRepository)
        {
            this.groupRepository = groupRepository;
        }

        /// <summary>
        /// Render the comment block frontend view.
        /// </summary>
        /// <param name="currentBlock">The current frontend block instance.</param>
        /// <returns></returns>
        public override ActionResult Index(GroupCreationBlock currentBlock)
        {
            var currentBlockLink = ((IContent)currentBlock).ContentLink;
            var successMessage = TempData["SubmitSuccessMessage"] == null ? null : TempData["SubmitSuccessMessage"].ToString();
            var errorMessage = TempData["SubmitErrorMessage"] == null ? null : TempData["SubmitErrorMessage"].ToString();

            //populate model to pass to block view
            var groupCreationBlockModel = new GroupCreationBlockViewModel()
            {
                Heading = currentBlock.Heading,
                CurrentBlockLink = currentBlockLink,
                PageId = pageRouteHelper.Page.ContentGuid.ToString(),
                CurrentPageLink = pageRouteHelper.PageLink,
                SubmitSuccessMessage = successMessage,
                SubmitErrorMessage = errorMessage
        };

            //remove existing values from input fields
            ModelState.Clear();

            //return block view
            return PartialView("~/Views/Social/GroupCreationBlock/Index.cshtml", groupCreationBlockModel);
        }

        /// <summary>
        /// Submit handles the submitting of new groups.  It accepts a group creation form model,
        /// stores the submitted group, and redirects back to the current page.
        /// </summary>
        /// <param name="groupCreationForm">The group form being submitted.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Submit(GroupCreationBlockViewModel model)
        {
            var data = this.contentRepository.Get<IContentData>(model.CurrentBlockLink);

            var validatedInputs = ValidateGroupInputs(model.Name, model.Description);
            TempData["SubmitErrorMessage"] = validatedInputs ? null : "Group name and description cannot be null or whitespace";

            if (validatedInputs)
            {
                try
                {
                    var group = new Group(model.Name, model.Description);
                    this.groupRepository.Add(group);
                    TempData["SubmitSuccessMessage"] = "Your group: " + model.Name + " was added successfully!";
                }
                catch (SocialRepositoryException ex)
                {
                    TempData["SubmitErrorMessage"] = ex.Message;
                }
            }

            return Redirect(UrlResolver.Current.GetUrl(model.CurrentPageLink));
        }

        private bool ValidateGroupInputs(string groupName, string groupDescription)
        {
            return !string.IsNullOrWhiteSpace(groupName) && !string.IsNullOrWhiteSpace(groupDescription);
        }
    }
}