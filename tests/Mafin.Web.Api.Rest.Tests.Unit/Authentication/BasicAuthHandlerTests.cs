using System.Net;
using System.Net.Http.Headers;
using System.Text;
using AutoFixture;
using Mafin.Web.Api.Rest.Authentication;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace Mafin.Web.Api.Rest.Tests.Unit.Authentication;

public class BasicAuthHandlerTests
{
    private readonly Fixture _fixture = new();

    [Fact]
    public async Task SendAsync_WhenPassedCredentials_ShouldSetHeader()
    {
        var userName = _fixture.Create<string>();
        var password = _fixture.Create<string>();
        AuthenticationHeaderValue expectedHeader = new("Basic", Convert.ToBase64String(Encoding.Default.GetBytes($"{userName}:{password}")));
        using HttpRequestMessage requestMessage = new()
        {
            RequestUri = new Uri(GetMockedUrl())
        };

        using TestBasicAuthHandler handler = new(userName, password);
        _ = await handler.PublicSendAsync(requestMessage);

        requestMessage.Headers.Authorization.Should().BeEquivalentTo(expectedHeader);
    }

    private static string GetMockedUrl()
    {
        var server = WireMockServer.Start();
        server.Given(Request.Create().UsingGet())
            .RespondWith(Response.Create().WithStatusCode(HttpStatusCode.OK));

        return server.Url!;
    }

    private sealed class TestBasicAuthHandler(string userName, string password) : BasicAuthHandler(userName, password)
    {
        public Task<HttpResponseMessage> PublicSendAsync(HttpRequestMessage request) => SendAsync(request, CancellationToken.None);
    }
}
