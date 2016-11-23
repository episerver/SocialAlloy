using EPiServer.Core;
using EPiServer.SocialAlloy.Web.Social.Blocks;
using EPiServer.SocialAlloy.Web.Social.Common.Models;
using System.Collections.Generic;

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
            //TODO remove the commented lines
            RatingValues = new List<int>() { 1, 2, 3, 4, 5 };
            //RatingValues = new List<int>();
            //RatingValues.AddRange(block.RatingValues);

            TotalCount = form.TotalCount;
            Average = form.Average;
            Rater = form.Rater;
            CurrentRating = form.CurrentRating;
            SubmittedRating = form.SubmittedRating;
        }

        /// <summary>
        /// The heading for the frontend rating block display.
        /// </summary>
        public string Heading { get; }

        /// <summary>
        /// The rating value settings for the frontend rating block display.
        /// </summary>

        public List<int> RatingValues { get; set; }
        //public List<int> RatingValues { get; }

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
        /// The existing rating, if any submitted by Rater for CurrentPageLink
        /// </summary>
        public int? CurrentRating { get; set; }

        /// <summary>
        /// The new rating submitted by Rater for CurrentPageLink
        /// </summary>
        public int SubmittedRating { get; set; }
    }
}