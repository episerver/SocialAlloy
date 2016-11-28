using EPiServer.Social.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;

namespace EPiServer.SocialAlloy.Web.Social.Common.Extensions
{
    public static class ReferenceExtensions
    {
        ///// <summary>
        ///// Serializes an object to a base64 string and returns that string encapsulated as an instance of the Reference class.
        ///// </summary>
        ///// <returns>Returns an instance of the Reference class whose reference string has been serialized and converted into a Base64 string</returns>
        public static Reference ToReference(this object key)
        {
            var formatter = new BinaryFormatter();

            try
            {
                using (var stream = new MemoryStream())
                {
                    formatter.Serialize(stream, key);
                    stream.Seek(0, SeekOrigin.Begin);
                    return Reference.Create(Convert.ToBase64String(stream.ToArray()));
                }
            }
            catch
            {
                return Reference.Empty;
            }
        }

        ///// <summary>
        ///// Deserializes the underlying reference string of a Reference instance to an object.
        //// If the underlying reference string is not base64 formatted and/or cannot be deserialized correctly as an object, an empty string is returned.
        //// If the Reference instance is Empty, an empty string is returned.
        ///// </summary>
        ///// <returns>An deserialized object of the appropriate type based on the serialized and base64-formatted reference string of the Reference instance.</returns>
        public static object ToProviderUserKey(this Reference reference)
        {
            if (Reference.IsNullOrEmpty(reference) ||
                string.IsNullOrEmpty(reference.Id))
                return String.Empty;

            try
            {
                using (var stream = new MemoryStream(Convert.FromBase64String(reference.Id)))
                {
                    stream.Seek(0, SeekOrigin.Begin);

                    var formatter = new BinaryFormatter();

                    return (object)formatter.Deserialize(stream);
                }
            }
            catch
            {
                return String.Empty;
            }
        }
    }
}