using Ardalis.Specification;
using RichbetsResurrected.Core.ProjectAggregate;

namespace RichbetsResurrected.Core.ProjectAggregate.Specifications;

public class ProjectByIdWithItemsSpec : Specification<Project>, ISingleResultSpecification
{
    public ProjectByIdWithItemsSpec(int projectId)
    {
        Query
            .Where(project => project.Id == projectId)
            .Include(project => project.Items);
    }
}