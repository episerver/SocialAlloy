namespace EPiServer.SocialAlloy.Web.Social.Common.Models
{
    /// <summary>
    /// Used to provide simple messaging to the user in the view
    /// </summary>
    public class MessageViewModel
    {
        /// <summary>
        /// The type of message that will be displayed to the user
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// The body of the message that will be displayed to the user
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Resolves what style color should be displayed to a user in the view
        /// </summary>
        /// <param name="messageType">The type of message that is being resolved</param>
        /// <returns>The color associated with the provided message type</returns>
        public string ResolveStyle(string messageType)
        {
            switch(messageType)
            {
                case "success":
                    return "green";
                case "error":
                    return "red";
                default:
                    return "black";
            }
        }
    }
}