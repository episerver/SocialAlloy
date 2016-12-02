using EPiServer.Core;
using EPiServer.PlugIn;
using Newtonsoft.Json;

namespace EPiServer.SocialAlloy.Web.Social.Blocks
{
    /// <summary>
    /// This class maps the RatingSetting type to a property definition type so it can be 
    /// used by the rating block.
    /// </summary>
    [PropertyDefinitionTypePlugIn]
    public class RatingSettingProperty : PropertyList<RatingSetting>
    {
        /// <summary>
        /// Overrides the base implementation of this method to 
        /// parse a string to an instance of the RatingSetting object.
        /// </summary>
        /// <param name="value">the string representation to parse to an instance of RatingSetting</param>
        /// <returns>an instance of the RatingSetting object</returns>
        protected override RatingSetting ParseItem(string value)
        {
            return JsonConvert.DeserializeObject<RatingSetting>(value);
        }

        /// <summary>
        /// Overrides the base implementation of this method to provide property data.
        /// </summary>
        /// <param name="value">the string representation to parse</param>
        /// <returns>this RatingSettingProperty instance</returns>
        public override PropertyData ParseToObject(string value)
        {
            ParseToSelf(value);
            return this;
        }
    }
}