using EPiServer.Core;

namespace EPiServer.SocialAlloy.Web.Social.Models
{
    /// <summary>
    /// The RatingFormViewModel class represents the model of a social rating form view.
    /// </summary>
    public class RatingFormViewModel
    {
        /// <summary>
        /// Default parameterless constructor required for view form submitting.
        /// </summary>
        public RatingFormViewModel()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="currentPageLink">the reference link of the page containing the social rating block</param>
        /// <param name="currentBlockLink">the reference of the social rating block</param>
        public RatingFormViewModel(PageReference currentPageLink, ContentReference currentBlockLink)
        {
            CurrentPageLink = currentPageLink;
            CurrentBlockLink = currentBlockLink;
        }

        /// <summary>
        /// Gets or sets the reference link of the page containing the rating block.
        /// </summary>
        public PageReference CurrentPageLink { get; set; }

        /// <summary>
        /// Gets or sets the reference link of the rating block.
        /// </summary>
        public ContentReference CurrentBlockLink { get; set; }

        /// <summary>
        /// The new rating submitted by logged in user for the page containing the rating block
        /// </summary>
        public int? SubmittedRating { get; set; }
    }
}