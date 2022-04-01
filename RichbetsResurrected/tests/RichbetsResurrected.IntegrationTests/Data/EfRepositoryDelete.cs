using RichbetsResurrected.Core.ProjectAggregate;
using Xunit;

namespace RichbetsResurrected.IntegrationTests.Data;

public class EfRepositoryDelete : BaseEfRepoTestFixture
{
    [Fact]
    public async Task DeletesItemAfterAddingIt()
    {
        // add a project
        var repository = GetRepository();
        var initialName = Guid.NewGuid().ToString();
        var project = new Project(initialName);
        await repository.AddAsync(project).ConfigureAwait(false);

        // delete the item
        await repository.DeleteAsync(project).ConfigureAwait(false);

        // verify it's no longer there
        Assert.DoesNotContain(await repository.ListAsync().ConfigureAwait(false),
            project => project.Name == initialName);
    }
}