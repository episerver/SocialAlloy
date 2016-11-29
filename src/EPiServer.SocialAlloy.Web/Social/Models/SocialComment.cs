using System;

namespace EPiServer.SocialAlloy.Web.Social.Models
{
    /// <summary>
    /// The SocialComment class describes a comment model used by the SocialAlloy site.
    /// </summary>
    public class SocialComment
    {
        /// <summary>
        /// The comment author.
        /// </summary>
        public string Author { get; set; }

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