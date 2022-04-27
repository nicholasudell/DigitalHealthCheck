namespace DigitalHealthCheckCommon
{
    /// <summary>
    /// Interface for objects that deserialise JSON into specified types.
    /// </summary>
    /// <typeparam name="T">The type that is produced after deserialising JSON.</typeparam>
    public interface IJsonDeserializer<T>
    {
        /// <summary>
        /// Deserializes the specified json.
        /// </summary>
        /// <param name="json">The json.</param>
        /// <returns>An object of type T matching the serialised JSON.</returns>
        T Deserialize(string json);
    }
}