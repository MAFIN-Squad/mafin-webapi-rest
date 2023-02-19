using System.Net.Http.Headers;

namespace Mafin.Web.Api.Rest.Authentication;

/// <summary>
/// Bearer Authentication handling type.
/// </summary>
public class BearerAuthHandler : BaseAuthHandler
{
    private readonly IBearerTokenProvider _tokenProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="BearerAuthHandler"/> class.
    /// </summary>
    /// <param name="tokenProvider">Bearer token provider implementation.</param>
    public BearerAuthHandler(IBearerTokenProvider tokenProvider)
    {
        _tokenProvider = tokenProvider;
    }

    /// <inheritdoc cref="BaseAuthHandler.GetAuthHeaderValue"/>
    protected override AuthenticationHeaderValue GetAuthHeaderValue() =>
        new("Bearer", _tokenProvider.GetBearerToken());
}
