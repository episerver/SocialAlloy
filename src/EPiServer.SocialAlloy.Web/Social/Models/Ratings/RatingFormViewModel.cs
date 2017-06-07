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
        /// The new rating submitted by logged in user for the page containing the rating block
        /// </summary>
        public int? SubmittedRating { get; set; }

        /// <summary>
        /// Gets or sets the whether the block is configured to send an activity when a new rating is added. 
        /// </summary>
        public bool SendActivity { get; set; }

        /// <summary>
        /// Gets or sets the reference link of the page containing the comment form.
        /// </summary>
        public PageReference CurrentPageLink { get; set; }
    }
}