using EPiServer.Social.Comments.Core;
using EPiServer.SocialAlloy.Web.Social.Blocks;
using EPiServer.SocialAlloy.Web.Social.Common.Models;
using System.Collections.Generic;

namespace EPiServer.SocialAlloy.Web.Social.Models
{
    /// <summary>
    /// The CommentsBlockViewModel class represents the model that will be used to
    /// feed data to the comments block frontend view.
    /// </summary>
    public class CommentsBlockViewModel : SocialBlockViewModel<CommentsBlock>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="block"></param>
        /// <param name="form"></param>
        public CommentsBlockViewModel(CommentsBlock block, 
                                      CommentFormViewModel form)
            : base(form.CurrentPageLink, form.CurrentBlockLink)
        {
            Heading = block.Heading;
            CommentBoxRows = block.CommentBoxRows;
            CommentMaxLength = block.CommentMaxLength;
            CommentsDisplayMax = block.CommentsDisplayMax;
            CommentAuthor = form.Author;
            CommentBody = form.Body;
        }

        /// <summary>
        /// The heading for the frontend comments block display.
        /// </summary>
        public string Heading { get; }

        /// <summary>
        /// The number of rows in the comment box.
        /// </summary>
        public int CommentBoxRows { get; }

        /// <summary>
        /// The max length of a new comment.
        /// </summary>
        public int CommentMaxLength { get; }

        /// <summary>
        /// The max length of a new comment.
        /// </summary>
        public int CommentsDisplayMax { get; }

        /// <summary>
        /// The comments to show.
        /// </summary>
        public List<Comment> Comments { get; set; }

        /// <summary>
        /// A success message that should be flashed in the view.
        /// </summary>
        public string SubmitSuccessMessage { get; set; }

        /// <summary>
        /// A error message that should be flashed in the view.
        /// </summary>
        public string SubmitErrorMessage { get; set; }

        /// <summary>
        /// A error message that should be flashed in the message display view.
        /// </summary>
        public string DisplayErrorMessage { get; set; }

        /// <summary>
        /// Gets the username for the user who may currently submit comments.
        /// </summary>
        public string CommentAuthor { get; }

        /// <summary>
        /// Gets the last submitted comment body.
        /// </summary>
        public string CommentBody { get; }
    }
}