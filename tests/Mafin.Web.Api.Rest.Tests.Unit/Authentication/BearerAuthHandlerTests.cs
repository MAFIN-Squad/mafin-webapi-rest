using System.Net;
using System.Net.Http.Headers;
using AutoFixture;
using Mafin.Web.Api.Rest.Authentication;
using Moq;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace Mafin.Web.Api.Rest.Tests.Unit.Authentication;

public class BearerAuthHandlerTests
{
    private readonly Fixture _fixture = new();
    private readonly Mock<IBearerTokenProvider> _mockBearerTokenProvider = new();

    [Fact]
    public async Task SendAsync_WhenPassedTokenProvider_ShouldSetHeader()
    {
        var token = _fixture.Create<string>();
        _mockBearerTokenProvider.Setup(x => x.GetBearerToken()).Returns(token);
        AuthenticationHeaderValue expectedHeader = new("Bearer", token);
        using HttpRequestMessage requestMessage = new()
        {
            RequestUri = new Uri(GetMockedUrl()!)
        };

        using TestBearerAuthHandler handler = new(_mockBearerTokenProvider.Object);
        _ = await handler.PublicSendAsync(requestMessage).ConfigureAwait(true);

        requestMessage.Headers.Authorization.Should().BeEquivalentTo(expectedHeader);
    }

    private static string? GetMockedUrl()
    {
        var server = WireMockServer.Start();
        server.Given(Request.Create().UsingGet())
            .RespondWith(Response.Create().WithStatusCode(HttpStatusCode.OK));

        return server.Url;
    }
}

#pragma warning disable SA1402 // File may only contain a single type
public class TestBearerAuthHandler(IBearerTokenProvider tokenProvider) : BearerAuthHandler(tokenProvider)
#pragma warning restore SA1402
{
    public Task<HttpResponseMessage> PublicSendAsync(HttpRequestMessage request) => SendAsync(request, CancellationToken.None);
}
