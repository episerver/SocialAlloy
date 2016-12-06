using EPiServer.Core;

namespace EPiServer.SocialAlloy.Web.Social.Repositories
{
    public interface IPageRepository
    {
        /// <summary>
        /// Gets the page Id given its page reference.
        /// </summary>
        /// <param name="pageLink">The page reference.</param>
        /// <returns>The page Id.</returns>
        string GetPageId(PageReference pageLink);

        /// <summary>
        /// Gets the name of the page that has the specified identifier
        /// </summary>
        /// <param name="pageId">the page Id</param>
        /// <returns>the name of the page</returns>
        string GetPageName(string pageId);
    }
}