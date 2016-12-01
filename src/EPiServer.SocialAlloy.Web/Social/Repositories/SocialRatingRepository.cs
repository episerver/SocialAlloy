using EPiServer.Social.Common;
using EPiServer.Social.Ratings.Core;
using EPiServer.SocialAlloy.Web.Social.Common.Exceptions;
using EPiServer.SocialAlloy.Web.Social.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EPiServer.SocialAlloy.Web.Social.Repositories
{
    /// <summary>
    /// The SocialRatingRepository interface defines the operations that can be issued
    /// against the EPiServer Social rating repository.
    /// </summary>
    public class SocialRatingRepository : ISocialRatingRepository
    {
        private readonly IRatingService ratingService;

        /// <summary>
        /// Constructor
        /// </summary>
        public SocialRatingRepository(IRatingService ratingService)
        {
            this.ratingService = ratingService;
        }

        /// <summary>
        /// Adds a rating to the EPiServer Social rating repository for the 
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
                ratingService.Add(new Rating(
                                Reference.Create(user),
                                Reference.Create(target),
                                new RatingValue(value))
            );
            }
            catch (SocialAuthenticationException ex)
            {
                throw new SocialRepositoryException("The application failed to authenticate with EPiServer Social.", ex);
            }
            catch (MaximumDataSizeExceededException ex)
            {
                throw new SocialRepositoryException("The application request was deemed too large for EPiServer Social.", ex);
            }
            catch (SocialCommunicationException ex)
            {
                throw new SocialRepositoryException("The application failed to communicate with EPiServer Social.", ex);
            }
            catch (SocialException ex)
            {
                throw new SocialRepositoryException("EPiServer Social failed to process the application request.", ex);
            }
        }

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
        public int? GetRating(SocialRatingFilter filter)
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
                    result = ratingPage.Results.ToList().FirstOrDefault().Value.Value;
            }
            catch (SocialAuthenticationException ex)
            {
                throw new SocialRepositoryException("The application failed to authenticate with EPiServer Social.", ex);
            }
            catch (MaximumDataSizeExceededException ex)
            {
                throw new SocialRepositoryException("The application request was deemed too large for EPiServer Social.", ex);
            }
            catch (SocialCommunicationException ex)
            {
                throw new SocialRepositoryException("The application failed to communicate with EPiServer Social.", ex);
            }
            catch (SocialException ex)
            {
                throw new SocialRepositoryException("EPiServer Social failed to process the application request.", ex);
            }

            return result;
        }

        /// <summary>
        /// Gets the rating statistics, if any, from the EPiServer Social rating 
        /// repository for the specified target reference.
        /// </summary>
        /// <param name="target">The target reference by which to filter ratings statistics</param>
        /// <returns>The rating statistics if any exist, null otherwise.</returns>
        /// <exception cref="SocialRepositoryException">Thrown when errors occur communicating with 
        /// the Social cloud services.</exception>
        public SocialRatingStatistics GetRatingStatistics(string target)
        {
            SocialRatingStatistics result = null;

            try
            {
                var ratingStatisticsPage = ratingService.Get(new Criteria<RatingStatisticsFilter>()
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
                        result = new SocialRatingStatistics
                        {
                            Average = statistics.TotalCount > 0 ?
                                        Decimal.Divide(statistics.Sum, statistics.TotalCount) : 0,
                            TotalCount = statistics.TotalCount
                        };
                    }
                }
            }
            catch (SocialAuthenticationException ex)
            {
                throw new SocialRepositoryException("The application failed to authenticate with EPiServer Social.", ex);
            }
            catch (MaximumDataSizeExceededException ex)
            {
                throw new SocialRepositoryException("The application request was deemed too large for EPiServer Social.", ex);
            }
            catch (SocialCommunicationException ex)
            {
                throw new SocialRepositoryException("The application failed to communicate with EPiServer Social.", ex);
            }
            catch (SocialException ex)
            {
                throw new SocialRepositoryException("EPiServer Social failed to process the application request.", ex);
            }

            return result;
        }
    }
}