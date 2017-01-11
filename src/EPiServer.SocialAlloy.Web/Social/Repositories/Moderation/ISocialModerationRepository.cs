using EPiServer.SocialAlloy.Web.Social.Models;
using EPiServer.SocialAlloy.Web.Social.Models.Groups;
using EPiServer.SocialAlloy.Web.Social.Models.Moderation;

namespace EPiServer.SocialAlloy.Web.Social.Repositories
{
    public interface ISocialModerationRepository
    {
        /// <summary>
        /// Adds a new workflow to the underlying repository for a specified group. 
        /// </summary>
        /// <param name="group">The group that will be associated with the workflow</param>
        void Add(SocialGroup group);

        /// <summary>
        /// Adds a new workflowitem to the underlying repository
        /// </summary>
        /// <param name="socialWorkflow">The workflow intem to be added</param>
        /// <param name="membershipRequest">Extension data containing member details that will be needed if member admission is approved</param>
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
        /// <returns>The workflowitem extension data</returns>
        AddMemberRequest Get(string user, string group);

        /// <summary>
        /// Takes action on the specified workflow item, representing a
        /// membership request.
        /// </summary>
        /// <param name="workflowId">The id of the workflow </param>
        /// <param name="socialMember">The member that under moderation for group admission</param>
        /// <param name="memberExtensionData">The extension data for a member under moderation  </param>
        /// <param name="action">The moderation action to be taken</param>
        void Moderate(string workflowId, SocialMember socialMember, MemberExtensionData memberExtensionData, string action);

        /// <summary>
        /// Returns the moderation workflow supporting the specified group.
        /// </summary>
        /// <param name="groupId">The Id of the group that a workflow is associated with</param>
        /// <returns>Moderation workflow supporting the specified group</returns>
        SocialWorkflow GetWorkflowFor(string groupId);

        /// <summary>
        /// Returns true if the specified group has a moderation workflow,
        /// false otherwise.
        /// </summary>
        /// <param name="groupId">ID of the group</param>
        /// <returns>True if the specified group has a moderation workflow, false otherwise</returns>
        bool IsModerated(string groupId);

        /// <summary>
        /// Creates a unique uri to be used with to track the progression of a member being moderated for group admission 
        /// </summary>
        /// <param name="group">Id of the group that a member is trying to join</param>
        /// <param name="user">The name of the member that is trying to join a group</param>
        /// <returns></returns>
        string CreateUri(string group, string user);
    }
}
