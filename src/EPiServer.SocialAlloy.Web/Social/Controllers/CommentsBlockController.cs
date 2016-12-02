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

        private const string SubmitSuccessMessage = "SubmitSuccessMessage";
        private const string SubmitErrorMessage = "SubmitErrorMessage";

        /// <summary>
        /// Constructor
        /// </summary>
        public CommentsBlockController()
        {
            this.userRepository = ServiceLocator.Current.GetInstance<IUserRepository>();
            this.commentRepository = ServiceLocator.Current.GetInstance<ISocialCommentRepository>();
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

            var commentForm = new CommentFormViewModel(this.pageRouteHelper.PageLink, currentBlockLink);

            // Create a comments block view model to fill the frontend block view
            var commentBlockViewModel = new CommentsBlockViewModel(currentBlock, commentForm);

            // Apply current model state to the comment block view model
            ApplyModelStateToCommentBlockViewModel(commentBlockViewModel);

            // Try to get recent comments
            IEnumerable<SocialComment> recentComments = new List<SocialComment>();
            try
            {
                commentBlockViewModel.Comments = this.commentRepository.Get(
                    new SocialCommentFilter
                    {
                        PageSize = currentBlock.CommentsDisplayMax
                    }
                );
            }
            catch (SocialRepositoryException ex)
            {
                commentBlockViewModel.DisplayErrorMessage = ex.Message;
            }

            return PartialView("~/Views/Social/CommentsBlock/CommentsView.cshtml", commentBlockViewModel);
        }

        /// <summary>
        /// Submit handles the submitting of new comments.  It accepts a comment form model,
        /// validates the form, stores the submitted comment, and redirects back to the current page.
        /// </summary>
        /// <param name="commentForm">The comment form being submitted.</param>
        /// <returns>The submit action result.</returns>
        [HttpPost]
        public ActionResult Submit(CommentFormViewModel commentForm)
        {
            var data = this.contentRepository.Get<IContentData>(commentForm.CurrentBlockLink);

            var commentsViewModel = new CommentsBlockViewModel(data as CommentsBlock, commentForm);

            var errors = ValidateCommentForm(commentForm);

            if (errors.Count() == 0)
            {
                var comment = this.AdaptCommentFormViewModelToSocialComment(commentForm);
                try
                {
                    this.commentRepository.Add(comment);
                    commentsViewModel.SubmitSuccessMessage = "Your comment was submitted successfully!";
                }
                catch (SocialRepositoryException ex)
                {
                    commentsViewModel.SubmitErrorMessage = ex.Message;
                }
            }
            else
            {
                // Flag the CommentBody model state with validation error
                ModelState.AddModelError("CommentBody", errors.First());
            }

            SaveModelState(commentForm.CurrentBlockLink, CollectViewModelStateToSave(commentsViewModel));

            return Redirect(UrlResolver.Current.GetUrl(commentForm.CurrentPageLink));
        }

        /// <summary>
        /// Adapts the comment form to a social comment model.
        /// </summary>
        /// <param name="commentForm">The comment form view model.</param>
        /// <returns>A social comment.</returns>
        private SocialComment AdaptCommentFormViewModelToSocialComment(CommentFormViewModel commentForm)
        {
            return new SocialComment
            {
                Target = GetPageId(commentForm.CurrentPageLink),
                Body = commentForm.Body,
                Author = this.userRepository.GetUserId(this.User)
            };
        }

        /// <summary>
        /// Validates the comment form.
        /// </summary>
        /// <param name="commentForm">The comment form view model.</param>
        /// <returns>Returns a list of validation errors.</returns>
        private List<string> ValidateCommentForm(CommentFormViewModel commentForm)
        {
            var errors = new List<string>();

            // Make sure the comment body has some text
            if (string.IsNullOrWhiteSpace(commentForm.Body))
            {
                errors.Add("Cannot add an empty comment.");
            }

            return errors;
        }

        /// <summary>
        /// Collects comment block view model state that needs to be saved.
        /// </summary>
        /// <param name="commentsViewModel">The comment block view model.</param>
        /// <returns>A model state dictionary.</returns>
        private ModelStateDictionary CollectViewModelStateToSave(CommentsBlockViewModel commentsViewModel)
        {
            var transientState = new ModelStateDictionary
            {
                new KeyValuePair<string, ModelState>
                (
                    SubmitSuccessMessage,
                    new ModelState() {
                        Value = new ValueProviderResult(commentsViewModel.SubmitSuccessMessage, commentsViewModel.SubmitSuccessMessage, CultureInfo.CurrentCulture)
                    }
                ),
                new KeyValuePair<string, ModelState>
                (
                    SubmitErrorMessage,
                    new ModelState() {
                        Value = new ValueProviderResult(commentsViewModel.SubmitErrorMessage, commentsViewModel.SubmitErrorMessage, CultureInfo.CurrentCulture)
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
        /// <param name="commentBlockViewModel">The comment block view model to apply model state to.</param>
        private void ApplyModelStateToCommentBlockViewModel(CommentsBlockViewModel commentBlockViewModel)
        {
            // Get success/error model state
            var successMessage = GetModelState(SubmitSuccessMessage);
            var errorMessage = GetModelState(SubmitErrorMessage);

            // Apply success/error model state to the view model
            commentBlockViewModel.SubmitSuccessMessage = successMessage != null ? successMessage.Value.AttemptedValue : "";
            commentBlockViewModel.SubmitErrorMessage = errorMessage != null ? errorMessage.Value.AttemptedValue : "";

            // If there was an error submitting the message then leave the current body in the comment box
            // so the user does not have to retype it.
            if (errorMessage != null && !String.IsNullOrWhiteSpace(errorMessage.Value.AttemptedValue))
            {
                var commentBody = GetModelState("Body");
                if (commentBody != null)
                {
                    commentBlockViewModel.CommentBody = commentBody.Value.AttemptedValue;
                }
            }
        }
    }
}
