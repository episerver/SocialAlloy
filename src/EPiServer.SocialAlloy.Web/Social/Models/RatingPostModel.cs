using EPiServer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPiServer.SocialAlloy.Web.Social.Models
{
    public class RatingPostModel
    {
        /// <summary>
        /// Gets or sets the reference link of the page containing the rating block.
        /// </summary>
        public PageReference CurrentPageLink { get; set; }

        /// <summary>
        /// Gets or sets the reference link of the block containing the rating block.
        /// </summary>
        public ContentReference CurrentBlockLink { get; set; }

        //New Rating related properties
        /// <summary>
        /// The user who submitted the rating
        /// </summary>
        public string Rater { get; set; }

        /// <summary>
        /// The new rating submitted by Rater for CurrentPageLink
        /// </summary>
        public int SubmittedRating { get; set; }
    }
}
