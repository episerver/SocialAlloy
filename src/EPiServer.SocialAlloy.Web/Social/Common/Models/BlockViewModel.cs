using EPiServer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPiServer.SocialAlloy.Web.Social.Common.Models
{
    public class BlockViewModel : SocialBlockViewModel
    {
        /// <summary>
        /// Gets the reference link of the page containing the frontend social block.
        /// </summary>
        public PageReference CurrentPageLink { get; }

        /// <summary>
        /// Gets the reference link of the frontend social block.
        /// </summary>
        public ContentReference CurrentBlockLink { get; }

        public MessageViewModel Message { get; set; }
    }
}