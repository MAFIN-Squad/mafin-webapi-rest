using System.Net.Http.Headers;

namespace Mafin.Web.Api.Rest;

/// <summary>
/// Interface representing API response content data.
/// </summary>
/// <typeparam name="T">Content type.</typeparam>
public interface IEndpointResponseContent<out T>
{
    /// <summary>
    /// Gets deserialized content as <typeparamref name="T"/>.
    /// </summary>
    T? Content { get; }

    /// <summary>
    /// Gets the HTTP content headers as defined in RFC 2616.
    /// </summary>
    HttpContentHeaders? ContentHeaders { get; }
}
