namespace EPiServer.SocialAlloy.Web.Social.Models.Moderation
{
    /// <summary>
    /// The SocialWorkflowState defines the workflow states that workflows can be in within the SocialAlloy sample
    /// </summary>
    public class SocialWorkflowState
    {
        public SocialWorkflowState(string name)
        {
            Name = name;
        }

        /// <summary>
        /// The name of the state that the workflow is in
        /// </summary>
        public string Name { get; set; }
    }
}