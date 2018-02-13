using EPiServer.Core;
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
    public class RatingBlockViewModel 
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="block">A block reference to use as a key under which to save the model state.</param>
        /// <param name="form">A rating form view model to get current form values for the block view model</param>
        public RatingBlockViewModel(RatingBlock block, PageReference currentPageLink)
        {
            Heading = block.Heading;
            ShowHeading = block.ShowHeading;
            SendActivity = block.SendActivity;
            CurrentPageLink = currentPageLink;
            LoadRatingSettings(block);
        }

        /// <summary>
        /// Gets or sets the reference link of the page containing the rating form.
        /// </summary>
        public PageReference CurrentPageLink { get; set; }

        /// <summary>
        /// Gets the heading for the frontend rating block display.
        /// </summary>
        public string Heading { get; }

        /// <summary>
        /// Gets or sets whether to show the block heading in the frontend rating block display.
        /// </summary>
        public bool ShowHeading { get; set; }

        /// <summary>
        /// Gets or sets the rating value settings for the frontend rating block display.
        /// </summary>

        public List<int> RatingSettings { get; set; }

        /// <summary>
        /// Gets or sets the total number of ratings found for the page containing the rating block.
        /// </summary>
        public long TotalCount { get; set; }

        /// <summary>
        /// Gets or sets the average of all ratings submitted for the page containing the rating block.
        /// </summary>
        public double Average { get; set; }

        /// <summary>
        /// Gets or sets the existing rating, if any submitted by Rater for the page containing the rating block.
        /// </summary>
        public int? CurrentRating { get; set; }

        /// <summary>
        /// Gets or sets the new rating submitted by Rater for the page containing the rating block.
        /// </summary>
        public int SubmittedRating { get; set; }

        /// <summary>
        /// Gets and sets message details to be displayed to the user
        /// </summary>
        public List<MessageViewModel> Messages  { get; set; }

        /// <summary>
        /// Gets or sets the message displayed in rating block if no rating statistics are found for the page 
        /// to prompt user to submit a rating for this page.
        /// </summary>
        public string NoStatisticsFoundMessage { get; set; }


        /// <summary>
        /// Gets or sets the whether the block is configured to send an activity when a new rating is added. 
        /// </summary>
        public bool SendActivity { get; private set; }


        private void LoadRatingSettings(RatingBlock block)
        {
            RatingSettings = new List<int>();

            if (block.RatingSettings?.Any() == true)
            {
                RatingSettings.AddRange(block.RatingSettings.Select(r => r.Value));
                RatingSettings.Sort();
            }
        }
    }
}