namespace Mafin.Web.Api.Rest.Authentication;

/// <summary>
/// Interface providing capability of retrieving bearer authentication token.
/// </summary>
public interface IBearerTokenProvider
{
    /// <summary>
    /// Retrieves Bearer authentication token.
    /// </summary>
    /// <returns>Value of bearer token.</returns>
    string GetBearerToken();
}
