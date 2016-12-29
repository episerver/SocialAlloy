namespace EPiServer.SocialAlloy.Web.Social.Models.Moderation
{
    /// <summary>
    /// The SocialWorkflowAction defines the workflow action details necessary to act on a workflow item within the SocialAlloy sample
    /// </summary>
    public class SocialWorkflowAction
    {
        public SocialWorkflowAction(string name)
        {
            Name = name;
        }

        /// <summary>
        /// The name of the action 
        /// </summary>
        public string Name { get; set; }
    }
}