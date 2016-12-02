using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.SocialAlloy.Web.Social.Blocks;
using EPiServer.SocialAlloy.Web.Social.Common.Controllers;
using EPiServer.SocialAlloy.Web.Social.Common.Exceptions;
using EPiServer.SocialAlloy.Web.Social.Models;
using EPiServer.SocialAlloy.Web.Social.Repositories;
using EPiServer.Web.Routing;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace EPiServer.SocialAlloy.Web.Social.Controllers
{
    /// <summary>
    /// The SubscriptionBlockController handles the rendering of the subscription block frontend view as well
    /// as the posting of new subscriptions.
    /// </summary>
    public class SubscriptionBlockController : SocialBlockController<SubscriptionBlock>
    {
        private readonly IUserRepository userRepository;
        private readonly ISocialSubscriptionRepository subscriptionRepository;

        private const string SubmitSuccessMessage = "SubmitSuccessMessage";
        private const string SubmitErrorMessage = "SubmitErrorMessage";

        /// <summary>
        /// Constructor
        /// </summary>
        public SubscriptionBlockController()
        {
            this.userRepository = ServiceLocator.Current.GetInstance<IUserRepository>();
            this.subscriptionRepository = ServiceLocator.Current.GetInstance<ISocialSubscriptionRepository>();
        }

        /// <summary>
        /// Render the subscription block frontend view.
        /// </summary>
        /// <param name="currentBlock">The current frontend block instance.</param>
        /// <returns>The action's result.</returns>
        public override ActionResult Index(SubscriptionBlock currentBlock)
        {
            var currentBlockLink = ((IContent)currentBlock).ContentLink;

            // Restore the saved model state
            LoadModelState(currentBlockLink);

            var subscriptionForm = new SubscriptionFormViewModel(this.pageRouteHelper.PageLink, currentBlockLink);

            // Create a subscription block view model to fill the frontend block view
            var subscriptionBlockViewModel = new SubscriptionBlockViewModel(currentBlock, subscriptionForm);

            // Apply current model state to the subscription block view model
            ApplyModelStateToSubscriptionBlockViewModel(subscriptionBlockViewModel);

            return PartialView("~/Views/Social/SubscriptionBlock/SubscriptionView.cshtml", subscriptionBlockViewModel);
        }

        /// <summary>
        /// Submit handles the submitting of new subscriptions.  It accepts a subscription form model,
        /// validates the form, stores the submitted subscription, and redirects back to the current page.
        /// </summary>
        /// <param name="subscriptionForm">The subscription form being submitted.</param>
        /// <returns>The submit action result.</returns>
        [HttpPost]
        public ActionResult Submit(SubscriptionFormViewModel subscriptionForm)
        {
            var data = this.contentRepository.Get<IContentData>(subscriptionForm.CurrentBlockLink);

            var subscriptionsViewModel = new SubscriptionBlockViewModel(data as SubscriptionBlock, subscriptionForm);

            var errors = ValidateSubscriptionForm(subscriptionForm);

            if (errors.Count() == 0)
            {
                var subscription = this.AdaptSubscriptionFormViewModelToSocialSubscription(subscriptionForm);
                try
                {
                    this.subscriptionRepository.Add(subscription);
                    subscriptionsViewModel.SubmitSuccessMessage = "Your subscription was submitted successfully!";
                }
                catch (SocialRepositoryException ex)
                {
                    subscriptionsViewModel.SubmitErrorMessage = ex.Message;
                }
            }
            else
            {
                // Flag the SubscriptionBody model state with validation error
                ModelState.AddModelError("SubscriptionBody", errors.First());
            }

            SaveModelState(subscriptionForm.CurrentBlockLink, CollectViewModelStateToSave(subscriptionsViewModel));

            return Redirect(UrlResolver.Current.GetUrl(subscriptionForm.CurrentPageLink));
        }

        /// <summary>
        /// Adapts the subscription form to a social subscription model.
        /// </summary>
        /// <param name="subscriptionForm">The subscription form view model.</param>
        /// <returns>A social subscription.</returns>
        private SocialSubscription AdaptSubscriptionFormViewModelToSocialSubscription(SubscriptionFormViewModel subscriptionForm)
        {
            return new SocialSubscription
            {
                // TODO: fill properties
            };
        }

        /// <summary>
        /// Validates the subscription form.
        /// </summary>
        /// <param name="subscriptionForm">The subscription form view model.</param>
        /// <returns>Returns a list of validation errors.</returns>
        private List<string> ValidateSubscriptionForm(SubscriptionFormViewModel subscriptionForm)
        {
            var errors = new List<string>();

            // TODO:  any validation?

            return errors;
        }

        /// <summary>
        /// Collects subscription block view model state that needs to be saved.
        /// </summary>
        /// <param name="subscriptionsViewModel">The subscription block view model.</param>
        /// <returns>A model state dictionary.</returns>
        private ModelStateDictionary CollectViewModelStateToSave(SubscriptionBlockViewModel subscriptionsViewModel)
        {
            var transientState = new ModelStateDictionary
            {
                new KeyValuePair<string, ModelState>
                (
                    SubmitSuccessMessage,
                    new ModelState() {
                        Value = new ValueProviderResult(subscriptionsViewModel.SubmitSuccessMessage, subscriptionsViewModel.SubmitSuccessMessage, CultureInfo.CurrentCulture)
                    }
                ),
                new KeyValuePair<string, ModelState>
                (
                    SubmitErrorMessage,
                    new ModelState() {
                        Value = new ValueProviderResult(subscriptionsViewModel.SubmitErrorMessage, subscriptionsViewModel.SubmitErrorMessage, CultureInfo.CurrentCulture)
                    }
                )
            };

            var modelState = ViewData.ModelState;
            if (transientState != null)
            {
                modelState.Merge(transientState);
            }

            return modelState;
        }

        /// <summary>
        /// Applies current model state to the subscription block view model.
        /// </summary>
        /// <param name="subscriptionBlockViewModel">The subscription block view model to apply model state to.</param>
        private void ApplyModelStateToSubscriptionBlockViewModel(SubscriptionBlockViewModel subscriptionBlockViewModel)
        {
            // Get success/error model state
            var successMessage = GetModelState(SubmitSuccessMessage);
            var errorMessage = GetModelState(SubmitErrorMessage);

            // Apply success/error model state to the view model
            subscriptionBlockViewModel.SubmitSuccessMessage = successMessage != null ? successMessage.Value.AttemptedValue : "";
            subscriptionBlockViewModel.SubmitErrorMessage = errorMessage != null ? errorMessage.Value.AttemptedValue : "";
        }
    }
}
