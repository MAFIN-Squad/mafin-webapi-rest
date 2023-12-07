using System.Net.Http.Headers;

namespace Mafin.Web.Api.Rest;

/// <summary>
/// Represents API response with content data.
/// </summary>
/// <typeparam name="T">Content type.</typeparam>
/// <remarks>
/// Initializes a new instance of the <see cref="EndpointResponse{T}"/> class.
/// </remarks>
/// <param name="httpResponseMessage">HTTP response message.</param>
/// <param name="content">HTTP response message content.</param>
public class EndpointResponse<T>(HttpResponseMessage httpResponseMessage, T? content)
    : EndpointResponse(httpResponseMessage), IEndpointResponseContent<T>
{
    /// <inheritdoc/>
    public T? Content { get; } = content;

    /// <inheritdoc/>
    public HttpContentHeaders? ContentHeaders => Response.Content?.Headers;
}
