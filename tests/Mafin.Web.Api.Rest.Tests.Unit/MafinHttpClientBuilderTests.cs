using System.Text.Json;
using NSubstitute;

namespace Mafin.Web.Api.Rest.Tests.Unit;

public class MafinHttpClientBuilderTests
{
    private const string Url = "https://example.local";

    private MafinHttpClientBuilder? _builder;

    [Fact]
    public void Constructor_WhenStringBaseAddress_ShouldSetHttpClientBaseAddress()
    {
        _builder = new MafinHttpClientBuilder(Url);

        var client = _builder.Build();

        client.BaseAddress.Should().Be(new Uri(Url));
    }

    [Fact]
    public void Constructor_WhenUriBaseAddress_ShouldSetHttpClientBaseAddress()
    {
        Uri uri = new(Url);
        _builder = new MafinHttpClientBuilder(uri);

        var client = _builder.Build();

        client.BaseAddress.Should().Be(uri);
    }

    // Note that WithAuthHandler_WhenHandlerPassed_ShouldDelegateHttpCallToSameHandler could not be re-written due to NSubstitute limitation
    /*[Fact]
    public void WithAuthHandler_WhenHandlerPassed_ShouldDelegateHttpCallToSameHandler()
    {
        const string sendMethodName = "Send";

        Mock<HttpClientHandler> authHandlerMock = new();
        Mock<HttpRequestMessage> requestMock = new();

        authHandlerMock.Protected()
            .Setup<HttpResponseMessage>(sendMethodName, requestMock.Object, ItExpr.IsAny<CancellationToken>())
            .Returns(new Mock<HttpResponseMessage>().Object)
            .Verifiable();

        _builder = new MafinHttpClientBuilder(Url);
        var client = _builder.WithAuthHandler(authHandlerMock.Object).Build();

        _ = client.Send(requestMock.Object);

        authHandlerMock.Protected().Verify(sendMethodName, Times.Once(), requestMock.Object, ItExpr.IsAny<CancellationToken>());
    }*/

    [Fact]
    public void WithAuthHandler_WhenCustomizationActionPassed_ShouldInvokeAction()
    {
        var actionMock = Substitute.For<Action<HttpClientHandler>>();

        _builder = new MafinHttpClientBuilder(Url);

        _ = _builder.WithAuthHandler(actionMock).Build();

        actionMock.Received(1).Invoke(Arg.Any<HttpClientHandler>());
    }

    [Fact]
    public void WithAuthHandler_WnenNullCustomizationAction_ShouldThrow()
    {
        Action<HttpClientHandler> authAction = null!;
        _builder = new MafinHttpClientBuilder(Url);

        Action action = () => _builder.WithAuthHandler(authAction).Build();

        action.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'authCustomizationAction')");
    }

    [Fact]
    public void WithJsonSerializerOptions_WhenOptionsPassed_ShouldSetMafinHttpClientOptions()
    {
        JsonSerializerOptions options = new() { MaxDepth = 5, IgnoreReadOnlyFields = true };
        _builder = new MafinHttpClientBuilder(Url);

        var client = _builder.WithJsonSerializerOptions(options).Build();

        client.JsonSerializerOptions.Should().Be(options);
    }

    [Fact]
    public void WithJsonSerializerOptions_WhenCustomizationActionPassed_ShouldInvokeAction()
    {
        var actionMock = Substitute.For<Action<JsonSerializerOptions>>();

        _builder = new MafinHttpClientBuilder(Url);

        _ = _builder.WithJsonSerializerOptions(actionMock).Build();

        actionMock.Received(1).Invoke(Arg.Any<JsonSerializerOptions>());
    }

    [Fact]
    public void WithJsonSerializerOptions_WhenCustomizationActionForExistingOptionsPassed_ShouldInvokeActionWithSameOptions()
    {
        JsonSerializerOptions options = new();
        var actionMock = Substitute.For<Action<JsonSerializerOptions>>();

        _builder = new MafinHttpClientBuilder(Url);

        _ = _builder.WithJsonSerializerOptions(options).WithJsonSerializerOptions(actionMock).Build();

        actionMock.Received(1).Invoke(options);
    }

    [Fact]
    public void WithJsonSerializerOptions_WnenNullCustomizationAction_ShouldThrow()
    {
        Action<JsonSerializerOptions> optionsAction = null!;
        _builder = new MafinHttpClientBuilder(Url);

        Action action = () => _builder.WithJsonSerializerOptions(optionsAction).Build();

        action.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'serializerCustomizationAction')");
    }
}
