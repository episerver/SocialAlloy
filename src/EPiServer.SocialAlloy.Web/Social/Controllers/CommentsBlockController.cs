using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.SocialAlloy.Web.Social.Blocks;
using EPiServer.SocialAlloy.Web.Social.Common.Controllers;
using EPiServer.SocialAlloy.Web.Social.Common.Exceptions;
using EPiServer.SocialAlloy.Web.Social.Models;
using EPiServer.SocialAlloy.Web.Social.Repositories;
using EPiServer.Web.Routing;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace EPiServer.SocialAlloy.Web.Social.Controllers
{
    /// <summary>
    /// The CommentsBlockController handles the rendering the comment block frontend view as well
    /// as the posting of new comments.
    /// </summary>
    public class CommentsBlockController : SocialBlockController<CommentsBlock>
    {
        private readonly ISocialCommentRepository commentRepository;
        private readonly IContentRepository contentRepository;

        public CommentsBlockController()
        {
            this.commentRepository = ServiceLocator.Current.GetInstance<ISocialCommentRepository>();
            this.contentRepository = ServiceLocator.Current.GetInstance<IContentRepository>();
        }

        /// <summary>
        /// Render the comment block frontend view.
        /// </summary>
        /// <param name="currentBlock">The current frontend block instance.</param>
        /// <returns></returns>
        public override ActionResult Index(CommentsBlock currentBlock)
        {
            var pageRouteHelper = ServiceLocator.Current.GetInstance<IPageRouteHelper>();
            var currentBlockLink = ((IContent)currentBlock).ContentLink;

            // Restore the saved model state
            LoadModelState(currentBlockLink);

            // Get model state
            var successMessage = GetModelState("SuccessMessage");
            var errorMessage = GetModelState("ErrorMessage");
            var commentBody = GetModelState("Body");

            // Update the comment form view model with latest state data.
            var commentForm = new CommentFormViewModel(pageRouteHelper.PageLink, currentBlockLink);
            if (commentBody != null)
            {
                commentForm.Body = commentBody.Value.AttemptedValue;
            }

            // Create a comments block view model
            var commentBlockViewModel = new CommentsBlockViewModel(currentBlock, commentForm);

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
                commentBlockViewModel.SubmitSuccessMessage = successMessage != null ? successMessage.Value.AttemptedValue : "";
            }
            catch (SocialRepositoryException ex)
            {
                commentBlockViewModel.SubmitErrorMessage = errorMessage != null ? errorMessage.Value.AttemptedValue : "";
                commentBlockViewModel.DisplayErrorMessage = ex.Message;
            }

            return PartialView("~/Views/Social/CommentsBlock/CommentsView.cshtml", commentBlockViewModel);
        }

        /// <summary>
        /// Submit handles the submitting of new comments.  It accepts a comment form model,
        /// validates the form, stores the submitted comment, and redirects back to the current page.
        /// </summary>
        /// <param name="commentForm">The comment form being submitted.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Submit(CommentFormViewModel commentForm)
        {
            var data = this.contentRepository.Get<IContentData>(commentForm.CurrentBlockLink);

            var commentsViewModel = new CommentsBlockViewModel(data as CommentsBlock, commentForm);

            var errors = ValidateBody(commentForm);

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
                commentsViewModel.SubmitErrorMessage = errors.First();
            }

            SaveModelState(commentForm.CurrentBlockLink, CollectViewModelStateToSave(commentsViewModel));

            return Redirect(UrlResolver.Current.GetUrl(commentForm.CurrentPageLink));
        }

        /// <summary>
        /// Adapts the comment form to a social comment model.
        /// </summary>
        /// <param name="commentForm"></param>
        /// <returns>A comment.</returns>
        private SocialComment AdaptCommentFormViewModelToSocialComment(CommentFormViewModel commentForm)
        {
            return new SocialComment
            {
                Target = commentForm.CurrentPageLink.ToString(),
                Body = commentForm.Body,
                Author = this.User.Identity.Name
            };
        }

        /// <summary>
        /// Validates the body in the comment form.
        /// </summary>
        /// <param name="commentForm"></param>
        /// <returns>Returns a list of validation errors.</returns>
        private List<string> ValidateBody(CommentFormViewModel commentForm)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(commentForm.Body))
            {
                errors.Add("Cannot add an empty comment.");
            }

            return errors;
        }

        /// <summary>
        /// Collects view model state that needs to be saved.
        /// </summary>
        /// <param name="commentsViewModel"></param>
        /// <returns></returns>
        private ModelStateDictionary CollectViewModelStateToSave(CommentsBlockViewModel commentsViewModel)
        {
            var transientState = new ModelStateDictionary
            {
                new KeyValuePair<string, ModelState>
                (
                    "SuccessMessage",
                    new ModelState() {
                        Value = new ValueProviderResult(commentsViewModel.SubmitSuccessMessage, commentsViewModel.SubmitSuccessMessage, CultureInfo.CurrentCulture)
                    }
                ),
                new KeyValuePair<string, ModelState>
                (
                    "ErrorMessage",
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
    }
}
