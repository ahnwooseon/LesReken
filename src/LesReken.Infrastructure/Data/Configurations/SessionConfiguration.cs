using LesReken.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LesReken.Infrastructure.Data.Configurations;

public class SessionConfiguration : IEntityTypeConfiguration<Session>
{
    public void Configure(EntityTypeBuilder<Session> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.HourlyRate).HasPrecision(18, 2);
    }
}

public class SessionStudentConfiguration : IEntityTypeConfiguration<SessionStudent>
{
    public void Configure(EntityTypeBuilder<SessionStudent> builder)
    {
        builder.HasKey(ss => new { ss.SessionId, ss.StudentId });

        builder
            .HasOne(ss => ss.Session)
            .WithMany(s => s.SessionStudents)
            .HasForeignKey(ss => ss.SessionId);

        builder.HasOne(ss => ss.Student).WithMany().HasForeignKey(ss => ss.StudentId);
    }
}
