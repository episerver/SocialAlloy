namespace EPiServer.SocialAlloy.Web.Social.Models.Moderation
{
    /// <summary>
    /// The SocialWorkflowTransition defines the workflow transitions that workflow items might experience while going through a workflow
    /// </summary>
    public class SocialWorkflowTransition
    {
        public SocialWorkflowTransition(SocialWorkflowState initialState, SocialWorkflowState secondaryState, SocialWorkflowAction action)
        {
            InitialState = initialState;
            SecondaryState = secondaryState;
            Action = action;
        }

        /// <summary>
        /// The ititial state that a workflow item will be in before the action
        /// </summary>
        public SocialWorkflowState InitialState { get; set; }

        /// <summary>
        /// The secondary state that a workflow item will be in after the action
        /// </summary>
        public SocialWorkflowState SecondaryState { get; set; }

        /// <summary>
        /// The action used to move a workflow item from its initial state to its secondary state
        /// </summary>
        public SocialWorkflowAction Action { get; set; }

    }
}