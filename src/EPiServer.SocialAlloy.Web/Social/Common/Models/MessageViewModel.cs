namespace EPiServer.SocialAlloy.Web.Social.Common.Models
{
    public class MessageViewModel
    {
        public string Type { get; set; }
        public string Body { get; set; }

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