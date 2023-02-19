using System.Net.Http.Headers;
using System.Text;

namespace Mafin.Web.Api.Rest.Authentication;

/// <summary>
/// HTTP basic Authentication handling type.
/// </summary>
public class BasicAuthHandler : BaseAuthHandler
{
    private readonly string _username;
    private readonly string _password;

    /// <summary>
    /// Initializes a new instance of the <see cref="BasicAuthHandler"/> class.
    /// </summary>
    /// <param name="username">Username value.</param>
    /// <param name="password">user password value.</param>
    public BasicAuthHandler(string username, string password)
    {
        _username = username;
        _password = password;
    }

    /// <inheritdoc cref="BaseAuthHandler.GetAuthHeaderValue"/>
    protected override AuthenticationHeaderValue GetAuthHeaderValue() =>
        new("Basic", Convert.ToBase64String(Encoding.Default.GetBytes($"{_username}:{_password}")));
}
