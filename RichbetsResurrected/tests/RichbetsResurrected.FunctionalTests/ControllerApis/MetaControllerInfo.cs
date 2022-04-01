﻿using RichbetsResurrected.Web;
using Xunit;

namespace RichbetsResurrected.FunctionalTests.ControllerApis;

[Collection("Sequential")]
public class MetaControllerInfo : IClassFixture<CustomWebApplicationFactory<WebMarker>>
{
    private readonly HttpClient _client;

    public MetaControllerInfo(CustomWebApplicationFactory<WebMarker> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task ReturnsVersionAndLastUpdateDate()
    {
        var response = await _client.GetAsync("/info").ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var stringResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        Assert.Contains("Version", stringResponse);
        Assert.Contains("Last Updated", stringResponse);
    }
}