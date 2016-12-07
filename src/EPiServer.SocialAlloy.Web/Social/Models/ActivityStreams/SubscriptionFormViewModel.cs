using EPiServer.Core;

namespace EPiServer.SocialAlloy.Web.Social.Models
{
    /// <summary>
    /// The SubscriptionFormViewModel class represents the model of a social subscription form view.
    /// </summary>
    public class SubscriptionFormViewModel
    {
        /// <summary>
        /// Default parameterless constructor required for view form submitting.
        /// </summary>
        public SubscriptionFormViewModel()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentPageLink"></param>
        /// <param name="pageId"></param>
        /// <param name="currentBlockLink"></param>
        public SubscriptionFormViewModel(PageReference currentPageLink, ContentReference currentBlockLink)
        {
            CurrentPageLink = currentPageLink;
            CurrentBlockLink = currentBlockLink;
        }

        //
        // COMMENT FORM METADATA
        //

        /// <summary>
        /// Gets or sets the reference link of the page containing the subscription form.
        /// </summary>
        public PageReference CurrentPageLink { get; set; }

        /// <summary>
        /// Gets or sets the reference link of the block containing the subscription form.
        /// </summary>
        public ContentReference CurrentBlockLink { get; set; }
    }
}