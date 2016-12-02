using EPiServer.Core;
using EPiServer.PlugIn;
using Newtonsoft.Json;

namespace EPiServer.SocialAlloy.Web.Social.Blocks
{
    [PropertyDefinitionTypePlugIn]
    public class RatingSettingProperty : PropertyList<RatingSetting>
    {
        protected override RatingSetting ParseItem(string value)
        {
            return JsonConvert.DeserializeObject<RatingSetting>(value);
        }
        public override PropertyData ParseToObject(string value)
        {
            ParseToSelf(value);
            return this;
        }
    }
}