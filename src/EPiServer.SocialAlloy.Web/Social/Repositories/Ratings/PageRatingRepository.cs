using EPiServer.Social.Common;
using EPiServer.Social.Ratings.Core;
using EPiServer.SocialAlloy.Web.Social.Common.Exceptions;
using EPiServer.SocialAlloy.Web.Social.Models;
using System.Collections.Generic;
using System.Linq;

namespace EPiServer.SocialAlloy.Web.Social.Repositories
{
    /// <summary>
    /// The PageRatingRepository class defines the operations that can be issued
    /// against the Episerver Social RatingService.
    /// </summary>
    public class PageRatingRepository : IPageRatingRepository
    {
        private readonly IRatingService ratingService;
        private readonly IRatingStatisticsService ratingStatisticsService;

        /// <summary>
        /// Constructor
        /// </summary>
        public PageRatingRepository(IRatingService ratingService, IRatingStatisticsService ratingStatisticsService)
        {
            this.ratingService = ratingService;
            this.ratingStatisticsService = ratingStatisticsService;
        }

        /// <summary>
        /// Adds a rating with the Episerver Social Framework for the 
        /// target and user reference specified.
        /// </summary>
        /// <param name="user">the reference of rater who submitted the rating.</param>
        /// <param name="target">the reference of target the rating applies to.</param>
        /// <param name="value">the rating value that was submitted by the rater.</param>
        /// <exception cref="SocialRepositoryException">Thrown when errors occur communicating with 
        /// the Social cloud services.</exception>
        public void AddRating(string user, string target, int value)
        {
            try
            {
                var rating = ratingService.Add(new Rating(
                                Reference.Create(user),
                                Reference.Create(target),
                                new RatingValue(value)));

                if (rating == null)
                    throw new SocialRepositoryException("The newly submitted rating could not be added. Please try again");

            }
            catch (SocialAuthenticationException ex)
            {
                throw new SocialRepositoryException("The application failed to authenticate with Episerver Social.", ex);
            }
            catch (MaximumDataSizeExceededException ex)
            {
                throw new SocialRepositoryException("The application request was deemed too large for Episerver Social.", ex);
            }
            catch (SocialCommunicationException ex)
            {
                throw new SocialRepositoryException("The application failed to communicate with Episerver Social.", ex);
            }
            catch (SocialException ex)
            {
                throw new SocialRepositoryException("Episerver Social failed to process the application request.", ex);
            }
        }

        /// <summary>
        /// Gets the value of the submitted rating, if any, from the Episerver Social Framework based on the target and user reference specified in the filter.
        /// </summary>
        /// <param name="filter">Criteria containing the target and user reference by 
        /// which to filter ratings</param>
        /// <returns>The rating value matching the filter criteria, null otherwise, if rating 
        /// does not exist for the target and user reference specified in the filter.</returns>
        /// <exception cref="SocialRepositoryException">Thrown when errors occur communicating with 
        /// the Social cloud services.</exception>
        public int? GetRating(PageRatingFilter filter)
        {
            int? result = null;

            try
            {
                var ratingPage = ratingService.Get(new Criteria<RatingFilter>()
                {
                    Filter = new RatingFilter()
                    {
                        Rater = Reference.Create(filter.Rater),
                        Targets = new List<Reference> { Reference.Create(filter.Target) }
                    },
                    PageInfo = new PageInfo() { PageSize = 1 }
                });

                if (ratingPage.Results.Count() > 0)
                {
                    result = ratingPage.Results.ToList().FirstOrDefault().Value.Value;
                }
            }
            catch (SocialAuthenticationException ex)
            {
                throw new SocialRepositoryException("The application failed to authenticate with Episerver Social.", ex);
            }
            catch (MaximumDataSizeExceededException ex)
            {
                throw new SocialRepositoryException("The application request was deemed too large for Episerver Social.", ex);
            }
            catch (SocialCommunicationException ex)
            {
                throw new SocialRepositoryException("The application failed to communicate with Episerver Social.", ex);
            }
            catch (SocialException ex)
            {
                throw new SocialRepositoryException("Episerver Social failed to process the application request.", ex);
            }

            return result;
        }

        /// <summary>
        /// Gets the rating statistics, if any, from the Episerver Social rating 
        /// repository for the specified target reference.
        /// </summary>
        /// <param name="target">The target reference by which to filter ratings statistics</param>
        /// <returns>The rating statistics if any exist, null otherwise.</returns>
        /// <exception cref="SocialRepositoryException">Thrown when errors occur communicating with 
        /// the Social cloud services.</exception>
        public PageRatingStatistics GetRatingStatistics(string target)
        {
            PageRatingStatistics result = null;

            try
            {
                var ratingStatisticsPage = ratingStatisticsService.Get(new Criteria<RatingStatisticsFilter>()
                {
                    Filter = new RatingStatisticsFilter()
                    {
                        Targets = new List<Reference> { Reference.Create(target) }
                    },
                    PageInfo = new PageInfo() { PageSize = 1 }
                });

                if (ratingStatisticsPage.Results.Count() > 0)
                {
                    var statistics = ratingStatisticsPage.Results.ToList().FirstOrDefault();
                    if (statistics.TotalCount > 0)
                    {
                        result = new PageRatingStatistics
                        {
                            Average = (double)statistics.Sum/statistics.TotalCount,
                            TotalCount = statistics.TotalCount
                        };
                    }
                }
            }
            catch (SocialAuthenticationException ex)
            {
                throw new SocialRepositoryException("The application failed to authenticate with Episerver Social.", ex);
            }
            catch (MaximumDataSizeExceededException ex)
            {
                throw new SocialRepositoryException("The application request was deemed too large for Episerver Social.", ex);
            }
            catch (SocialCommunicationException ex)
            {
                throw new SocialRepositoryException("The application failed to communicate with Episerver Social.", ex);
            }
            catch (SocialException ex)
            {
                throw new SocialRepositoryException("Episerver Social failed to process the application request.", ex);
            }

            return result;
        }
    }
}