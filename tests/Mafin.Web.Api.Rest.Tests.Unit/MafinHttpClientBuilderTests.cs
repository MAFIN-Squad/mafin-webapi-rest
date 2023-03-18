namespace Mafin.Web.Api.Rest.Tests.Unit;

public class MafinHttpClientBuilderTests
{
    private const string Url = "https://example.com";

    private MafinHttpClientBuilder? _builder;

    [Fact]
    public void Constructor_WithStringBaseAddress_ShouldSetHttpClientBaseAddress()
    {
        _builder = new MafinHttpClientBuilder(Url);

        var client = _builder.Build();

        client.BaseAddress.Should().Be(new Uri(Url));
    }

    [Fact]
    public void Constructor_WithUriBaseAddress_ShouldSetHttpClientBaseAddress()
    {
        Uri uri = new(Url);
        _builder = new MafinHttpClientBuilder(uri);

        var client = _builder.Build();

        client.BaseAddress.Should().Be(uri);
    }
}
