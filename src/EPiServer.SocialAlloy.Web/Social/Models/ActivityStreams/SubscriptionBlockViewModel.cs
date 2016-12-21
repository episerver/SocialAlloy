using EPiServer.SocialAlloy.Web.Social.Blocks;
using EPiServer.SocialAlloy.Web.Social.Common.Models;
using System.Collections.Generic;

namespace EPiServer.SocialAlloy.Web.Social.Models
{
    /// <summary>
    /// The SubscriptionBlockViewModel class represents the model that will be used to
    /// feed data to the subscriptions block frontend view.
    /// </summary>
    public class SubscriptionBlockViewModel : SocialBlockViewModel
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="block">A block reference to use as a key under which to save the model state.</param>
        /// <param name="form">A subscription form view model to get current form values for the block view model</param>
        public SubscriptionBlockViewModel(SubscriptionBlock block, SubscriptionFormViewModel form)
            : base(form.CurrentPageLink, form.CurrentBlockLink)
        {
            Heading = block.Heading;
            ShowHeading = block.ShowHeading;
            ShowSubscriptionForm = false;
            UserSubscribedToPage = false;
        }

        /// <summary>
        /// Gets or sets whether to show subscription form.
        /// </summary>
        public bool ShowSubscriptionForm { get; set; }

        /// <summary>
        /// Gets or sets the heading for the frontend subscriptions block display.
        /// </summary>
        public string Heading { get; set; }

        /// <summary>
        /// Gets or sets whether to show the block heading in the frontend subscriptions block display.
        /// </summary>
        public bool ShowHeading { get; set; }

        /// <summary>
        /// Gets or sets whether the current user is subscribed to current page.
        /// </summary>
        public bool UserSubscribedToPage { get; set; }

        ///// <summary>
        ///// Gets or sets a success message that should be flashed in the view.
        ///// </summary>
        //public string SubmitSuccessMessage { get; set; }

        ///// <summary>
        ///// Gets or sets an error message that should be flashed in the view.
        ///// </summary>
        //public string SubmitErrorMessage { get; set; }

        /// <summary>
        /// Contains the infromation for displaying messaging to the user in the view
        /// </summary>
        public List<MessageViewModel> Messages { get; set; }
    }
}