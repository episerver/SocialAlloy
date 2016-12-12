using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Social.Groups.Core;
using EPiServer.SocialAlloy.Web.Social.Blocks.Groups;
using EPiServer.SocialAlloy.Web.Social.Common.Controllers;
using EPiServer.SocialAlloy.Web.Social.Common.Exceptions;
using EPiServer.SocialAlloy.Web.Social.Models.Groups;
using EPiServer.SocialAlloy.Web.Social.Repositories;
using EPiServer.Web.Routing;
using System.Web.Mvc;

namespace EPiServer.SocialAlloy.Web.Social.Controllers
{
    /// <summary>
    /// The GroupCreationBlockController handles rendering the Group Creation block view 
    /// </summary>
    public class GroupCreationBlockController : SocialBlockController<GroupCreationBlock>
    {
        private readonly ISocialGroupRepository groupRepository;

        public GroupCreationBlockController()
        {
            this.groupRepository = ServiceLocator.Current.GetInstance<ISocialGroupRepository>();
        }

        /// <summary>
        /// Render the GroupCreationBlock view.
        /// </summary>
        /// <param name="currentBlock">The current block instance.</param>
        /// <returns></returns>
        public override ActionResult Index(GroupCreationBlock currentBlock)
        {
            var currentBlockLink = ((IContent)currentBlock).ContentLink;
            var successMessage = TempData["GroupCreationSuccessMessage"] == null ? null : TempData["GroupCreationSuccessMessage"].ToString();
            var errorMessage = TempData["GroupCreationErrorMessage"] == null ? null : TempData["GroupCreationErrorMessage"].ToString();

            //populate model to pass to block view
            var groupCreationBlockModel = new GroupCreationBlockViewModel()
            {
                Heading = currentBlock.Heading,
                ShowHeading = currentBlock.ShowHeading,
                CurrentBlockLink = currentBlockLink,
                CurrentPageLink = pageRouteHelper.PageLink,
                SubmitSuccessMessage = successMessage,
                SubmitErrorMessage = errorMessage
        };

            //remove existing values from input fields
            ModelState.Clear();

            //return block view
            return PartialView("~/Views/Social/GroupCreationBlock/Index.cshtml", groupCreationBlockModel);
        }

        /// <summary>
        /// Submit handles the submitting of new groups.  It accepts a group creation form model,
        /// stores the submitted group, and redirects back to the current page.
        /// </summary>
        /// <param name="groupCreationForm">The group form being submitted.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Submit(GroupCreationBlockViewModel model)
        {
            var data = this.contentRepository.Get<IContentData>(model.CurrentBlockLink);

            var validatedInputs = ValidateGroupInputs(model.Name, model.Description);
            
            AddGroup(model, validatedInputs);

            return Redirect(UrlResolver.Current.GetUrl(model.CurrentPageLink));
        }

        private void AddGroup(GroupCreationBlockViewModel model, bool validatedInputs)
        {
            if (validatedInputs)
            {
                try
                {
                    var group = new Group(model.Name, model.Description);
                    this.groupRepository.Add(group);
                    TempData["GroupCreationSuccessMessage"] = "Your group: " + model.Name + " was added successfully!";
                }
                catch (SocialRepositoryException ex)
                {
                    TempData["GroupCreationErrorMessage"] = ex.Message;
                }
            }
            else
            {
                TempData["GroupCreationErrorMessage"] = "Group name and description cannot be null or whitespace";
            }
        }

        private bool ValidateGroupInputs(string groupName, string groupDescription)
        {
            return !string.IsNullOrWhiteSpace(groupName) && !string.IsNullOrWhiteSpace(groupDescription);
        }
    }
}