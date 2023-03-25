using System.Text.Json;
using Moq;
using Moq.Protected;

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

    [Fact]
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
    }

    [Fact]
    public void WithAuthHandler_WhenCustomizationActionPassed_ShouldInvokeAction()
    {
        Mock<Action<HttpClientHandler>> actionMock = new();
        actionMock.Setup(m => m(It.IsAny<HttpClientHandler>())).Verifiable();

        _builder = new MafinHttpClientBuilder(Url);

        _ = _builder.WithAuthHandler(actionMock.Object).Build();

        actionMock.Verify(m => m(It.IsAny<HttpClientHandler>()), Times.Once());
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
        Mock<Action<JsonSerializerOptions>> actionMock = new();
        actionMock.Setup(m => m(It.IsAny<JsonSerializerOptions>())).Verifiable();

        _builder = new MafinHttpClientBuilder(Url);

        _ = _builder.WithJsonSerializerOptions(actionMock.Object).Build();

        actionMock.Verify(m => m(It.IsAny<JsonSerializerOptions>()), Times.Once());
    }

    [Fact]
    public void WithJsonSerializerOptions_WhenCustomizationActionForExistingOptionsPassed_ShouldInvokeActionWithSameOptions()
    {
        JsonSerializerOptions options = new();
        Mock<Action<JsonSerializerOptions>> actionMock = new();
        actionMock.Setup(m => m(It.IsAny<JsonSerializerOptions>())).Verifiable();

        _builder = new MafinHttpClientBuilder(Url);

        _ = _builder.WithJsonSerializerOptions(options).WithJsonSerializerOptions(actionMock.Object).Build();

        actionMock.Verify(m => m(options), Times.Once());
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
