﻿using Ardalis.HttpClientTestExtensions;
using RichbetsResurrected.Web;
using RichbetsResurrected.Web.ApiModels;
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

    [Fact]
    public async Task ReturnsOneProject()
    {
        var result = await _client.GetAndDeserialize<IEnumerable<ProjectDTO>>("/api/projects").ConfigureAwait(false);

        Assert.Single(result);
        Assert.Contains(result, i => i.Name == SeedData.TestProject1.Name);
    }
}