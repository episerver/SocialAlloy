using EPiServer.SocialAlloy.Web.Social.Blocks.Groups;
using EPiServer.SocialAlloy.Web.Social.Common.Controllers;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.SocialAlloy.Web.Social.Blocks;
using EPiServer.SocialAlloy.Web.Social.Models;
using EPiServer.Web.Routing;
using System.Web.Mvc;
using EPiServer.SocialAlloy.Web.Social.Models.Groups;

namespace EPiServer.SocialAlloy.Web.Social.Controllers
{
        /// <summary>
        /// The CommentsBlockController handles the rendering the comment block frontend view as well
        /// as the posting of new comments.
        /// </summary>
        public class GroupCreationBlockController : SocialBlockController<GroupCreationBlock>
        {
            /// <summary>
            /// Render the comment block frontend view.
            /// </summary>
            /// <param name="currentBlock">The current frontend block instance.</param>
            /// <returns></returns>
            public override ActionResult Index(GroupCreationBlock currentBlock)
            {
                var pageRouteHelper = ServiceLocator.Current.GetInstance<IPageRouteHelper>();
                var currentBlockLink = ((IContent)currentBlock).ContentLink;

                // Include some context in the comment form view rendering so that when new comments
                // are posted we have this context handy to associate with the comment.
                var commentBlockViewModel = new GroupCreationBlockModel(currentBlock,
                                                                       new GroupCreationFormModel(pageRouteHelper.PageLink, currentBlockLink));

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

                var contentRepository = ServiceLocator.Current.GetInstance<IContentRepository>();
                var data = contentRepository.Get<IContentData>(commentForm.CurrentBlockLink);

                var commentsViewModel = new CommentsBlockViewModel(data as CommentsBlock, commentForm);

                return Redirect(UrlResolver.Current.GetUrl(commentForm.CurrentPageLink));
            }
        }
    }