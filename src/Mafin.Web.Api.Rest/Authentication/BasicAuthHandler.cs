using System.Net.Http.Headers;
using System.Text;

namespace Mafin.Web.Api.Rest.Authentication;

/// <summary>
/// HTTP basic Authentication handling type.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="BasicAuthHandler"/> class.
/// </remarks>
/// <param name="userName">User name value.</param>
/// <param name="password">User password value.</param>
public class BasicAuthHandler(string userName, string password) : BaseAuthHandler
{
    /// <inheritdoc cref="BaseAuthHandler.GetAuthHeaderValue"/>
    protected override AuthenticationHeaderValue GetAuthHeaderValue() =>
        new("Basic", Convert.ToBase64String(Encoding.Default.GetBytes($"{userName}:{password}")));
}
