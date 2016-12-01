using EPiServer.SocialAlloy.Web.Social.Blocks.Groups;
using EPiServer.SocialAlloy.Web.Social.Common.Controllers;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.SocialAlloy.Web.Social.Blocks;
using EPiServer.SocialAlloy.Web.Social.Models;
using EPiServer.Web.Routing;
using System.Web.Mvc;
using EPiServer.SocialAlloy.Web.Social.Models.Groups;
using EPiServer.Social.Groups.Core;
using EPiServer.Web.Mvc;
using System;
using EPiServer.SocialAlloy.Web.Social.Common.Exceptions;
using System.Collections.Generic;
using System.Globalization;

namespace EPiServer.SocialAlloy.Web.Social.Controllers
{
    /// <summary>
    /// The CommentsBlockController handles the rendering the comment block frontend view as well
    /// as the posting of new comments.
    /// </summary>
    public class GroupCreationBlockController : SocialBlockController<GroupCreationBlock>
    {

        private readonly IGroupService groupService;
        private readonly IContentRepository contentRepository;

        private const string SubmitSuccessMessage = "SubmitSuccessMessage";
        private const string SubmitErrorMessage = "SubmitErrorMessage";
        

        public GroupCreationBlockController()
        {
            this.groupService = ServiceLocator.Current.GetInstance<IGroupService>();
            this.contentRepository = ServiceLocator.Current.GetInstance<IContentRepository>();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="groupService"></param>
        /// <param name="contentRepository"></param>
        public GroupCreationBlockController(IGroupService groupService, IContentRepository contentRepository)
        {
            this.groupService = groupService;
            this.contentRepository = contentRepository;
        }

        /// <summary>
        /// Render the comment block frontend view.
        /// </summary>
        /// <param name="currentBlock">The current frontend block instance.</param>
        /// <returns></returns>
        public override ActionResult Index(GroupCreationBlock currentBlock)
        {
            var pageRouteHelper = ServiceLocator.Current.GetInstance<IPageRouteHelper>();

            var currentBlockLink = ((IContent)currentBlock).ContentLink;

            // Restore the saved model state
            LoadModelState(currentBlockLink);

            var target = pageRouteHelper.Page.ContentGuid.ToString();

            //Populate the view model
            var groupCreationBlockModel = new GroupCreationBlockViewModel() { Heading = currentBlock.Heading, CurrentBlockLink = currentBlockLink, PageId = target, CurrentPageLink = pageRouteHelper.PageLink };

            // Apply current model state to the group block view model
            ApplyModelStateToGroupCreationBlockViewModel(groupCreationBlockModel);


            return PartialView("~/Views/Social/GroupCreationBlock/Index.cshtml", groupCreationBlockModel);
        }

        /// <summary>
        /// Submit handles the submitting of new groups.  It accepts a group creation form model,
        /// stores the submitted group, and redirects back to the current page.
        /// </summary>
        /// <param name="groupCreationForm">The group form being submitted.</param>
        /// <returns></returns>
        [HttpPost]
        //public ActionResult Submit(GroupCreationBlockModel model)
        public ActionResult Submit(string groupName, string groupDescription , string currentBlockLink , string pageId , string currentPageLink)
        {
            var cCurrentBlockLink = ContentReference.Parse(currentBlockLink);
            var data = this.contentRepository.Get<IContentData>(cCurrentBlockLink);

            var validatedInputs = IsValid(groupName, groupDescription);
            var groupModel = new GroupCreationBlockViewModel()
            {
                SubmitErrorMessage = validatedInputs ? null : "Group name and description cannot be null or whitespace",
                SubmitSuccessMessage = validatedInputs ? "Group name and description were added successfully" : null,
                CurrentBlockLink = cCurrentBlockLink,
                CurrentPageLink = PageReference.Parse(currentPageLink),
                PageId = pageId
            };

            if (validatedInputs)
            {
                try
                {
                    this.groupService.Add(new Group(groupName, groupDescription));
                    groupModel.SubmitSuccessMessage = "Your group was added successfully!";
                }
                catch (SocialRepositoryException ex)
                {
                    groupModel.SubmitErrorMessage = ex.Message;
                    ModelState.AddModelError("CommentBody", ex.Message);
                }
            }
            else
            {
                // Flag the CommentBody model state with validation error
                ModelState.AddModelError("CommentBody", groupModel.SubmitErrorMessage);
            }

            SaveModelState(cCurrentBlockLink, CollectViewModelStateToSave(groupModel));

            return Redirect(UrlResolver.Current.GetUrl(groupModel.CurrentPageLink));
        }
        private bool IsValid(string groupName, string groupDescription)
        {
            return !string.IsNullOrWhiteSpace(groupName) &&  !string.IsNullOrWhiteSpace(groupDescription);
        }

        /// <summary>
        /// Applies current model state to the group creation block view model.
        /// </summary>
        /// <param name="groupCreationBlockViewModel">The group block view model to apply model state to.</param>
        private void ApplyModelStateToGroupCreationBlockViewModel(GroupCreationBlockViewModel groupCreationBlockViewModel)
        {
            // Get success/error model state
            var successMessage = GetModelState(SubmitSuccessMessage);
            var errorMessage = GetModelState(SubmitErrorMessage);

            // Apply success/error model state to the view model
            groupCreationBlockViewModel.SubmitSuccessMessage = successMessage != null ? successMessage.Value.AttemptedValue : "";
            groupCreationBlockViewModel.SubmitErrorMessage = errorMessage != null ? errorMessage.Value.AttemptedValue : "";
        }

        /// <summary>
        /// Collects comment block view model state that needs to be saved.
        /// </summary>
        /// <param name="groupCreationViewModel">The comment block view model.</param>
        /// <returns>A model state dictionary.</returns>
        private ModelStateDictionary CollectViewModelStateToSave(GroupCreationBlockViewModel groupCreationViewModel)
        {
            var transientState = new ModelStateDictionary
            {
                new KeyValuePair<string, ModelState>
                (
                    SubmitSuccessMessage,
                    new ModelState() {
                        Value = new ValueProviderResult(groupCreationViewModel.SubmitSuccessMessage, groupCreationViewModel.SubmitSuccessMessage, CultureInfo.CurrentCulture)
                    }
                ),
                new KeyValuePair<string, ModelState>
                (
                    SubmitErrorMessage,
                    new ModelState() {
                        Value = new ValueProviderResult(groupCreationViewModel.SubmitErrorMessage, groupCreationViewModel.SubmitErrorMessage, CultureInfo.CurrentCulture)
                    }
                )
            };

            var modelState = ViewData.ModelState;
            if (transientState != null)
            {
                modelState.Merge(transientState);
            }

            return modelState;
        }
    }
}