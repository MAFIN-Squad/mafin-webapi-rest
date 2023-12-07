using System.Net;
using System.Net.Http.Headers;

namespace Mafin.Web.Api.Rest;

/// <summary>
/// Represents API response without content.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="EndpointResponse"/> class.
/// </remarks>
/// <param name="httpResponseMessage">HTTP response message.</param>
public class EndpointResponse(HttpResponseMessage httpResponseMessage) : IEndpointResponseMeta
{
    /// <inheritdoc/>
    public HttpResponseHeaders Headers => Response.Headers;

    /// <inheritdoc/>
    public HttpStatusCode StatusCode => Response.StatusCode;

    /// <inheritdoc/>
    public string? ReasonPhrase => Response.ReasonPhrase;

    /// <inheritdoc/>
    public HttpRequestMessage? RequestMessage => Response.RequestMessage;

    /// <summary>
    /// Gets HTTP response message.
    /// </summary>
    protected HttpResponseMessage Response { get; } = httpResponseMessage;

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    ///  Releases the resources used by <see cref="HttpResponseMessage"/>.
    /// </summary>
    /// <param name="disposing">Whether to dispose resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            Response.Dispose();
        }
    }
}
