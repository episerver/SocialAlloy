using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Social.Comments.Core;
using EPiServer.Social.Common;
using EPiServer.SocialAlloy.Web.Social.Blocks;
using EPiServer.SocialAlloy.Web.Social.Common.Controllers;
using EPiServer.SocialAlloy.Web.Social.Models;
using EPiServer.Web.Routing;
using System;
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
        private readonly ICommentService commentService;
        private readonly IContentRepository contentRepository;

        public CommentsBlockController()
        {
            this.commentService = ServiceLocator.Current.GetInstance<ICommentService>();
            this.contentRepository = ServiceLocator.Current.GetInstance<IContentRepository>();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="commentService"></param>
        /// <param name="contentRepository"></param>
        public CommentsBlockController(ICommentService commentService, IContentRepository contentRepository)
        {
            this.commentService = commentService;
            this.contentRepository = contentRepository;
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

            List<Comment> recentComments;

            var getCommentErrors = TryGetComments(currentBlock.CommentsDisplayMax, out recentComments);

            // Update the comment block view model with latest state data.
            var commentBlockViewModel = new CommentsBlockViewModel(currentBlock, commentForm);
            if (successMessage != null)
            {
                commentBlockViewModel.SubmitSuccessMessage = successMessage.Value.AttemptedValue;
            }
            if (errorMessage != null)
            {
                commentBlockViewModel.SubmitErrorMessage = errorMessage.Value.AttemptedValue;
            }
            if (getCommentErrors.Count() == 0)
            {
                commentBlockViewModel.Comments = recentComments;
            }
            else
            {
                commentBlockViewModel.DisplayErrorMessage = getCommentErrors.First();
            }

            return PartialView("~/Views/Social/CommentsBlock/CommentsView.cshtml", commentBlockViewModel);
        }

        /// <summary>
        /// Submit handles the submitting of new comments.  It accepts a comment form model,
        /// stores the submitted comment, and redirects back to the current page.
        /// </summary>
        /// <param name="commentForm">The comment form being submitted.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Submit(CommentFormViewModel commentForm)
        {
            var data = this.contentRepository.Get<IContentData>(commentForm.CurrentBlockLink);

            var commentsViewModel = new CommentsBlockViewModel(data as CommentsBlock, commentForm);

            var comment = this.BuildComment(commentForm);

            if (comment != null)
            {
                var errors = TryAddComment(comment);
                if (errors.Count() > 0)
                {
                    commentsViewModel.SubmitErrorMessage = errors.First();
                }
                else
                {
                    commentsViewModel.SubmitSuccessMessage = "Your comment was submitted successfully!";
                }
            }
            else
            {
                commentsViewModel.SubmitErrorMessage = "Your comment is not valid!";
            }

            SaveModelState(commentForm.CurrentBlockLink, CollectViewModelStateToSave(commentsViewModel));

            return Redirect(UrlResolver.Current.GetUrl(commentForm.CurrentPageLink));
        }

        /// <summary>
        /// Try to add the comment via the social comment service.
        /// </summary>
        /// <param name="comment">The comment to add.</param>
        /// <returns>A list of any errors encountered.</returns>
        private IEnumerable<string> TryAddComment(Comment comment)
        {
            var errors = new List<string>();

            try
            {
                this.commentService.Add(comment);
            }
            catch (ArgumentNullException ex)
            {
                errors.Add("ArgumentNullException: " + ex.Message);
            }
            catch (ArgumentException ex)
            {
                errors.Add("ArgumentException: " + ex.Message);
            }
            catch (InvalidCommentException ex)
            {
                errors.Add("InvalidCommentException: " + ex.Message);
            }
            catch (SocialAuthenticationException ex)
            {
                errors.Add("SocialAuthenticationException: " + ex.Message);
            }
            catch (MaximumDataSizeExceededException ex)
            {
                errors.Add("MaximumDataSizeExceededException: " + ex.Message);
            }
            catch (SocialCommunicationException ex)
            {
                errors.Add("SocialCommunicationException: " + ex.Message);
            }
            catch (SocialException ex)
            {
                errors.Add("SocialException: " + ex.Message);
            }
            catch (Exception ex)
            {
                errors.Add("Exception: " + ex.Message);
            }

            return errors;
        }

        /// <summary>
        /// Attempt to get recent comments via the social comment service.
        /// </summary>
        /// <param name="comment">The comment to add.</param>
        /// <param name="comments">The retrieved comments</param>
        /// <returns>A list of any errors encountered.</returns>
        private List<string> TryGetComments(int commentDisplayMax, out List<Comment> comments)
        {
            var errors = new List<string>();
            comments = new List<Comment>();

            try
            {
                comments = this.commentService.Get(
                    new Criteria<CommentFilter>
                    {
                        PageInfo = new PageInfo
                        {
                            PageSize = commentDisplayMax
                        }
                    }
                ).Results.ToList();
            }
            catch (ArgumentNullException ex)
            {
                errors.Add("ArgumentNullException: " + ex.Message);
            }
            catch (ArgumentException ex)
            {
                errors.Add("ArgumentException: " + ex.Message);
            }
            catch (InvalidCommentException ex)
            {
                errors.Add("InvalidCommentException: " + ex.Message);
            }
            catch (SocialAuthenticationException ex)
            {
                errors.Add("SocialAuthenticationException: " + ex.Message);
            }
            catch (MaximumDataSizeExceededException ex)
            {
                errors.Add("MaximumDataSizeExceededException: " + ex.Message);
            }
            catch (SocialCommunicationException ex)
            {
                errors.Add("SocialCommunicationException: " + ex.Message);
            }
            catch (SocialException ex)
            {
                errors.Add("SocialException: " + ex.Message);
            }
            catch (Exception ex)
            {
                errors.Add("Exception: " + ex.Message);
            }

            return errors;
        }

        /// <summary>
        /// Build the comment from the comment form.
        /// </summary>
        /// <param name="commentForm"></param>
        /// <returns>A comment.</returns>
        private Comment BuildComment(CommentFormViewModel commentForm)
        {
            Comment comment = null;

            if (ValidBody(commentForm))
            {
                comment = new Comment(Reference.Create(commentForm.CurrentPageLink.ToString()), commentForm.Body, true);
            }

            return comment;
        }

        /// <summary>
        /// Validates the body in the comment form.
        /// </summary>
        /// <param name="commentForm"></param>
        /// <returns>Whether the comment body is valid.</returns>
        private static bool ValidBody(CommentFormViewModel commentForm)
        {
            return !string.IsNullOrWhiteSpace(commentForm.Body);
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
                new System.Collections.Generic.KeyValuePair<string, System.Web.Mvc.ModelState>
                (
                    "SuccessMessage",
                    new System.Web.Mvc.ModelState() {
                        Value = new ValueProviderResult(commentsViewModel.SubmitSuccessMessage, commentsViewModel.SubmitSuccessMessage, CultureInfo.CurrentCulture)
                    }
                ),
                new System.Collections.Generic.KeyValuePair<string, System.Web.Mvc.ModelState>
                (
                    "ErrorMessage",
                    new System.Web.Mvc.ModelState() {
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
