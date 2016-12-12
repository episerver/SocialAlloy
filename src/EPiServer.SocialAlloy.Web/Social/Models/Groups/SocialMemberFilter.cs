using EPiServer.Social.Groups.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPiServer.SocialAlloy.Web.Social.Models
{
    /// <summary>
    /// The SocialMemberFilter class exposes a set of properties by which social member repo can use to filter
    /// </summary>
    public class SocialMemberFilter
    {
        /// <summary>
        /// The group id to filter on.
        /// </summary>
        public GroupId GroupId { get; set; }

        /// <summary>
        /// The number of comments to retrieve.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// The offset to start retrieving the next page of comments from.
        /// </summary>
        public int PageOffset { get; set; }
    }
}