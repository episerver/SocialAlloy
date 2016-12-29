using EPiServer.Social.Common;
using EPiServer.Social.Groups.Core;
using EPiServer.Social.Moderation.Core;
using EPiServer.SocialAlloy.Web.Social.Models.Groups;
using EPiServer.SocialAlloy.Web.Social.Models.Moderation;

namespace EPiServer.SocialAlloy.Web.Social.Repositories
{
    public interface ISocialModerationRepository
    {

        void Add(SocialWorkflow workflow, MembershipModeration membershipModeration);

        void Add(SocialWorkflowItem socialWorkflow, AddMemberRequest membershipRequest);

        /// <summary>
        /// Returns a view model supporting the presentation of group
        /// membership moderation information.
        /// </summary>
        /// <param name="workflowId">Identifier for the selected membership moderation workflow</param>
        /// <returns>View model of moderation information</returns>
        ModerationViewModel Get(string workflowId);

        /// <summary>
        /// Retrieves relevant membership infromation needed to allow for a workflow item to be approved 
        /// </summary>
        /// <param name="user">The user reference for the member requesting group admission</param>
        /// <param name="group">The group id for the group the user is looking to gain admission</param>
        /// <returns></returns>
        Composite<SocialWorkflowItem, AddMemberRequest> Get(string user, string group);

        /// <summary>
        /// Takes action on the specified workflow item, representing a
        /// membership request.
        /// </summary>
        /// <param name="itemToActUpon">Membership request item to act upon</param>
        /// <param name="action">Moderation action to be taken</param>
        void Moderate(MembershipRequestModel itemToActUpon, string action);

        /// <summary>
        /// Returns the moderation workflow supporting the specified group.
        /// </summary>
        /// <param name="group">ID of the group</param>
        /// <returns>Moderation workflow supporting the specified group</returns>
        Workflow GetWorkflowFor(GroupId group);

        /// <summary>
        /// Returns true if the specified group has a moderation workflow,
        /// false otherwise.
        /// </summary>
        /// <param name="group">ID of the group</param>
        /// <returns>True if the specified group has a moderation workflow, false otherwise</returns>
        bool IsModerated(GroupId group);
    }
}
