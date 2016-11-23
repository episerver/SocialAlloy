using EPiServer.Core;
using EPiServer.SocialAlloy.Web.Social.Blocks;
using EPiServer.SocialAlloy.Web.Social.Common.Models;

namespace EPiServer.SocialAlloy.Web.Social.Models
{
    /// <summary>
    /// The RatingBlockViewModel class represents the model that will be used to
    /// feed data to the rating block frontend view.
    /// </summary>
    public class RatingBlockViewModel : SocialBlockViewModel<RatingBlock>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="block"></param>
        /// <param name="form"></param>
        public RatingBlockViewModel(RatingBlock block,
                                    RatingFormViewModel form)
            : base(form.CurrentPageLink, form.CurrentBlockLink)
        {
            Heading = block.Heading;
            TotalCount = form.TotalCount;
            Average = form.Average;
            Rater = form.Rater;
            CurrentRating = form.CurrentRating;
        }

        /// <summary>
        /// The heading for the frontend comments block display.
        /// </summary>
        public string Heading { get; }

        /// <summary>
        /// The total number of ratings found for CurrentPageLink
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// The average of all ratings submitted for CurrentPageLink
        /// </summary>
        public decimal Average { get; set; }

        /// <summary>
        /// User who submitted the rating
        /// </summary>
        public string Rater { get; set; }

        /// <summary>
        /// The current rating, if any submitted by Rater for CurrentPageLink
        /// </summary>
        public int? CurrentRating { get; set; }
    }
}