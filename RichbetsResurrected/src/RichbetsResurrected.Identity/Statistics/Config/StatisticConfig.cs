using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RichbetsResurrected.Entities.DatabaseEntities.Statistics;

namespace RichbetsResurrected.Identity.Statistics.Config;

public class StatisticConfig : IEntityTypeConfiguration<Statistic>
{

    public void Configure(EntityTypeBuilder<Statistic> builder)
    {
        builder.HasKey(st => new
        {
            st.RichetUserId
        });

        builder.ToTable("statistics");
    }
}