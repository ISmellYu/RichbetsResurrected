using Ardalis.HttpClientTestExtensions;
using RichbetsResurrected.Web;
using Xunit;

namespace RichbetsResurrected.FunctionalTests.ControllerApis;

[Collection("Sequential")]
public class ProjectCreate : IClassFixture<CustomWebApplicationFactory<WebMarker>>
{
    private readonly HttpClient _client;

    public ProjectCreate(CustomWebApplicationFactory<WebMarker> factory)
    {
        _client = factory.CreateClient();
    }

}