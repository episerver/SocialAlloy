using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.SocialAlloy.Web.Social.Blocks;
using EPiServer.SocialAlloy.Web.Social.Common.Controllers;
using EPiServer.SocialAlloy.Web.Social.Common.Exceptions;
using EPiServer.SocialAlloy.Web.Social.Common.Models;
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
            var formViewModel = new CommentFormViewModel(this.pageRouteHelper.PageLink, currentBlockLink);

            // Create a comments block view model to fill the frontend block view
            var blockViewModel = new CommentsBlockViewModel(currentBlock, formViewModel);
            blockViewModel.Messages = PopulateMessages();

            // Try to get recent comments
            try
            {
                var socialComments = this.commentRepository.Get(
                    new SocialCommentFilter
                    {
                        PageSize = currentBlock.CommentsDisplayMax
                    }
                );

                blockViewModel.Comments = ValidateCommentList(socialComments);
            }
            catch (SocialRepositoryException ex)
            {
                blockViewModel.Messages.Add(new MessageViewModel { Body = ex.Message, Type = "error" });
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
                if (addedComment != null && currentBlock.SendActivity)
                {
                    AddCommentActivity(addedComment, blockViewModel);
                }
            }
            else
            {
                // Flag the CommentBody model state with validation error
                AddToTempData("CommentAddActivityErrorMessage", errors.First());
            }

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
                AddToTempData("CommentAddSuccessMessage", SubmitSuccessMessage);
            }
            catch (SocialRepositoryException ex)
            {
                AddToTempData("CommentAddErrorMessage", ex.Message);
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
                var commentActivity = new SocialCommentActivity { Body = comment.Body };

                this.activityRepository.Add(comment.AuthorId, comment.Target, commentActivity);
            }
            catch (SocialRepositoryException ex)
            {
                AddToTempData("CommentActivityAddErrorMessage", ex.Message);
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
        /// Populates the messages that will be displayed to the user in the group admission view.
        /// </summary>
        /// <returns>A list of messages used to convey statuses to the user</returns>
        private List<MessageViewModel> PopulateMessages()
        {
            var commentSuccessMessageBody = GetFromTempData<string>("CommentAddSuccessMessage");
            var commentSuccessMessage = new MessageViewModel { Body = commentSuccessMessageBody, Type = "success" };

            var commentAddErrorMessageBody = GetFromTempData<string>("CommentAddErrorMessage");
            var commentErrorMessage = new MessageViewModel { Body = commentAddErrorMessageBody, Type = "error" };

            var commentAddActivityErrorMessageBody = GetFromTempData<string>("CommentAddActivityErrorMessage");
            var commentAddActivityErrorMessage = new MessageViewModel { Body = commentAddActivityErrorMessageBody, Type = "error" };

            return new List<MessageViewModel> { commentSuccessMessage, commentErrorMessage, commentAddActivityErrorMessage };
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

        //Validates the list of members
        private IEnumerable<SocialComment> ValidateCommentList(IEnumerable<SocialComment> commentList)
        {
            return commentList != null && commentList.Any() ? commentList : new List<SocialComment>();
        }
    }
}
