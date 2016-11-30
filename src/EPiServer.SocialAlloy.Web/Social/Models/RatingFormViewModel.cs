using EPiServer.Core;

namespace EPiServer.SocialAlloy.Web.Social.Models
{
    /// <summary>
    /// The RatingFormViewModel class represents the model of a social rating form view.
    /// </summary>
    public class RatingFormViewModel
    {
        public RatingFormViewModel()
        {
        }

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
        /// Gets or sets the reference link of the block containing the rating block.
        /// </summary>
        public ContentReference CurrentBlockLink { get; set; }

        /// <summary>
        /// The new rating submitted by logged in user for CurrentPageLink
        /// </summary>
        public int? SubmittedRating { get; set; }
    }
}