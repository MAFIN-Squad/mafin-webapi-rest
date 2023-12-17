using System.Text;
using System.Text.Json;

namespace Mafin.Web.Api.Rest.Extensions;

/// <summary>
/// Provides extensions for (de-)serialization process.
/// </summary>
public static class HttpContextExtensions
{
    /// <summary>
    /// Performs de-serialization of JSON content to a specified entity type.
    /// </summary>
    /// <typeparam name="T">Entity type.</typeparam>
    /// <param name="response">REST API response message.</param>
    /// <param name="options">JSON serializer configuration instance.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="response"/> is null.</exception>
    public static async Task<T?> AsEntity<T>(this HttpResponseMessage response, JsonSerializerOptions? options = null)
    {
        if (response is null)
        {
            throw new ArgumentNullException(nameof(response));
        }

        var jsonContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        return string.IsNullOrWhiteSpace(jsonContent)
            ? default
            : JsonSerializer.Deserialize<T>(jsonContent, options);
    }

    /// <summary>
    /// Performs entity serialization to JSON format.
    /// </summary>
    /// <typeparam name="T">Entity type.</typeparam>
    /// <param name="entity">Entity instance.</param>
    /// <param name="options">JSON serializer configuration instance.</param>
    /// <returns><see cref="StringContent"/> representing JSON object.</returns>
    public static StringContent ToJson<T>(this T entity, JsonSerializerOptions? options = null) =>
        new(JsonSerializer.Serialize(entity, options), Encoding.UTF8, "application/json");
}
