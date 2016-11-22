using EPiServer.SocialAlloy.Web.Social.Blocks;
using EPiServer.SocialAlloy.Web.Social.Common.Models;

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
            CommentAuthor = form.Author;
            CommentBody = form.Body;
        }

        /// <summary>
        /// The heading for the frontend comments block display.
        /// </summary>
        public string Heading { get; }

        /// <summary>
        /// A success message that should be flashed in the view.
        /// </summary>
        public string SuccessMessage { get; set; }

        /// <summary>
        /// A error message that should be flashed in the view.
        /// </summary>
        public string ErrorMessage { get; set; }

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