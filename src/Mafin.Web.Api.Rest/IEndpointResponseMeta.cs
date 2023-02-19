using System.Net;
using System.Net.Http.Headers;

namespace Mafin.Web.Api.Rest;

/// <summary>
/// Interface representing API response meta data.
/// </summary>
internal interface IEndpointResponseMeta : IDisposable
{
    /// <summary>
    /// Gets HTTP response headers.
    /// </summary>
    HttpResponseHeaders Headers { get; }

    /// <summary>
    /// Gets HTTP response status code.
    /// </summary>
    HttpStatusCode StatusCode { get; }

    /// <summary>
    /// Gets the reason phrase which typically is sent by servers together with the status code.
    /// </summary>
    string? ReasonPhrase { get; }

    /// <summary>
    /// Gets the request message which led to this response message.
    /// </summary>
    HttpRequestMessage? RequestMessage { get; }
}
