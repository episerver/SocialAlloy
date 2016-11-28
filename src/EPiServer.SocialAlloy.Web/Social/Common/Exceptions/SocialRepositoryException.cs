using System;

namespace EPiServer.SocialAlloy.Web.Social.Common.Exceptions
{
    public class SocialRepositoryException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">A custom message for the exception.</param>
        public SocialRepositoryException(string message = null) : base(message)
        {
        }
    }
}