using System.Globalization;
using System.Reflection;
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
        const string sendMethodName = "Send";

        var authHandlerMock = Substitute.For<HttpClientHandler>();
        var requestMock = Substitute.For<HttpRequestMessage>();
        var responseMock = Substitute.For<HttpResponseMessage>();

        this.InvokeHandlerMethod(sendMethodName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.InvokeMethod, null, authHandlerMock, new object[] { requestMock, Arg.Any<CancellationToken>() }, CultureInfo.InvariantCulture).Returns(responseMock);

        _builder = new MafinHttpClientBuilder(Url);
        var client = _builder.WithAuthHandler(authHandlerMock).Build();

        client.Send(requestMock);

        Received.InOrder(() =>
        {
            this.InvokeHandlerMethod(sendMethodName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.InvokeMethod, null, authHandlerMock, new object[] { requestMock, Arg.Any<CancellationToken>() }, CultureInfo.InvariantCulture);
        });
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

    private object? InvokeHandlerMethod(string methodName, BindingFlags flag, Binder? binder, HttpClientHandler authHandlerMock, object?[] objects, CultureInfo cultureInfo) =>
        typeof(HttpClientHandler).InvokeMember(methodName, flag, binder, authHandlerMock, objects, cultureInfo);
}
