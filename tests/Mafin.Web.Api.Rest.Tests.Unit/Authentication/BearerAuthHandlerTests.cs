using System.Net;
using System.Net.Http.Headers;
using AutoFixture;
using Mafin.Web.Api.Rest.Authentication;
using NSubstitute;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace Mafin.Web.Api.Rest.Tests.Unit.Authentication;

public class BearerAuthHandlerTests
{
    private readonly Fixture _fixture = new();
    private readonly IBearerTokenProvider _bearerTokenProviderMock = Substitute.For<IBearerTokenProvider>();

    [Fact]
    public async Task SendAsync_WhenPassedTokenProvider_ShouldSetHeader()
    {
        var token = _fixture.Create<string>();
        _ = _bearerTokenProviderMock.GetBearerToken().Returns(token);
        AuthenticationHeaderValue expectedHeader = new("Bearer", token);
        using HttpRequestMessage requestMessage = new()
        {
            RequestUri = new Uri(GetMockedUrl())
        };

        using TestBearerAuthHandler handler = new(_bearerTokenProviderMock);
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

    private sealed class TestBearerAuthHandler(IBearerTokenProvider tokenProvider) : BearerAuthHandler(tokenProvider)
    {
        public Task<HttpResponseMessage> PublicSendAsync(HttpRequestMessage request) => SendAsync(request, CancellationToken.None);
    }
}
