using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;

namespace EPiServer.SocialAlloy.Web.Social.Blocks
{
    /// <summary>
    /// The LikeButtonBlock class defines the configuration used for rendering like button views.
    /// </summary>
    [ContentType(DisplayName = "Like Button Block", 
                 GUID = "1dae01b7-72ad-4a9d-b543-82b0f5af7bbc", 
                 Description = "A Like Button block implementation using the EPiServer Social Ratings feature.")]
    public class LikeButtonBlock : BlockData
    {
    }
}