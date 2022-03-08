using RichbetsResurrected.Core.ProjectAggregate;
using Xunit;

namespace RichbetsResurrected.UnitTests.Core.ProjectAggregate;

public class Project_AddItem
{
    private readonly Project _testProject = new("some name");

    [Fact]
    public void AddsItemToItems()
    {
        var _testItem = new ToDoItem
        {
            Title = "title", Description = "description"
        };

        _testProject.AddItem(_testItem);

        Assert.Contains(_testItem, _testProject.Items);
    }

    [Fact]
    public void ThrowsExceptionGivenNullItem()
    {
#nullable disable
        var action = () => _testProject.AddItem(null);
#nullable enable

        var ex = Assert.Throws<ArgumentNullException>(action);
        Assert.Equal("newItem", ex.ParamName);
    }
}