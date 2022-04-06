using Ardalis.Result;
using RichbetsResurrected.Core.DiscordAggregate;
using RichbetsResurrected.Core.Interfaces;
using RichbetsResurrected.Core.ProjectAggregate;
using RichbetsResurrected.Core.ProjectAggregate.Specifications;
using RichbetsResurrected.SharedKernel.Interfaces;

namespace RichbetsResurrected.Core.Services;

public class ToDoItemSearchService : IToDoItemSearchService
{
    private readonly IRepository<Project> _repository;

    public ToDoItemSearchService(IRepository<Project> repository)
    {
        _repository = repository;
    }

    public async Task<Result<List<ToDoItem>>> GetAllIncompleteItemsAsync(int projectId, string searchString)
    {
        if (string.IsNullOrEmpty(searchString))
        {
            var errors = new List<ValidationError>();
            errors.Add(new ValidationError
            {
                Identifier = nameof(searchString), ErrorMessage = $"{nameof(searchString)} is required."
            });
            return Result<List<ToDoItem>>.Invalid(errors);
        }

        var projectSpec = new ProjectByIdWithItemsSpec(projectId);
        var project = await _repository.GetBySpecAsync(projectSpec).ConfigureAwait(false);

        // TODO: Optionally use Ardalis.GuardClauses Guard.Against.NotFound and catch
        if (project == null) return Result<List<ToDoItem>>.NotFound();

        var incompleteSpec = new IncompleteItemsSearchSpec(searchString);

        try
        {
            var items = incompleteSpec.Evaluate(project.Items).ToList();

            return new Result<List<ToDoItem>>(items);
        }
        catch (Exception ex)
        {
            // TODO: Log details here
            return Result<List<ToDoItem>>.Error(ex.Message);
        }
    }

    public async Task<Result<ToDoItem>> GetNextIncompleteItemAsync(int projectId)
    {
        var projectSpec = new ProjectByIdWithItemsSpec(projectId);
        var project = await _repository.GetBySpecAsync(projectSpec).ConfigureAwait(false);
        if (project == null) return Result<ToDoItem>.NotFound();

        var incompleteSpec = new IncompleteItemsSpec();

        var items = incompleteSpec.Evaluate(project.Items).ToList();

        if (!items.Any()) return Result<ToDoItem>.NotFound();

        return new Result<ToDoItem>(items.First());
    }
}