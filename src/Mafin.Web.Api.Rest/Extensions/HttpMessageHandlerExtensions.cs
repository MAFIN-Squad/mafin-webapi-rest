namespace Mafin.Web.Api.Rest.Extensions;

/// <summary>
/// Provides extensions to <see cref="HttpMessageHandler"/>.
/// </summary>
internal static class HttpMessageHandlerExtensions
{
    /// <summary>
    /// Performs wrapping of request handlers via <see cref="DelegatingHandler"/> instances.
    /// </summary>
    /// <param name="handler">Handler to wrap.</param>
    /// <param name="delegatingHandlers">Collection of handlers performing the wrapping.</param>
    /// <returns>Resulting wrapper handler after wrapping.</returns>
    internal static HttpMessageHandler WrapInto(this HttpMessageHandler handler, IEnumerable<DelegatingHandler> delegatingHandlers) =>
        delegatingHandlers.Reverse().Aggregate(handler, (currentHandler, delegatingHandler) => currentHandler.WrapInto(delegatingHandler));

    private static DelegatingHandler WrapInto(this HttpMessageHandler handler, DelegatingHandler delegatingHandler)
    {
        delegatingHandler.InnerHandler = handler;
        return delegatingHandler;
    }
}
