using FluentAssertions.Execution;
using Mafin.Web.Api.Rest.Extensions;
using NSubstitute;

namespace Mafin.Web.Api.Rest.Tests.Unit.Extensions;

public class HttpMessageHandlerExtensionsTests
{
    [Fact]
    public void WrapInto_WhenHandlerPassed_ShouldWrapIntoInnerHandler()
    {
        using SocketsHttpHandler handler = new();
        var delegatingHandlerMock = Substitute.For<DelegatingHandler>();

        var resultingHandler = handler.WrapInto(new[] { delegatingHandlerMock });

        resultingHandler.Should().BeAssignableTo<DelegatingHandler>();
        resultingHandler.As<DelegatingHandler>().InnerHandler.Should().BeEquivalentTo(handler);
    }

    [Fact]
    public void WrapInto_WhenMultipleHandlersPassed_ShouldWrapInCorrectOrder()
    {
        using SocketsHttpHandler handler = new();
        var firstDelegatingHandlerMock = Substitute.For<DelegatingHandler>();
        var secondDelegatingHandlerMock = Substitute.For<DelegatingHandler>();

        var resultingHandler = handler.WrapInto(new[] { firstDelegatingHandlerMock, secondDelegatingHandlerMock });

        resultingHandler.Should().BeAssignableTo<DelegatingHandler>();
        using (new AssertionScope())
        {
            resultingHandler.As<DelegatingHandler>().Should().Be(firstDelegatingHandlerMock);
            resultingHandler.As<DelegatingHandler>().InnerHandler.Should().BeEquivalentTo(secondDelegatingHandlerMock);
        }
    }
}
