namespace EPiServer.SocialAlloy.Web.Social.Models.Moderation
{
    /// <summary>
    /// The CommunityMembershipWorkflow describes a workflow supporting the moderation of community membership.
    /// </summary>
    public class CommunityMembershipWorkflow
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">The unique id of the workflow</param>
        /// <param name="name">The name of the workflow</param>
        /// <param name="initialState">The initial state that community membership request will be entered into when entering the workflow</param>
        public CommunityMembershipWorkflow(string id, string name, string initialState )
        {
            this.Id = id;
            this.Name = name;
            this.InitialState = initialState;
        }

        /// <summary>
        /// Gets and sets the unique id of the workflow
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the initial state of a workflow item that enters this social workflow
        /// </summary>
        public string InitialState { get; set; }

        /// <summary>
        /// Gets or sets the name of the workflow.
        /// </summary>
        public string Name { get; set; }
    }
}