using EPiServer.SocialAlloy.Web.Social.Comments.Blocks;

namespace EPiServer.SocialAlloy.Web.Social.Comments.Models
{
    /// <summary>
    /// The CommentsDisplayModel class represents the model that will be used to
    /// feed comment data to the CommentsDisplayView.
    /// </summary>
    public class CommentsDisplayModel
    {
        public CommentsDisplayModel(CommentsBlock block)
        {
            Heading = block.Heading;
        }

        /// <summary>
        /// The heading for the comments display.
        /// </summary>
        public string Heading { get; set; }
    }
}