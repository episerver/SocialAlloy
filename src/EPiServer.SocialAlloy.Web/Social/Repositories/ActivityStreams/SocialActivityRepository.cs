using EPiServer.Social.ActivityStreams.Core;
using EPiServer.Social.Common;
using EPiServer.SocialAlloy.Web.Social.Common.Exceptions;
using EPiServer.SocialAlloy.Web.Social.Models;

namespace EPiServer.SocialAlloy.Web.Social.Repositories
{
    /// <summary>
    /// The SocialActivityRepository class defines the operations that can be issued
    /// against the EPiServer Social activity repository when social activities occur
    /// on the site using the Episerver Social sample.
    /// </summary>
    public class SocialActivityRepository : ISocialActivityRepository
    {
        private readonly IActivityService service;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="service">an instance of the Episerver Social ActivityService</param>
        public SocialActivityRepository(IActivityService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Adds an activity to the EPiServer Social ActivityStreams system.
        /// </summary>
        /// <typeparam name="T">Type of the activity</typeparam>
        /// <param name="actor">the actor who initiated the activity</param>
        /// <param name="target">the target of the activity</param>
        /// <param name="activity">an instance of SocialActivity</param>
        /// <exception cref="SocialRepositoryException">Thrown when errors occur 
        /// interacting with the Social cloud services.</exception>
        public void Add<T>(string actor, string target, T activity) where T : SocialActivity
        {
            try
            {
                service.Add(new Activity(
                                Reference.Create(actor),
                                Reference.Create(target)), activity
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
    }
}