using System;

namespace EPiServer.SocialAlloy.Web.Social.Common.Exceptions
{
    /// <summary>
    /// The SocialRepositoryException class is used to wrap any exception that
    /// may be thrown while using the social services.
    /// </summary>
    public class SocialRepositoryException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">A custom message for the exception.</param>
        public SocialRepositoryException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">A custom message for the exception.</param>
        /// <param name="ex">The inner social exception.</param>
        public SocialRepositoryException(string message, Exception ex) 
            : base(message, ex)
        {
        }
    }
}