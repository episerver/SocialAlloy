using EPiServer.Core;
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

            // Restore the saved model state
            LoadModelState(currentBlockLink);

            //populate model to pass to block view
            var groupCreationBlockModel = new GroupCreationBlockViewModel()
            {
                Heading = currentBlock.Heading,
                CurrentBlockLinkString = currentBlockLink.ToString(),
                PageId = pageRouteHelper.Page.ContentGuid.ToString(),
                CurrentPageLinkString = pageRouteHelper.PageLink.ToString()
            };

            // Apply current model state to the group block view model
            ApplyModelStateToGroupCreationBlockViewModel(groupCreationBlockModel);

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
            model.CurrentBlockLink = ContentReference.Parse(model.CurrentBlockLinkString);
            var data = this.contentRepository.Get<IContentData>(model.CurrentBlockLink);

            var validatedInputs = ValidateGroupInputs(model.Name, model.Description);
            model.SubmitErrorMessage = validatedInputs ? null : "Group name and description cannot be null or whitespace";
            model.CurrentPageLink = PageReference.Parse(model.CurrentPageLinkString);

            if (validatedInputs)
            {
                try
                {
                    var group = new Group(model.Name, model.Description);
                    this.groupRepository.Add(group);
                    model.SubmitSuccessMessage = "Your group: " + model.Name + " was added successfully!";
                }
                catch (SocialRepositoryException ex)
                {
                    model.SubmitErrorMessage = ex.Message;
                    ModelState.AddModelError("GroupCreation", ex.Message);
                }
            }
            else
            {
                // Flag the CommentBody model state with validation error
                ModelState.AddModelError("GroupCreation", model.SubmitErrorMessage);
            }

            SaveModelState(model.CurrentBlockLink, CollectViewModelStateToSave(model));

            return Redirect(UrlResolver.Current.GetUrl(model.CurrentPageLink));
        }

        private bool ValidateGroupInputs(string groupName, string groupDescription)
        {
            return !string.IsNullOrWhiteSpace(groupName) && !string.IsNullOrWhiteSpace(groupDescription);
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