using System.Net.Http.Headers;

namespace Mafin.Web.Api.Rest.Authentication;

/// <summary>
/// Bearer Authentication handling type.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="BearerAuthHandler"/> class.
/// </remarks>
/// <param name="tokenProvider">Bearer token provider implementation.</param>
public class BearerAuthHandler(IBearerTokenProvider tokenProvider) : BaseAuthHandler
{
    /// <inheritdoc cref="BaseAuthHandler.GetAuthHeaderValue"/>
    protected override AuthenticationHeaderValue GetAuthHeaderValue() => new("Bearer", tokenProvider.GetBearerToken());
}
