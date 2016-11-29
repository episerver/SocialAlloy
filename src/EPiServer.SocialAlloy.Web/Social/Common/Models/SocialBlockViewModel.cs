using EPiServer.Core;

namespace EPiServer.SocialAlloy.Web.Social.Common.Models
{
    /// <summary>
    /// The SocialBlockViewModel class contains common social block view model configuration.
    /// </summary>
    /// <typeparam name="T">The social block type.</typeparam>
    public abstract class SocialBlockViewModel<T> where T : BlockData
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="currentPageLink">The current page containing the social block.</param>
        /// <param name="currentBlockLink">The current block instance.</param>
        public SocialBlockViewModel(PageReference currentPageLink, ContentReference currentBlockLink)
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