using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Social.Comments.Core;
using EPiServer.SocialAlloy.Web.Social.Blocks;
using EPiServer.SocialAlloy.Web.Social.Common.Controllers;
using EPiServer.SocialAlloy.Web.Social.Models;
using EPiServer.Web.Routing;
using System.Globalization;
using System.Web.Mvc;

namespace EPiServer.SocialAlloy.Web.Social.Controllers
{
    /// <summary>
    /// The CommentsBlockController handles the rendering the comment block frontend view as well
    /// as the posting of new comments.
    /// </summary>
    public class CommentsBlockController : SocialBlockController<CommentsBlock>
    {
        /// <summary>
        /// Render the comment block frontend view.
        /// </summary>
        /// <param name="currentBlock">The current frontend block instance.</param>
        /// <returns></returns>
        public override ActionResult Index(CommentsBlock currentBlock)
        {
            var pageRouteHelper = ServiceLocator.Current.GetInstance<IPageRouteHelper>();
            var currentBlockLink = ((IContent)currentBlock).ContentLink;

            LoadModelState(currentBlockLink);

            var successMessage = GetSavedState("SuccessMessage");

            var errorMessage = GetSavedState("ErrorMessage");

            var commentBody = GetSavedState("Body");

            // Update the comment form view model with latest state.
            var commentForm = new CommentFormViewModel(pageRouteHelper.PageLink, currentBlockLink);
            if (commentBody != null)
            {
                commentForm.Body = commentBody.Value.AttemptedValue;
            }

            // Update the comment block view model with latest state.
            var commentBlockViewModel = new CommentsBlockViewModel(currentBlock, commentForm);
            if (successMessage != null)
            {
                commentBlockViewModel.SuccessMessage = successMessage.Value.AttemptedValue;
            }
            if (errorMessage != null)
            {
                commentBlockViewModel.ErrorMessage = errorMessage.Value.AttemptedValue;
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
            // TODO:  validate/store the comment here
            Comment addedComment = null;

            var contentRepository = ServiceLocator.Current.GetInstance<IContentRepository>();
            var data = contentRepository.Get<IContentData>(commentForm.CurrentBlockLink);

            var commentsViewModel = new CommentsBlockViewModel(data as CommentsBlock, commentForm);

            if (addedComment != null)
            {
                commentsViewModel.SuccessMessage = "Your comment was submitted successfully!";
            }
            else
            {
                commentsViewModel.ErrorMessage = "Failed to submit your comment!";
            }

            var transientState = GetTransientState(commentsViewModel);

            SaveModelState(commentForm.CurrentBlockLink, transientState);

            return Redirect(UrlResolver.Current.GetUrl(commentForm.CurrentPageLink));
        }

        /// <summary>
        /// Attempt to get a saved state given its key.
        /// </summary>
        /// <param name="key">The key of the state to get.</param>
        /// <returns></returns>
        private ModelState GetSavedState(string key)
        {
            ModelState value;
            ViewData.ModelState.TryGetValue(key, out value);
            return value;
        }

        /// <summary>
        /// Gets comment block view model state that is transient in nature and would be lost during redirection.
        /// </summary>
        /// <param name="commentsViewModel"></param>
        /// <returns></returns>
        private static ModelStateDictionary GetTransientState(CommentsBlockViewModel commentsViewModel)
        {
            return new ModelStateDictionary
            {
                new System.Collections.Generic.KeyValuePair<string, System.Web.Mvc.ModelState>
                (
                    "SuccessMessage",
                    new System.Web.Mvc.ModelState() {
                        Value = new ValueProviderResult(commentsViewModel.SuccessMessage, commentsViewModel.SuccessMessage, CultureInfo.CurrentCulture)
                    }
                ),
                new System.Collections.Generic.KeyValuePair<string, System.Web.Mvc.ModelState>
                (
                    "ErrorMessage",
                    new System.Web.Mvc.ModelState() {
                        Value = new ValueProviderResult(commentsViewModel.ErrorMessage, commentsViewModel.ErrorMessage, CultureInfo.CurrentCulture)
                    }
                )
            };
        }
    }
}
