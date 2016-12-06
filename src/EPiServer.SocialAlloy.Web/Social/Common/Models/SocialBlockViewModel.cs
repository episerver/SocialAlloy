using EPiServer.Core;

namespace EPiServer.SocialAlloy.Web.Social.Common.Models
{
    /// <summary>
    /// The SocialBlockViewModel class contains common social block view model configuration.
    /// </summary>
    public abstract class SocialBlockViewModel
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="currentPageLink">An optional parameter containing the page reference of the current page containing the social block.</param>
        /// <param name="currentBlockLink">An optional parameter containing the reference of the current social block instance.</param>
        public SocialBlockViewModel(PageReference currentPageLink = null, ContentReference currentBlockLink = null)
        {
            CurrentPageLink = currentPageLink;
            CurrentBlockLink = currentBlockLink;
        }

        
        /// <summary>
        /// Gets the reference link of the page containing the frontend social block.
        /// </summary>
        public PageReference CurrentPageLink { get; }

        /// <summary>
        /// Gets the reference link of the frontend social block.
        /// </summary>
        public ContentReference CurrentBlockLink { get; }
    }
}