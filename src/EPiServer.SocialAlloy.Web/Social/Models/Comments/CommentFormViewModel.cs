using EPiServer.Core;

namespace EPiServer.SocialAlloy.Web.Social.Models
{
    /// <summary>
    /// The CommentFormViewModel class represents the model of a social comment form view.
    /// </summary>
    public class CommentFormViewModel
    {
        /// <summary>
        /// Default parameterless constructor required for view form submitting.
        /// </summary>
        public CommentFormViewModel()
        {
        }

        /// <summary>
        /// The comment body
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets the whether the block is configured to send an activity when a new comment is added. 
        /// </summary>
        public bool SendActivity { get; set; }

        /// <summary>
        /// Gets or sets the reference link of the page containing the comment form.
        /// </summary>
        public PageReference CurrentPageLink { get; set; }
    }

}