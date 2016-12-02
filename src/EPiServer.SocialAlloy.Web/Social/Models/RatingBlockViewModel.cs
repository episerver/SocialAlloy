using EPiServer.SocialAlloy.Web.Social.Blocks;
using EPiServer.SocialAlloy.Web.Social.Common.Models;
using System.Collections.Generic;
using System.Linq;

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
            ShowHeading = block.ShowHeading;

            LoadRatingSettings(block);

            if (form.SubmittedRating.HasValue)
                SubmittedRating = form.SubmittedRating.Value;
        }

        private void LoadRatingSettings(RatingBlock block)
        {
            RatingSettings = new List<int>();

            //if (block.EditableRatingSettings != null && block.EditableRatingSettings.Count > 0)
            //{
            //    foreach (var socialRating in block.EditableRatingSettings)
            //    {
            //        if (!RatingSettings.Contains(socialRating.Value))
            //            RatingSettings.Add(socialRating.Value);
            //    }
            //    RatingSettings.Sort();
            //}

            RatingSettings.AddRange(block.RatingSettings.Cast<RatingSetting>().Select(r => r.Value).ToList());
            RatingSettings.Sort();
        }

        /// <summary>
        /// The heading for the frontend rating block display.
        /// </summary>
        public string Heading { get; }

        /// <summary>
        /// Gets or sets whether to show the block heading in the frontend rating block display.
        /// </summary>
        public bool ShowHeading { get; set; }

        /// <summary>
        /// The rating value settings for the frontend rating block display.
        /// </summary>

        public List<int> RatingSettings { get; set; }

        /// <summary>
        /// The total number of ratings found for CurrentPageLink
        /// </summary>
        public long TotalCount { get; set; }

        /// <summary>
        /// The average of all ratings submitted for CurrentPageLink
        /// </summary>
        public double Average { get; set; }

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

        /// <summary>
        /// Message displayed in rating block if submitted rating saved successfully
        /// </summary>
        public string SubmitSuccessMessage { get; set; }

        /// <summary>
        /// Message displayed in rating block if error encountered while saving submitted rating
        /// </summary>
        public string SubmitErrorMessage { get; set; }

        /// <summary>
        /// Message displayed in rating block if error encountered while retrieving rating statistics or ratings for logged in user. 
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Message displayed in rating block if no rating statistics are found for the page 
        /// to prompt user to submit a rating for this page.
        /// </summary>
        public string NoStatisticsFoundMessage { get; set; }
        

    }
}