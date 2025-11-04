using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SubmissionService.Domain.Entities;

namespace SubmissionService.Infrastructure.Data;

public class SubmissionDbContext : DbContext
{
    public SubmissionDbContext(DbContextOptions<SubmissionDbContext> options)
        : base(options)
    {
    }

    public DbSet<Submission> Submissions { get; set; }
    public DbSet<Appeal> Appeals { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Submission configuration
        modelBuilder.Entity<Submission>(entity =>
        {
            entity.ToTable("Submission");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.DateTime)
                .IsRequired()
                .HasColumnType("timestamp")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.CoverLetter)
                .HasColumnType("text");
            entity.Property(e => e.Comment)
                .HasColumnType("text");
            entity.Property(e => e.DocsPackageIsValid)
                .HasColumnType("boolean");
            entity.Property(e => e.SubmissionState)
                .HasMaxLength(100);

            // Note: ContestNoticeId and ParticipantId reference tables in other services/contexts
            // Foreign key constraints exist in the database
        });

        // Appeal configuration
        modelBuilder.Entity<Appeal>(entity =>
        {
            entity.ToTable("Appeal");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.DateTime)
                .IsRequired()
                .HasColumnType("timestamp")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.CoverLetter)
                .HasColumnType("text");
            entity.Property(e => e.AppealState)
                .HasMaxLength(100);
            entity.Property(e => e.Comment)
                .HasColumnType("text");

            entity.HasOne(e => e.Submission)
                .WithOne(e => e.Appeal)
                .HasForeignKey<Appeal>(e => e.SubmissionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Ensure 1:1 relationship with Submission
            entity.HasIndex(e => e.SubmissionId)
                .IsUnique();
        });
    }

    public void SeedData(ILogger? logger = null)
    {
        DbSeeder.SeedData(this, logger);
    }
}

