using EPiServer.Core;

namespace EPiServer.SocialAlloy.Web.Models.Pages
{
    public interface IHasRelatedContent
    {
        ContentArea RelatedContentArea { get; }
    }
}
