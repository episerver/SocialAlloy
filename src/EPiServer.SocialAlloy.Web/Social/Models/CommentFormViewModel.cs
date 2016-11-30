using EPiServer.Core;

namespace EPiServer.SocialAlloy.Web.Social.Models
{
    /// <summary>
    /// The CommentFormViewModel class represents the model of a social comment form view.
    /// </summary>
    public class CommentFormViewModel
    {
        public CommentFormViewModel()
        {
        }

        public CommentFormViewModel(PageReference currentPageLink, ContentReference currentBlockLink)
        {
            CurrentPageLink = currentPageLink;
            CurrentBlockLink = currentBlockLink;
        }

        /// <summary>
        /// The comment author
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// The comment body
        /// </summary>
        public string Body { get; set; }

        //
        // COMMENT FORM METADATA
        //

        /// <summary>
        /// Gets or sets the reference link of the page containing the comment form.
        /// </summary>
        public PageReference CurrentPageLink { get; set; }

        /// <summary>
        /// Gets or sets the reference link of the block containing the comment form.
        /// </summary>
        public ContentReference CurrentBlockLink { get; set; }  
    }
}