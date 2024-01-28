using System.Runtime.CompilerServices;
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

    [Fact]
    public void WithAuthHandler_WhenHandlerPassed_ShouldDelegateHttpCallToSameHandler()
    {
        var authHandlerMock = Substitute.For<HttpClientHandler>();
        var requestMock = Substitute.For<HttpRequestMessage>();
        var responseMock = Substitute.For<HttpResponseMessage>();

        InvokeHandlerMethod(authHandlerMock, requestMock, Arg.Any<CancellationToken>()).Returns(responseMock);

        _builder = new MafinHttpClientBuilder(Url);
        var client = _builder.WithAuthHandler(authHandlerMock).Build();

        client.Send(requestMock);

        Received.InOrder(() =>
        {
            InvokeHandlerMethod(authHandlerMock, requestMock, Arg.Any<CancellationToken>());
        });
    }

    [Fact]
    public void WithAuthHandler_WhenNullHandler_ShouldThrow()
    {
        _builder = new MafinHttpClientBuilder(Url);

        var action = () => _builder.WithAuthHandler((HttpClientHandler)null!);

        action.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'handler')");
    }

    [Fact]
    public void WithAuthHandler_WhenCustomizationActionPassed_ShouldInvokeAction()
    {
        var actionMock = Substitute.For<Action<HttpClientHandler>>();

        _builder = new MafinHttpClientBuilder(Url);

        _ = _builder.WithAuthHandler(actionMock).Build();

        actionMock.Received(1).Invoke(Arg.Any<HttpClientHandler>());
    }

    [Fact]
    public void WithAuthHandler_WhenNullCustomizationAction_ShouldThrow()
    {
        _builder = new MafinHttpClientBuilder(Url);

        var action = () => _builder.WithAuthHandler((Action<HttpClientHandler>)null!);

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
    public void WithJsonSerializerOptions_WhenNullOptions_ShouldThrow()
    {
        _builder = new MafinHttpClientBuilder(Url);

        var action = () => _builder.WithJsonSerializerOptions((JsonSerializerOptions)null!);

        action.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'options')");
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
    public void WithJsonSerializerOptions_WhenNullCustomizationAction_ShouldThrow()
    {
        _builder = new MafinHttpClientBuilder(Url);

        var action = () => _builder.WithJsonSerializerOptions((Action<JsonSerializerOptions>)null!);

        action.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'serializerCustomizationAction')");
    }

    [UnsafeAccessor(UnsafeAccessorKind.Method, Name = "Send")]
    private static extern HttpResponseMessage? InvokeHandlerMethod(HttpClientHandler authHandlerMock, HttpRequestMessage request, CancellationToken cancellationToken);
}
