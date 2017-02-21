using System;

namespace EPiServer.SocialAlloy.Web.Social.Models
{
    /// <summary>
    /// The PageComment class describes a comment model used by the SocialAlloy site.
    /// </summary>
    public class PageComment
    {
        /// <summary>
        /// The comment author identifier.
        /// </summary>
        public string AuthorId { get; set; }

        /// <summary>
        /// The comment author username.
        /// </summary>
        public string AuthorUsername { get; set; }

        /// <summary>
        /// The comment body.
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// The reference to the target the comment applies to.
        /// </summary>
        public string Target { get; set; }

        /// <summary>
        /// The date/time the comment was created at.
        /// </summary>
        public DateTime Created { get; set; }
    }
}