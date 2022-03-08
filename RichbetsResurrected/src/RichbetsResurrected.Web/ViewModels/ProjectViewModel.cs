namespace RichbetsResurrected.Web.ViewModels;

public class ProjectViewModel
{
    public List<ToDoItemViewModel> Items = new();
    public int Id { get; set; }
    public string? Name { get; set; }
}