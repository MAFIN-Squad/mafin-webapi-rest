using System.Text.Json;
using Mafin.Web.Api.Rest.Extensions;

namespace Mafin.Web.Api.Rest;

/// <summary>
/// Type providing possibility to build <see cref="MafinHttpClient"/> with required capabilities.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="MafinHttpClientBuilder"/> class.
/// </remarks>
/// <param name="baseAddress">Application base address value.</param>
#pragma warning disable CA1001 // Types that own disposable fields should be disposable - other type owns dispose.
public class MafinHttpClientBuilder(Uri baseAddress)
#pragma warning restore CA1001
{
    private readonly List<DelegatingHandler> _requestHandlers = [];
    private HttpClientHandler _authHandler = new();
    private JsonSerializerOptions _options = JsonSerializerOptions.Default;

    /// <summary>
    /// Initializes a new instance of the <see cref="MafinHttpClientBuilder"/> class.
    /// </summary>
    /// <param name="baseAddress">Application base address value.</param>
    public MafinHttpClientBuilder(string baseAddress)
        : this(new Uri(baseAddress))
    {
    }

    /// <summary>
    /// Adds authentication handler into build chain.
    /// </summary>
    /// <param name="handler">Authentication handler.</param>
    /// <returns><see cref="MafinHttpClientBuilder"/> instance.</returns>
    public MafinHttpClientBuilder WithAuthHandler(HttpClientHandler handler)
    {
        _authHandler = handler ?? throw new ArgumentNullException(nameof(handler));
        return this;
    }

    /// <summary>
    /// Customizes predefined authentication handler.
    /// </summary>
    /// <param name="authCustomizationAction">Authentication handler customization action.</param>
    /// <returns><see cref="MafinHttpClientBuilder"/> instance.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="authCustomizationAction"/> is null.</exception>
    public MafinHttpClientBuilder WithAuthHandler(Action<HttpClientHandler> authCustomizationAction)
    {
        if (authCustomizationAction is null)
        {
            throw new ArgumentNullException(nameof(authCustomizationAction));
        }

        authCustomizationAction(_authHandler);
        return this;
    }

    /// <summary>
    /// Adds custom <see cref="JsonSerializerOptions"/> for altering serialization process.
    /// </summary>
    /// <param name="options">Serialization process options instance.</param>
    /// <returns><see cref="MafinHttpClientBuilder"/> instance.</returns>
    public MafinHttpClientBuilder WithJsonSerializerOptions(JsonSerializerOptions options)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
        return this;
    }

    /// <summary>
    /// Customizes predefined serialization process.
    /// </summary>
    /// <param name="serializerCustomizationAction">Serialization process customization action.</param>
    /// <returns><see cref="MafinHttpClientBuilder"/> instance.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="serializerCustomizationAction"/> is null.</exception>
    public MafinHttpClientBuilder WithJsonSerializerOptions(Action<JsonSerializerOptions> serializerCustomizationAction)
    {
        if (serializerCustomizationAction is null)
        {
            throw new ArgumentNullException(nameof(serializerCustomizationAction));
        }

        serializerCustomizationAction(_options);
        return this;
    }

    /// <summary>
    /// Adds custom handler into build chain.
    /// </summary>
    /// <param name="handler">Custom handler derived from <see cref="DelegatingHandler"/>.</param>
    /// <returns><see cref="MafinHttpClientBuilder"/> instance.</returns>
    public MafinHttpClientBuilder WithRequestHandler(DelegatingHandler handler) => WithRequestHandlers(new[] { handler });

    /// <summary>
    /// Adds set of custom handlers into build chain.
    /// </summary>
    /// <param name="handlers">Collection of custom handlers derived from <see cref="DelegatingHandler"/>.</param>
    /// <returns><see cref="MafinHttpClientBuilder"/> instance.</returns>
    public MafinHttpClientBuilder WithRequestHandlers(IEnumerable<DelegatingHandler> handlers)
    {
        _requestHandlers.AddRange(handlers);
        return this;
    }

    /// <summary>
    /// Builds <see cref="MafinHttpClient"/> using build chain.
    /// </summary>
    /// <returns><see cref="MafinHttpClient"/> instance.</returns>
    public virtual MafinHttpClient Build() => new(GetHandlerChain())
    {
        BaseAddress = baseAddress,
        JsonSerializerOptions = _options,
    };

    /// <summary>
    /// Creates handler chain using provided build options.
    /// </summary>
    /// <returns><see cref="HttpMessageHandler"/> wrapping a handler chain.</returns>
    protected virtual HttpMessageHandler GetHandlerChain() =>
        _requestHandlers.Count > 0
        ? _authHandler.WrapInto(_requestHandlers)
        : _authHandler;
}
