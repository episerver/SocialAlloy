using EPiServer.Social.Common;
using EPiServer.Social.Moderation.Core;
using EPiServer.SocialAlloy.ExtensionData.Membership;
using EPiServer.SocialAlloy.Web.Social.Models.Groups;
using EPiServer.SocialAlloy.Web.Social.Repositories;
using System.Linq;

namespace EPiServer.SocialAlloy.Web.Social.Adapters.Moderation
{
    /// <summary>
    /// Adapts data into a CommunityMembershipRequest.
    /// </summary>
    public class CommunityMembershipRequestAdapter
    {
        private Workflow workflow;
        private readonly IUserRepository userRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="workflow">The workflow that will be adapted</param>
        public CommunityMembershipRequestAdapter(Workflow workflow, IUserRepository userRepository)
        {
            this.workflow = workflow;
            this.userRepository = userRepository;
        }

        /// <summary>
        /// Converts a composite WorkflowItem with extension data of type AddMemberRequestModel into a CommunityMembershipRequest
        /// </summary>
        /// <param name="item">Composite item to be adapted into a CommunityMembershipRequest</param>
        /// <returns>CommunityMembershipRequest</returns>
        public CommunityMembershipRequest Adapt(Composite<WorkflowItem, AddMemberRequest> item)
        {
            var user = item.Extension.User;
            var userName = userRepository.ParseUserUri(user);

            return new CommunityMembershipRequest
            {
                User = user,
                Group = item.Extension.Group,
                WorkflowId = item.Data.Workflow.ToString(),
                Created = item.Data.Created.ToLocalTime(),
                State = item.Data.State.Name,
                Actions = workflow.ActionsFor(item.Data.State).Select(a => a.Name),
                UserName = userName
            };
        }
    }
}
