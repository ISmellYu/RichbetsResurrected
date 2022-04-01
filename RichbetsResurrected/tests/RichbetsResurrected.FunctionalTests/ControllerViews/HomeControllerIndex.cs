using RichbetsResurrected.Web;
using Xunit;

namespace RichbetsResurrected.FunctionalTests.ControllerViews;

[Collection("Sequential")]
public class HomeControllerIndex : IClassFixture<CustomWebApplicationFactory<WebMarker>>
{
    private readonly HttpClient _client;

    public HomeControllerIndex(CustomWebApplicationFactory<WebMarker> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task ReturnsViewWithCorrectMessage()
    {
        var response = await _client.GetAsync("/").ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var stringResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        Assert.Contains("RichbetsResurrected.Web", stringResponse);
    }
}