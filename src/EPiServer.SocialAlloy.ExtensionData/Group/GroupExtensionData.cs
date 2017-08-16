using EPiServer.Social.Common;

namespace EPiServer.SocialAlloy.ExtensionData.Group
{
    /// <summary>
    /// The GroupExtensionData is a serializable class representing 
    /// the encoded url of a group community page.
    /// </summary>
    [ExtensionData("6d71b45c-92f7-497c-a49e-8fa517cb46d9")]
    public class GroupExtensionData
    {
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="pageLink"></param>
        public GroupExtensionData(string pageLink)
        {
            PageLink = pageLink;
        }

        /// <summary>
        /// The encoded absolute uri of a group community page.  
        /// </summary>
        public string  PageLink{ get; set; }
    }
}
