using Newtonsoft.Json;

namespace DigitalHealthCheckCommon
{
    /// <summary>
    /// Helper that uses Newtonsoft.JSON to serialise and deserialise objects of a specific type.
    /// </summary>
    /// <typeparam name="T">The type that this serializer will serialise and deserialize.</typeparam>
    /// <seealso cref="IJsonDeserializer{T}"/>
    /// <seealso cref="IJsonSerializer{T}"/>
    public class NewtonsoftJsonSerializationWrapper<T> : IJsonDeserializer<T>, IJsonSerializer<T>
    {
        static JsonSerializerSettings JsonSettings => new()
        {
            DateFormatString = "dd/MM/yyyy"
        };

        /// <summary>
        /// Deserializes the specified json.
        /// </summary>
        /// <param name="json">The json.</param>
        /// <returns>An object of type T matching the serialised JSON.</returns>
        public T Deserialize(string json) =>
            JsonConvert.DeserializeObject<T>(json, JsonSettings);

        /// <summary>
        /// Serializes the specified item.
        /// </summary>
        /// <param name="item">The item to serialize to JSON.</param>
        /// <returns>A JSON string matching the supplied item.</returns>
        public string Serialize(T item) =>
            JsonConvert.SerializeObject(item, JsonSettings);
    }
}