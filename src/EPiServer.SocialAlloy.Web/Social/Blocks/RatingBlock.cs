using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;

namespace EPiServer.SocialAlloy.Web.Social.Blocks
{
    [ContentType(DisplayName = "RatingBlock", GUID = "069e2c52-fd48-49c5-8993-7a0347ea1f78", Description = "Social Rating Statistics")]
    public class RatingBlock : BlockData
    {
        [Display(
            GroupName = SystemTabNames.Content,
            Order = 1)]
        [CultureSpecific]
        public virtual string Heading { get; set; }

        /// <summary>
        /// Sets the default property values on the content data.
        /// </summary>
        /// <param name="contentType">Type of the content.</param>
        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);
            Heading = "Social Rating Statistics";
        }
    }
}