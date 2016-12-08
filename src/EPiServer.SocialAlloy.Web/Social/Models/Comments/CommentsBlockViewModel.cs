using EPiServer.SocialAlloy.Web.Social.Blocks;
using EPiServer.SocialAlloy.Web.Social.Common.Models;
using System.Collections.Generic;

namespace EPiServer.SocialAlloy.Web.Social.Models
{
    /// <summary>
    /// The CommentsBlockViewModel class represents the model that will be used to
    /// feed data to the comments block frontend view.
    /// </summary>
    public class CommentsBlockViewModel : SocialBlockViewModel
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="block">A block reference to use as a key under which to save the model state.</param>
        /// <param name="form">A comment form view model to get current form values for the block view model</param>
        public CommentsBlockViewModel(CommentsBlock block, CommentFormViewModel form)
            : base(form.CurrentPageLink, form.CurrentBlockLink)
        {
            Heading = block.Heading;
            ShowHeading = block.ShowHeading;
            CommentBoxRows = block.CommentBoxRows;
            CommentMaxLength = block.CommentMaxLength;
            CommentsDisplayMax = block.CommentsDisplayMax;
            CommentAuthor = form.Author;
            CommentBody = form.Body;
            Comments = new List<SocialComment>();
        }

        /// <summary>
        /// Gets or sets the heading for the frontend comments block display.
        /// </summary>
        public string Heading { get; set; }

        /// <summary>
        /// Gets or sets whether to show the block heading in the frontend comments block display.
        /// </summary>
        public bool ShowHeading { get; set; }

        /// <summary>
        /// Gets or sets the number of rows in the comment box.
        /// </summary>
        public int CommentBoxRows { get; set; }

        /// <summary>
        /// Gets or sets the max length of a new comment.
        /// </summary>
        public int CommentMaxLength { get; set; }

        /// <summary>
        /// Gets or sets the max length of a new comment.
        /// </summary>
        public int CommentsDisplayMax { get; set; }

        /// <summary>
        /// Gets or sets the comments to show.
        /// </summary>
        public IEnumerable<SocialComment> Comments { get; set; }

        /// <summary>
        /// Gets or sets a success message that should be flashed in the view.
        /// </summary>
        public string SubmitSuccessMessage { get; set; }

        /// <summary>
        /// Gets or sets an error message that should be flashed in the view.
        /// </summary>
        public string SubmitErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets an error message that should be flashed in the message display view.
        /// </summary>
        public string DisplayErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the username for the user who may currently submit comments.
        /// </summary>
        public string CommentAuthor { get; set; }

        /// <summary>
        /// Gets or sets the last submitted comment body.
        /// </summary>
        public string CommentBody { get; set; }
    }
}