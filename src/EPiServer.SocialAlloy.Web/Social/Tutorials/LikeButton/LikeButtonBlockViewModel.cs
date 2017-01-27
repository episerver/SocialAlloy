namespace EPiServer.SocialAlloy.Web.Social.Models
{
    /// <summary>
    /// The LikeButtonBlockViewModel class represents the model that will be used to
    /// provide data to the Like button block frontend view.
    /// </summary>
    public class LikeButtonBlockViewModel
    {
        /// <summary>
        /// Parameterless constructor -- required by the EPiServer CMS framework.
        /// </summary>
        public LikeButtonBlockViewModel()
        {
        }

        /// <summary>
        /// Gets or sets the total number of Liked ratings found for the current page.
        /// </summary>
        public long TotalCount { get; set; }

        /// <summary>
        /// Gets or sets the existing Liked rating, if any, submitted by current user for the current page.
        /// </summary>
        public int? CurrentRating { get; set; }
    }
}