using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace EPiServer.SocialAlloy.Web.Social.Comments.Blocks
{
    /// <summary>
    /// The CommentBlock class defines the configuration used for rendering comments views.
    /// </summary>
    [ContentType(DisplayName = "Comments Block", GUID = "b8a110ff-a8e2-4c17-9706-ce777694ebd4", Description = "")]
    public class CommentsBlock : BlockData
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
            Heading = "Social Comments";
        }
    }
}