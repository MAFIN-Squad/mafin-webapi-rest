using System.Net.Http.Headers;

namespace Mafin.Web.Api.Rest;

/// <summary>
/// Represents API response with content data.
/// </summary>
/// <typeparam name="T">Content type.</typeparam>
public class EndpointResponse<T> : EndpointResponse, IEndpointResponseContent<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EndpointResponse{T}"/> class.
    /// </summary>
    /// <param name="httpResponseMessage">HTTP response message.</param>
    /// <param name="content">HTTP response message content.</param>
    public EndpointResponse(HttpResponseMessage httpResponseMessage, T? content)
        : base(httpResponseMessage)
    {
        Content = content;
    }

    /// <inheritdoc/>
    public T? Content { get; }

    /// <inheritdoc/>
    public HttpContentHeaders? ContentHeaders => Response.Content?.Headers;
}
