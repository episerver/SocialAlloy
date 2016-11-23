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
            Rater = string.Empty;
            CurrentRating = null;
        }

        //Rating Statistics related properties
        /// <summary>
        /// The total number of ratings found for CurrentPageLink
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// The average of all ratings submitted for CurrentPageLink
        /// </summary>
        public decimal Average { get; set; }

        /// <summary>
        /// The current rating, if any submitted by Rater for CurrentPageLink
        /// </summary>
        public int? CurrentRating { get; set; }

        /// <summary>
        /// Gets or sets the reference link of the page containing the rating block.
        /// </summary>
        public PageReference CurrentPageLink { get; set; }

        /// <summary>
        /// Gets or sets the reference link of the block containing the rating block.
        /// </summary>
        public ContentReference CurrentBlockLink { get; set; }

        //New Rating related properties
        /// <summary>
        /// The user who submitted the rating
        /// </summary>
        public string Rater { get; set; }

        /// <summary>
        /// The new rating submitted by Rater for CurrentPageLink
        /// </summary>
        public int SubmittedRating { get; set; }
    }
}