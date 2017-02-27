using EPiServer.ServiceLocation;
using EPiServer.SocialAlloy.Web.Social.Repositories;
using System.Web.Mvc;
using System.Web.Routing;

namespace EPiServer.SocialAlloy.Web.Social.Controllers
{
    /// <summary>
    /// The ModerationController handles the display of information
    /// for membership requests under moderation and faciliates
    /// the actions which may be taken upon them.
    /// </summary>
    public class ModerationController : Controller
    {
        private readonly ICommunityMembershipModerationRepository moderationRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        public ModerationController()
        {
            moderationRepository = ServiceLocator.Current.GetInstance<ICommunityMembershipModerationRepository>();
        }

        /// <summary>
        /// Presents a view which includes a list of the requests under
        /// moderation for the specified workflow. If no workflow is
        /// specified, the first one in the system will be selected.
        /// </summary>
        /// <param name="selectedWorkflow">ID of the selected moderation workflow</param>        
        public ActionResult Index(string selectedWorkflow)
        {
            var viewModel = this.moderationRepository.Get(selectedWorkflow);
            return View("~/Views/Social/Moderation/Index.cshtml", viewModel);
        }

        /// <summary>
        /// Retrieves relevant membership information and takes the specified action on a membership request under moderation.
        /// </summary>
        /// <param name="userId">User associated with the membership request</param>
        /// <param name="communityId">Community associated with the membership request</param>
        /// <param name="workflow">Workflow associated with the membership request</param>
        /// <param name="workflowAction">Action to be taken on the membership request</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        public ActionResult Index(string userId, string communityId, string workflow, string workflowAction)
        {
            this.moderationRepository.Moderate(workflow, workflowAction, userId, communityId);

            return RedirectToAction("Index", new RouteValueDictionary(new { SelectedWorkflow = workflow}));
        }
    }
}
