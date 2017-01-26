using EPiServer.Core;
using EPiServer.SocialAlloy.Web.Social.Blocks;
using EPiServer.SocialAlloy.Web.Social.Common.Models;
using System.Collections.Generic;

namespace EPiServer.SocialAlloy.Web.Social.Models
{
    /// <summary>
    /// The CommentsBlockViewModel class represents the model that will be used to
    /// feed data to the comments block frontend view.
    /// </summary>
    public class CommentsBlockViewModel
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="block">A block reference to use as a key under which to save the model state.</param>
        /// <param name="form">A comment form view model to get current form values for the block view model</param>
        public CommentsBlockViewModel(CommentsBlock block, PageReference pageReference)
        {
            Heading = block.Heading;
            ShowHeading = block.ShowHeading;
            CommentBoxRows = block.CommentBoxRows;
            CommentMaxLength = block.CommentMaxLength; 
            CommentsDisplayMax = block.CommentsDisplayMax;
            Comments = new List<SocialComment>();
            SendActivity = block.SendActivity;
            CurrentPageLink = pageReference;
        }
        /// <summary>
        /// Gets or sets the reference link of the page containing the comment form.
        /// </summary>
        public PageReference CurrentPageLink { get; set; }

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
        /// Gets and sets message details to be displayed to the user
        /// </summary>
        public List<MessageViewModel> Messages { get; set; }

        /// <summary>
        /// Gets or sets the whether the block is configured to send an activity when a new comment is added. 
        /// </summary>
        public bool SendActivity { get; private set; }
    }
}