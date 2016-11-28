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

        public RatingFormViewModel(PageReference currentPageLink, string pageId, ContentReference currentBlockLink)
        {
            CurrentPageLink = currentPageLink;
            PageId = pageId;
            CurrentBlockLink = currentBlockLink;
            //SubmittedRating = null; validate if rating was submitted
        }

        /// <summary>
        /// Gets or sets the reference link of the page containing the rating block.
        /// </summary>
        public PageReference CurrentPageLink { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the page containing the rating block.
        /// </summary>
        public string PageId { get; set; }

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