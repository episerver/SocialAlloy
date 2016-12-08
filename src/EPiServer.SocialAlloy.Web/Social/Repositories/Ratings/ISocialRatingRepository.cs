using EPiServer.SocialAlloy.Web.Social.Models;

namespace EPiServer.SocialAlloy.Web.Social.Repositories
{
    /// <summary>
    /// The ISocialRatingRepository interface defines the operations that can be issued
    /// against a rating repository.
    /// </summary>
    interface ISocialRatingRepository
    {
        /// <summary>
        /// Gets the value of the submitted rating, if any, from the EPiServer Social rating 
        /// repository based on the target and user reference specified in the filter.
        /// </summary>
        /// <param name="filter">Criteria containing the target and user reference by 
        /// which to filter ratings</param>
        /// <returns>The rating value matching the filter criteria, null otherwise, if rating 
        /// does not exist for the target and user reference specified in the filter.</returns>
        /// <exception cref="SocialRepositoryException">Thrown when errors occur communicating with 
        /// the Social cloud services.</exception>
        int? GetRating(SocialRatingFilter filter);

        /// <summary>
        /// Gets the rating statistics, if any, from the EPiServer Social rating 
        /// repository for the specified target reference.
        /// </summary>
        /// <param name="target">The target reference by which to filter ratings statistics</param>
        /// <returns>The rating statistics if any exist, null otherwise.</returns>
        /// <exception cref="SocialRepositoryException">Thrown when errors occur communicating with 
        /// the Social cloud services.</exception>
        SocialRatingStatistics GetRatingStatistics(string target);

        /// <summary>
        /// Adds a rating to the EPiServer Social rating repository for the 
        /// target and user reference specified.
        /// </summary>
        /// <param name="user">the reference of rater who submitted the rating.</param>
        /// <param name="target">the reference of target the rating applies to.</param>
        /// <param name="value">the rating value that was submitted by the rater.</param>
        /// <exception cref="SocialRepositoryException">Thrown when errors occur communicating with 
        /// the Social cloud services.</exception>
        void AddRating(string user, string target, int value);
    }
}
