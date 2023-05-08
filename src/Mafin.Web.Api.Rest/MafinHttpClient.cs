using System.Text.Json;
using Mafin.Web.Api.Rest.Extensions;

namespace Mafin.Web.Api.Rest;

/// <inheritdoc cref="HttpClient"/>
public class MafinHttpClient : HttpClient
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MafinHttpClient"/> class
    /// using a <see cref="HttpClientHandler"/> that is disposed when this instance is disposed.
    /// </summary>
    public MafinHttpClient()
        : base()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MafinHttpClient"/> class with the specified handler.
    /// The handler is disposed when this instance is disposed.
    /// </summary>
    /// <param name="handler">The HTTP handler stack to use for sending requests.</param>
    public MafinHttpClient(HttpMessageHandler handler)
        : base(handler)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MafinHttpClient"/> class with the provided handler,
    /// and specifies whether that handler should be disposed when this instance is disposed.
    /// </summary>
    /// <param name="handler">The HTTP handler stack to use for sending requests.</param>
    /// <param name="disposeHandler">Whether to dispose handler when <see cref="MafinHttpClient"/> is disposed.</param>
    public MafinHttpClient(HttpMessageHandler handler, bool disposeHandler)
        : base(handler, disposeHandler)
    {
    }

    /// <summary>
    /// Gets or sets options for <see cref="JsonSerializer"/>.
    /// </summary>
    public JsonSerializerOptions? JsonSerializerOptions { get; set; }

    /// <summary>
    /// Send a GET request to the specified Uri as an asynchronous operation.
    /// </summary>
    /// <param name="requestUrl">The Url the request is sent to.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public async Task<EndpointResponse> SendGetAsync(string? requestUrl, CancellationToken? cancellationToken = null) =>
        await SendGetAsync(CreateUri(requestUrl), cancellationToken).ConfigureAwait(false);

    /// <summary>
    /// Send a GET request to the specified Uri as an asynchronous operation.
    /// </summary>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public async Task<EndpointResponse> SendGetAsync(Uri? requestUri, CancellationToken? cancellationToken = null) =>
        new EndpointResponse(await GetAsync(requestUri, ResolveToken(cancellationToken)).ConfigureAwait(false));

    /// <summary>
    /// Send a GET request to the specified Uri as an asynchronous operation.
    /// </summary>
    /// <typeparam name="T">Response content type.</typeparam>
    /// <param name="requestUrl">The Url the request is sent to.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public async Task<EndpointResponse<T?>> SendGetAsync<T>(string? requestUrl, CancellationToken? cancellationToken = null) =>
        await SendGetAsync<T?>(CreateUri(requestUrl), cancellationToken).ConfigureAwait(false);

    /// <summary>
    /// Send a GET request to the specified Uri as an asynchronous operation.
    /// </summary>
    /// <typeparam name="T">Response content type.</typeparam>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public async Task<EndpointResponse<T?>> SendGetAsync<T>(Uri? requestUri, CancellationToken? cancellationToken = null)
    {
        var response = await GetAsync(requestUri, ResolveToken(cancellationToken)).ConfigureAwait(false);
        return new EndpointResponse<T?>(response, await response.AsEntity<T?>(JsonSerializerOptions).ConfigureAwait(false));
    }

    /// <summary>
    /// Send a POST request to the specified Uri as an asynchronous operation.
    /// </summary>
    /// <typeparam name="T">Request content type.</typeparam>
    /// <param name="requestUrl">The Url the request is sent to.</param>
    /// <param name="content">Request body.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public async Task<EndpointResponse> SendPostAsync<T>(string? requestUrl, T? content, CancellationToken? cancellationToken = null) =>
        await SendPostAsync(CreateUri(requestUrl), content, cancellationToken).ConfigureAwait(false);

    /// <summary>
    /// Send a POST request to the specified Uri as an asynchronous operation.
    /// </summary>
    /// <typeparam name="T">Request content type.</typeparam>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="content">Request body.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public async Task<EndpointResponse> SendPostAsync<T>(Uri? requestUri, T? content, CancellationToken? cancellationToken = null) =>
        new EndpointResponse(await PostAsync(requestUri, content?.ToJson(JsonSerializerOptions), ResolveToken(cancellationToken)).ConfigureAwait(false));

    /// <summary>
    /// Send a POST request to the specified Uri as an asynchronous operation.
    /// </summary>
    /// <typeparam name="TO">Response content type.</typeparam>
    /// <typeparam name="TI">Request content type.</typeparam>
    /// <param name="requestUrl">The Url the request is sent to.</param>
    /// <param name="content">Request body.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public async Task<EndpointResponse<TO?>> SendPostAsync<TO, TI>(string? requestUrl, TI? content, CancellationToken? cancellationToken = null) =>
        await SendPostAsync<TO?, TI?>(CreateUri(requestUrl), content, cancellationToken).ConfigureAwait(false);

    /// <summary>
    /// Send a POST request to the specified Uri as an asynchronous operation.
    /// </summary>
    /// <typeparam name="TO">Response content type.</typeparam>
    /// <typeparam name="TI">Request content type.</typeparam>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="content">Request body.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public async Task<EndpointResponse<TO?>> SendPostAsync<TO, TI>(Uri? requestUri, TI? content, CancellationToken? cancellationToken = null)
    {
        var response = await PostAsync(requestUri, content?.ToJson(JsonSerializerOptions), ResolveToken(cancellationToken)).ConfigureAwait(false);
        return new EndpointResponse<TO?>(response, await response.AsEntity<TO?>(JsonSerializerOptions).ConfigureAwait(false));
    }

    /// <summary>
    /// Send a PUT request to the specified Uri as an asynchronous operation.
    /// </summary>
    /// <typeparam name="T">Request content type.</typeparam>
    /// <param name="requestUrl">The Url the request is sent to.</param>
    /// <param name="content">Request body.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public async Task<EndpointResponse> SendPutAsync<T>(string? requestUrl, T? content, CancellationToken? cancellationToken = null) =>
        await SendPutAsync(CreateUri(requestUrl), content, cancellationToken).ConfigureAwait(false);

    /// <summary>
    /// Send a PUT request to the specified Uri as an asynchronous operation.
    /// </summary>
    /// <typeparam name="T">Request content type.</typeparam>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="content">Request body.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public async Task<EndpointResponse> SendPutAsync<T>(Uri? requestUri, T? content, CancellationToken? cancellationToken = null) =>
        new EndpointResponse(await PutAsync(requestUri, content?.ToJson(JsonSerializerOptions), ResolveToken(cancellationToken)).ConfigureAwait(false));

    /// <summary>
    /// Send a PUT request to the specified Uri as an asynchronous operation.
    /// </summary>
    /// <typeparam name="TO">Response content type.</typeparam>
    /// <typeparam name="TI">Request content type.</typeparam>
    /// <param name="requestUrl">The Url the request is sent to.</param>
    /// <param name="content">Request body.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public async Task<EndpointResponse<TO?>> SendPutAsync<TO, TI>(string? requestUrl, TI? content, CancellationToken? cancellationToken = null) =>
        await SendPutAsync<TO?, TI?>(CreateUri(requestUrl), content, cancellationToken).ConfigureAwait(false);

    /// <summary>
    /// Send a PUT request to the specified Uri as an asynchronous operation.
    /// </summary>
    /// <typeparam name="TO">Response content type.</typeparam>
    /// <typeparam name="TI">Request content type.</typeparam>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="content">Request body.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public async Task<EndpointResponse<TO?>> SendPutAsync<TO, TI>(Uri? requestUri, TI? content, CancellationToken? cancellationToken = null)
    {
        var response = await PutAsync(requestUri, content?.ToJson(JsonSerializerOptions), ResolveToken(cancellationToken)).ConfigureAwait(false);
        return new EndpointResponse<TO?>(response, await response.AsEntity<TO?>(JsonSerializerOptions).ConfigureAwait(false));
    }

    /// <summary>
    /// Send a PATCH request to the specified Uri as an asynchronous operation.
    /// </summary>
    /// <typeparam name="T">Request content type.</typeparam>
    /// <param name="requestUrl">The Url the request is sent to.</param>
    /// <param name="content">Request body.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public async Task<EndpointResponse> SendPatchAsync<T>(string? requestUrl, T? content, CancellationToken? cancellationToken = null) =>
        await SendPatchAsync(CreateUri(requestUrl), content, cancellationToken).ConfigureAwait(false);

    /// <summary>
    /// Send a PATCH request to the specified Uri as an asynchronous operation.
    /// </summary>
    /// <typeparam name="T">Request content type.</typeparam>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="content">Request body.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public async Task<EndpointResponse> SendPatchAsync<T>(Uri? requestUri, T? content, CancellationToken? cancellationToken = null) =>
        new EndpointResponse(await PatchAsync(requestUri, content?.ToJson(JsonSerializerOptions), ResolveToken(cancellationToken)).ConfigureAwait(false));

    /// <summary>
    /// Send a PATCH request to the specified Uri as an asynchronous operation.
    /// </summary>
    /// <typeparam name="TO">Response content type.</typeparam>
    /// <typeparam name="TI">Request content type.</typeparam>
    /// <param name="requestUrl">The Url the request is sent to.</param>
    /// <param name="content">Request body.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public async Task<EndpointResponse<TO?>> SendPatchAsync<TO, TI>(string? requestUrl, TI? content, CancellationToken? cancellationToken = null) =>
        await SendPatchAsync<TO?, TI?>(CreateUri(requestUrl), content, cancellationToken).ConfigureAwait(false);

    /// <summary>
    /// Send a PATCH request to the specified Uri as an asynchronous operation.
    /// </summary>
    /// <typeparam name="TO">Response content type.</typeparam>
    /// <typeparam name="TI">Request content type.</typeparam>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="content">Request body.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public async Task<EndpointResponse<TO?>> SendPatchAsync<TO, TI>(Uri? requestUri, TI? content, CancellationToken? cancellationToken = null)
    {
        var response = await PatchAsync(requestUri, content?.ToJson(JsonSerializerOptions), ResolveToken(cancellationToken)).ConfigureAwait(false);
        return new EndpointResponse<TO?>(response, await response.AsEntity<TO?>(JsonSerializerOptions).ConfigureAwait(false));
    }

    /// <summary>
    /// Send a DELETE request to the specified Uri as an asynchronous operation.
    /// </summary>
    /// <param name="requestUrl">The Url the request is sent to.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public async Task<EndpointResponse> SendDeleteAsync(string? requestUrl, CancellationToken? cancellationToken = null) =>
        await SendDeleteAsync(CreateUri(requestUrl), cancellationToken).ConfigureAwait(false);

    /// <summary>
    /// Send a DELETE request to the specified Uri as an asynchronous operation.
    /// </summary>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public async Task<EndpointResponse> SendDeleteAsync(Uri? requestUri, CancellationToken? cancellationToken = null) =>
        new EndpointResponse(await DeleteAsync(requestUri, ResolveToken(cancellationToken)).ConfigureAwait(false));

    /// <summary>
    /// Send a DELETE request to the specified Uri as an asynchronous operation.
    /// </summary>
    /// <typeparam name="T">Response content type.</typeparam>
    /// <param name="requestUrl">The Url the request is sent to.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public async Task<EndpointResponse<T?>> SendDeleteAsync<T>(string? requestUrl, CancellationToken? cancellationToken = null) =>
        await SendDeleteAsync<T?>(CreateUri(requestUrl), cancellationToken).ConfigureAwait(false);

    /// <summary>
    /// Send a DELETE request to the specified Uri as an asynchronous operation.
    /// </summary>
    /// <typeparam name="T">Response content type.</typeparam>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public async Task<EndpointResponse<T?>> SendDeleteAsync<T>(Uri? requestUri, CancellationToken? cancellationToken = null)
    {
        var response = await DeleteAsync(requestUri, ResolveToken(cancellationToken)).ConfigureAwait(false);
        return new EndpointResponse<T?>(response, await response.AsEntity<T?>(JsonSerializerOptions).ConfigureAwait(false));
    }

    /// <summary>
    /// Creates <see cref="Uri"/> instance from <see cref="string"/> url form.
    /// </summary>
    /// <param name="url"><see cref="string"/> representation of url address.</param>
    /// <returns><see cref="Uri"/> instance for the provided url.</returns>
#pragma warning disable CA1054 // URI-like parameters should not be strings
    protected static Uri? CreateUri(string? url) => string.IsNullOrWhiteSpace(url) ? null : new Uri(url, UriKind.Relative);
#pragma warning restore CA1054

    private static CancellationToken ResolveToken(CancellationToken? token) => token ?? CancellationToken.None;
}
