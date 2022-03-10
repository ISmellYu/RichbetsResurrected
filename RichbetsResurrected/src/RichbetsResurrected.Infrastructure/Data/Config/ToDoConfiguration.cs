using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RichbetsResurrected.Core.ProjectAggregate;

namespace RichbetsResurrected.Infrastructure.Data.Config;

public class ToDoConfiguration : IEntityTypeConfiguration<ToDoItem>
{
    public void Configure(EntityTypeBuilder<ToDoItem> builder)
    {
        builder.Property(t => t.Title)
            .IsRequired();
        builder.ToTable("toDoItems");
    }
}