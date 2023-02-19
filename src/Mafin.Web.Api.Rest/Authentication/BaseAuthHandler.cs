using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;

namespace Mafin.Web.Api.Rest.Authentication;

/// <summary>
/// Authentication handling base type.
/// </summary>
public abstract class BaseAuthHandler : HttpClientHandler
{
    /// <inheritdoc cref="HttpClientHandler.SendAsync"/>
    protected override Task<HttpResponseMessage> SendAsync([NotNull] HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.Authorization = GetAuthHeaderValue();
        return base.SendAsync(request, cancellationToken);
    }

    /// <summary>
    /// Resolves the way of retrieving Authorization header value.
    /// </summary>
    /// <returns>Value for request Authorization header.</returns>
    protected abstract AuthenticationHeaderValue GetAuthHeaderValue();
}
