using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.SocialAlloy.Web.Social.Blocks;
using EPiServer.SocialAlloy.Web.Social.Common.Controllers;
using EPiServer.SocialAlloy.Web.Social.Common.Exceptions;
using EPiServer.SocialAlloy.Web.Social.Models;
using EPiServer.SocialAlloy.Web.Social.Repositories;
using EPiServer.Web.Routing;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace EPiServer.SocialAlloy.Web.Social.Controllers
{
    /// <summary>
    /// The CommentsBlockController handles the rendering of the comment block frontend view as well
    /// as the posting of new comments.
    /// </summary>
    public class CommentsBlockController : SocialBlockController<CommentsBlock>
    {
        private readonly IUserRepository userRepository;
        private readonly ISocialCommentRepository commentRepository;
        private readonly IPageRepository pageRepository;
        private readonly ISocialActivityRepository activityRepository;

        private const string ModelState_SubmitSuccessMessage = "SubmitSuccessMessage";
        private const string ModelState_SubmitErrorMessage = "SubmitErrorMessage";
        private const string SubmitSuccessMessage = "Your comment was submitted successfully!";
        private const string BodyValidationErrorMessage = "Cannot add an empty comment.";

        /// <summary>
        /// Constructor
        /// </summary>
        public CommentsBlockController()
        {
            this.userRepository = ServiceLocator.Current.GetInstance<IUserRepository>();
            this.commentRepository = ServiceLocator.Current.GetInstance<ISocialCommentRepository>();
            this.pageRepository = ServiceLocator.Current.GetInstance<IPageRepository>();
            this.activityRepository = ServiceLocator.Current.GetInstance<ISocialActivityRepository>();
        }

        /// <summary>
        /// Render the comment block frontend view.
        /// </summary>
        /// <param name="currentBlock">The current frontend block instance.</param>
        /// <returns>The action's result.</returns>
        public override ActionResult Index(CommentsBlock currentBlock)
        {
            var currentBlockLink = ((IContent)currentBlock).ContentLink;

            // Restore the saved model state
            LoadModelState(currentBlockLink);

            var formViewModel = new CommentFormViewModel(this.pageRouteHelper.PageLink, currentBlockLink);

            // Create a comments block view model to fill the frontend block view
            var blockViewModel = new CommentsBlockViewModel(currentBlock, formViewModel);

            // Apply current model state to the comment block view model
            ApplyModelStateToCommentBlockViewModel(blockViewModel);

            // Try to get recent comments
            IEnumerable<SocialComment> recentComments = new List<SocialComment>();
            try
            {
                blockViewModel.Comments = this.commentRepository.Get(
                    new SocialCommentFilter
                    {
                        PageSize = currentBlock.CommentsDisplayMax
                    }
                );
            }
            catch (SocialRepositoryException ex)
            {
                blockViewModel.DisplayErrorMessage = ex.Message;
            }

            return PartialView("~/Views/Social/CommentsBlock/CommentsView.cshtml", blockViewModel);
        }

        /// <summary>
        /// Submit handles the submitting of new comments.  It accepts a comment form model,
        /// validates the form, stores the submitted comment, sends a new activity if configuration
        /// allows, and redirects back to the current page.
        /// </summary>
        /// <param name="formViewModel">The comment form being submitted.</param>
        /// <returns>The submit action result.</returns>
        [HttpPost]
        public ActionResult Submit(CommentFormViewModel formViewModel)
        {
            var currentBlock = this.contentRepository.Get<IContentData>(formViewModel.CurrentBlockLink) as CommentsBlock;

            var blockViewModel = new CommentsBlockViewModel(currentBlock, formViewModel);

            var errors = ValidateCommentForm(formViewModel);

            if (errors.Count() == 0)
            {
                var addedComment = AddComment(formViewModel, blockViewModel);
                if (BlockModelHasNoErrors(blockViewModel) && currentBlock.SendActivity)
                {
                    AddCommentActivity(addedComment, blockViewModel);
                }
            }
            else
            {
                // Flag the CommentBody model state with validation error
                ModelState.AddModelError("CommentBody", errors.First());
            }

            SaveModelState(formViewModel.CurrentBlockLink, CollectViewModelStateToSave(blockViewModel));

            return Redirect(UrlResolver.Current.GetUrl(formViewModel.CurrentPageLink));
        }

        /// <summary>
        /// Adds the comment in the CommentFormViewModel to the Episerver Social repository.
        /// </summary>
        /// <param name="formViewModel">The submitted comment form view model.</param>
        /// <param name="blockViewModel">The comments block view model.</param>
        /// <returns>The added SocialComment</returns>
        private SocialComment AddComment(CommentFormViewModel formViewModel, CommentsBlockViewModel blockViewModel)
        {
            var newComment = this.AdaptCommentFormViewModelToSocialComment(formViewModel);
            SocialComment addedComment = null;

            try
            {
                addedComment = this.commentRepository.Add(newComment);
                blockViewModel.SubmitSuccessMessage = SubmitSuccessMessage;
            }
            catch (SocialRepositoryException ex)
            {
                blockViewModel.SubmitErrorMessage = ex.Message;
            }

            return addedComment;
        }

        /// <summary>
        /// Add an activity for the newly added comment.
        /// </summary>
        /// <param name="comment">The added comment.</param>
        private void AddCommentActivity(SocialComment comment, CommentsBlockViewModel blockViewModel)
        {
            try
            {
                var commentActivity = new SocialCommentActivity
                {
                    Body = comment.Body
                };

                this.activityRepository.Add(comment.AuthorId, comment.Target, commentActivity);
            }
            catch (SocialRepositoryException ex)
            {
                blockViewModel.SubmitErrorMessage = ex.Message;
            }
        }

        /// <summary>
        /// Adapts the comment form to a social comment model.
        /// </summary>
        /// <param name="formViewModel">The comment form view model.</param>
        /// <returns>A social comment.</returns>
        private SocialComment AdaptCommentFormViewModelToSocialComment(CommentFormViewModel formViewModel)
        {
            return new SocialComment
            {
                Target = this.pageRepository.GetPageId(formViewModel.CurrentPageLink),
                Body = formViewModel.Body,
                AuthorId = this.userRepository.GetUserId(this.User)
            };
        }

        /// <summary>
        /// Validates the comment form.
        /// </summary>
        /// <param name="formViewModel">The comment form view model.</param>
        /// <returns>Returns a list of validation errors.</returns>
        private List<string> ValidateCommentForm(CommentFormViewModel formViewModel)
        {
            var errors = new List<string>();

            // Make sure the comment body has some text
            if (string.IsNullOrWhiteSpace(formViewModel.Body))
            {
                errors.Add(BodyValidationErrorMessage);
            }

            return errors;
        }

        /// <summary>
        /// Collects comment block view model state that needs to be saved.
        /// </summary>
        /// <param name="blockViewModel">The comment block view model.</param>
        /// <returns>A model state dictionary.</returns>
        private ModelStateDictionary CollectViewModelStateToSave(CommentsBlockViewModel blockViewModel)
        {
            var transientState = new ModelStateDictionary
            {
                new KeyValuePair<string, ModelState>
                (
                    ModelState_SubmitSuccessMessage,
                    new ModelState() {
                        Value = new ValueProviderResult(blockViewModel.SubmitSuccessMessage, null, CultureInfo.CurrentCulture)
                    }
                ),
                new KeyValuePair<string, ModelState>
                (
                    ModelState_SubmitErrorMessage,
                    new ModelState() {
                        Value = new ValueProviderResult(blockViewModel.SubmitErrorMessage, null, CultureInfo.CurrentCulture)
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

        /// <summary>
        /// Applies current model state to the comment block view model.
        /// </summary>
        /// <param name="blockViewModel">The comment block view model to apply model state to.</param>
        private void ApplyModelStateToCommentBlockViewModel(CommentsBlockViewModel blockViewModel)
        {
            // Get success/error model state
            var successMessage = GetModelState(ModelState_SubmitSuccessMessage);
            var errorMessage = GetModelState(ModelState_SubmitErrorMessage);

            // Apply success/error model state to the view model
            blockViewModel.SubmitSuccessMessage = successMessage != null ? (string)successMessage.Value.RawValue : "";
            blockViewModel.SubmitErrorMessage = errorMessage != null ? (string)errorMessage.Value.RawValue : "";

            // If there was an error submitting the message then leave the current body in the comment box
            // so the user does not have to retype it.
            if (errorMessage != null && !String.IsNullOrWhiteSpace(errorMessage.Value.AttemptedValue))
            {
                var commentBody = GetModelState("Body");
                if (commentBody != null)
                {
                    blockViewModel.CommentBody = commentBody.Value.AttemptedValue;
                }
            }
        }

        /// <summary>
        /// Returns true if the block model contains no errors that were encountered while 
        /// processing the comment that was posted.
        /// </summary>
        /// <param name="blockModel">a reference to the CommentsBlockViewModel</param>
        /// <returns>Returns true if no errors were encountered while processing the comment that was posted, 
        /// false otherwise.</returns>
        private static bool BlockModelHasNoErrors(CommentsBlockViewModel blockModel)
        {
            return String.IsNullOrEmpty(blockModel.SubmitErrorMessage);
        }
    }
}
