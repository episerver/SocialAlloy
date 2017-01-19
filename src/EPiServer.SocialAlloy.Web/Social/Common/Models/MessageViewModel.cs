namespace EPiServer.SocialAlloy.Web.Social.Common.Models
{
    /// <summary>
    /// Used to provide simple messaging to the user in the view
    /// </summary>
    public class MessageViewModel
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="body">The message body</param>
        /// <param name="type">The message type</param>
        public MessageViewModel(string body, string type)
        {
            Body = body;
            Type = type;
        }

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
                case "Success":
                    return "green";
                case "Error":
                    return "red";
                default:
                    return "black";
            }
        }
    }
}