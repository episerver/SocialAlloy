using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.SocialAlloy.Web.Social.Blocks;
using EPiServer.SocialAlloy.Web.Social.Common.Controllers;
using EPiServer.SocialAlloy.Web.Social.Common.Exceptions;
using EPiServer.SocialAlloy.Web.Social.Models;
using EPiServer.SocialAlloy.Web.Social.Repositories;
using EPiServer.Web.Routing;
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

        private const string ModelState_SubmitSuccessMessage = "SubmitSuccessMessage";
        private const string ModelState_SubmitErrorMessage = "SubmitErrorMessage";
        private const string ModelState_UserSubscribedToPage = "UserSubscribedToPage";
        private const string Action_Subscribe = "Subscribe";
        private const string Action_Unsubscribe = "Unssubscribe";
        private const string SubmitSuccessMessage = "Your request was processed successfully!";

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

            // Create a subscription form view model to fill the frontend form view
            var formViewModel = new SubscriptionFormViewModel(this.pageRouteHelper.PageLink, currentBlockLink);

            // Create a subscription block view model to fill the frontend block view
            var blockViewModel = new SubscriptionBlockViewModel(currentBlock, formViewModel);

            // Set Block View Model Properties
            SetBlockViewModelProperties(blockViewModel);

            // Apply current model state to the subscription block view model
            ApplyModelStateToSubscriptionBlockViewModel(blockViewModel);

            // Render the frontend block view
            return PartialView("~/Views/Social/SubscriptionBlock/SubscriptionView.cshtml", blockViewModel);
        }

        /// <summary>
        /// Subscribes the current user to the current page. It accepts a subscription form model,
        /// validates the form, stores the submitted subscription, and redirects back to the current page.
        /// </summary>
        /// <param name="formViewModel">The subscription form being submitted.</param>
        /// <returns>The submit action result.</returns>
        [HttpPost]
        public ActionResult Subscribe(SubscriptionFormViewModel formViewModel)
        {
            return HandleAction(Action_Subscribe, formViewModel);
        }

        /// <summary>
        /// Unsubscribes the current user from the current page. It accepts a subscription form model,
        /// validates the form, stores the submitted subscription, and redirects back to the current page.
        /// </summary>
        /// <param name="formViewModel">The subscription form being submitted.</param>
        /// <returns>The submit action result.</returns>
        [HttpPost]
        public ActionResult Unsubscribe(SubscriptionFormViewModel formViewModel)
        {
            return HandleAction(Action_Unsubscribe, formViewModel);
        }

        /// <summary>
        /// Handle subscribe/unsubscribe actions.
        /// </summary>
        /// <param name="actionName">The action.</param>
        /// <param name="formViewModel">The form view model.</param>
        /// <returns>The action result.</returns>
        private ActionResult HandleAction(string actionName, SubscriptionFormViewModel formViewModel)
        {
            var data = this.contentRepository.Get<IContentData>(formViewModel.CurrentBlockLink);

            var blockViewModel = new SubscriptionBlockViewModel(data as SubscriptionBlock, formViewModel);

            var subscription = this.AdaptSubscriptionFormViewModelToSocialSubscription(formViewModel);
            try
            {
                if (actionName == Action_Subscribe)
                {
                    this.subscriptionRepository.Add(subscription);
                    blockViewModel.UserSubscribedToPage = true;
                }
                else
                {
                    this.subscriptionRepository.Remove(subscription);
                    blockViewModel.UserSubscribedToPage = false;
                }
                blockViewModel.SubmitSuccessMessage = SubmitSuccessMessage;
            }
            catch (SocialRepositoryException ex)
            {
                blockViewModel.SubmitErrorMessage = ex.Message;
            }

            SaveModelState(formViewModel.CurrentBlockLink, CollectViewModelStateToSave(blockViewModel));

            return Redirect(UrlResolver.Current.GetUrl(formViewModel.CurrentPageLink));
        }

        /// <summary>
        /// Adapts the SubscriptionFormViewModel to a social SocialSubscription model.
        /// </summary>
        /// <param name="formViewModel">The subscription form view model.</param>
        /// <returns>A social subscription.</returns>
        private SocialSubscription AdaptSubscriptionFormViewModelToSocialSubscription(SubscriptionFormViewModel formViewModel)
        {
            return new SocialSubscription
            {
                Subscriber = this.userRepository.GetUserId(this.User),
                Target = this.GetPageId(formViewModel.CurrentPageLink),
                Type = SocialSubscription.PageSubscription
            };
        }

        /// <summary>
        /// Collects subscription block view model state that needs to be saved.
        /// </summary>
        /// <param name="blockViewModel">The subscription block view model.</param>
        /// <returns>A model state dictionary.</returns>
        private ModelStateDictionary CollectViewModelStateToSave(SubscriptionBlockViewModel blockViewModel)
        {
            var transientState = new ModelStateDictionary
            {
                new KeyValuePair<string, ModelState>
                (
                    ModelState_SubmitSuccessMessage,
                    new ModelState() {
                        Value = new ValueProviderResult(blockViewModel.SubmitSuccessMessage, null, CultureInfo.CurrentCulture)
                    }
                ),
                new KeyValuePair<string, ModelState>
                (
                    ModelState_SubmitErrorMessage,
                    new ModelState() {
                        Value = new ValueProviderResult(blockViewModel.SubmitErrorMessage, null, CultureInfo.CurrentCulture)
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
        /// <param name="blockViewModel">The subscription block view model to apply model state to.</param>
        private void ApplyModelStateToSubscriptionBlockViewModel(SubscriptionBlockViewModel blockViewModel)
        {
            // Get success/error model state
            var successMessage = GetModelState(ModelState_SubmitSuccessMessage);
            var errorMessage = GetModelState(ModelState_SubmitErrorMessage);
            var userSubscribedToCurrentPage = GetModelState(ModelState_UserSubscribedToPage);

            // Apply success/error model state to the view model
            blockViewModel.SubmitSuccessMessage = successMessage != null ? (string)successMessage.Value.RawValue : "";
            blockViewModel.SubmitErrorMessage = errorMessage != null ? (string)errorMessage.Value.RawValue : blockViewModel.SubmitErrorMessage;
            blockViewModel.UserSubscribedToPage = blockViewModel.UserSubscribedToPage;
        }

        /// <summary>
        /// Set any properties the block view model needs for the view to render properly.
        /// </summary>
        /// <param name="blockViewModel">The subscription block view model.</param>
        private void SetBlockViewModelProperties(SubscriptionBlockViewModel blockViewModel)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                blockViewModel.ShowSubscriptionForm = true;
                SetUserSubscribedToPage(blockViewModel);
            }
        }

        /// <summary>
        /// Set the block view  model property indicating whether the current user is subscribed to the current page.
        /// </summary>
        /// <param name="blockViewModel">The subscription block view model.</param>
        private void SetUserSubscribedToPage(SubscriptionBlockViewModel blockViewModel)
        {
            try
            {
                var filter = new SocialSubscriptionFilter
                {
                    Subscriber = this.userRepository.GetUserId(this.User),
                    Target = this.GetPageId(blockViewModel.CurrentPageLink),
                    Type = SocialSubscription.PageSubscription
                };

                if (this.subscriptionRepository.Exist(filter))
                {
                    blockViewModel.UserSubscribedToPage = true;
                }
                else
                {
                    blockViewModel.UserSubscribedToPage = false;
                }
            }
            catch (SocialRepositoryException ex)
            {
                blockViewModel.SubmitErrorMessage = ex.Message;
            }
        }
    }
}
