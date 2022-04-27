namespace DigitalHealthCheckCommon
{
    /// <summary>
    /// Interface for objects that serialise JSON from specified types.
    /// </summary>
    /// <typeparam name="T">The type that this serializer can serialize.</typeparam>
    public interface IJsonSerializer<T>
    {
        /// <summary>
        /// Serializes the specified item.
        /// </summary>
        /// <param name="item">The item to serialize to JSON.</param>
        /// <returns>A JSON string matching the supplied item.</returns>
        string Serialize(T item);
    }
}