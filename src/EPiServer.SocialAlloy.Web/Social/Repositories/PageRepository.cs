using EPiServer.Core;
using System;

namespace EPiServer.SocialAlloy.Web.Social.Repositories
{
    public class PageRepository : IPageRepository
    {
        private readonly IContentRepository contentRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="contentRepository">an instance of the Episerver's content repository</param>
        public PageRepository(IContentRepository contentRepository) 
        {
            this.contentRepository = contentRepository;
        }

        /// <summary>
        /// Gets the page Id given its page reference.
        /// </summary>
        /// <param name="pageLink">The page reference.</param>
        /// <returns>The page Id.</returns>
        public string GetPageId(PageReference pageLink)
        {
            var pageData = contentRepository.Get<PageData>(pageLink as ContentReference);
            return pageData != null ? pageData.ContentGuid.ToString() : String.Empty;
        }

        /// <summary>
        /// Gets the name of the page that has the specified identifier
        /// </summary>
        /// <param name="pageId">the page Id</param>
        /// <returns>the name of the page</returns>
        public string GetPageName(string pageId)
        {
            var pageName = String.Empty;
            try
            {
                Guid pageIdGuid;
                if (Guid.TryParse(pageId, out pageIdGuid) && pageIdGuid != Guid.Empty)
                {
                    var data = contentRepository.Get<PageData>(pageIdGuid);
                    pageName = data.Name;
                }
            }
            catch (ContentNotFoundException)
            {
                pageName = "[Undetermined page name with Id: " + pageId + "]";
            }

            return pageName;
        }
    }
}