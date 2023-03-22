using FluentAssertions.Execution;
using Mafin.Web.Api.Rest.Extensions;
using Moq;

namespace Mafin.Web.Api.Rest.Tests.Unit.Extensions;

public class HttpMessageHandlerExtensionsTests
{
    [Fact]
    public void WrapInto_WhenHandlerPassed_ShouldWrapIntoInnerHandler()
    {
        using SocketsHttpHandler handler = new();
        Mock<DelegatingHandler> delegatingHandlerMock = new();

        var resultingHandler = handler.WrapInto(new[] { delegatingHandlerMock.Object });

        resultingHandler.Should().BeAssignableTo<DelegatingHandler>();
        resultingHandler.As<DelegatingHandler>().InnerHandler.Should().BeEquivalentTo(handler);
    }

    [Fact]
    public void WrapInto_WhenMultipleHandlersPassed_ShouldWrapInCorrectOrder()
    {
        using SocketsHttpHandler handler = new();
        Mock<DelegatingHandler> firstDelegatingHandlerMock = new();
        Mock<DelegatingHandler> secondDelegatingHandlerMock = new();

        var resultingHandler = handler.WrapInto(new[] { firstDelegatingHandlerMock.Object, secondDelegatingHandlerMock.Object });

        resultingHandler.Should().BeAssignableTo<DelegatingHandler>();
        using (new AssertionScope())
        {
            resultingHandler.As<DelegatingHandler>().Should().Be(firstDelegatingHandlerMock.Object);
            resultingHandler.As<DelegatingHandler>().InnerHandler.Should().BeEquivalentTo(secondDelegatingHandlerMock.Object);
        }
    }
}
