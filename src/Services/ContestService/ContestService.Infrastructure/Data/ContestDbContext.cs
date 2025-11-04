using ContestService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContestService.Infrastructure.Data;

public class ContestDbContext : DbContext
{
    public ContestDbContext(DbContextOptions<ContestDbContext> options)
        : base(options)
    {
    }

    public DbSet<Event> Events { get; set; }
    public DbSet<EventStage> EventStages { get; set; }
    public DbSet<EventStageCriteria> EventStageCriterias { get; set; }
    public DbSet<ContestNotice> ContestNotices { get; set; }
    public DbSet<Attachment> Attachments { get; set; }
    public DbSet<ContestDocsPackage> ContestDocsPackages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Event configuration
        modelBuilder.Entity<Event>(entity =>
        {
            entity.ToTable("Event");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.Description)
                .HasColumnType("text");
        });

        // EventStage configuration
        modelBuilder.Entity<EventStage>(entity =>
        {
            entity.ToTable("EventStage");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.Description)
                .HasColumnType("text");
            entity.Property(e => e.DateStart)
                .IsRequired()
                .HasColumnType("timestamp");
            entity.Property(e => e.DateEnd)
                .IsRequired()
                .HasColumnType("timestamp");

            entity.HasOne(e => e.Event)
                .WithMany(e => e.EventStages)
                .HasForeignKey(e => e.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.PreviousStage)
                .WithMany(e => e.NextStages)
                .HasForeignKey(e => e.PreviousStageId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // EventStageCriteria configuration
        modelBuilder.Entity<EventStageCriteria>(entity =>
        {
            entity.ToTable("EventStageCriteria");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.Description)
                .HasColumnType("text");
            entity.Property(e => e.IsRequired)
                .IsRequired()
                .HasDefaultValue(false);
            entity.Property(e => e.MinimumThresholdScore)
                .HasColumnType("decimal(10,2)");

            entity.HasOne(e => e.EventStage)
                .WithMany(e => e.EventStageCriterias)
                .HasForeignKey(e => e.EventStageId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ContestNotice configuration
        modelBuilder.Entity<ContestNotice>(entity =>
        {
            entity.ToTable("ContestNotice");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Goal)
                .HasColumnType("text");
            entity.Property(e => e.CompetitionType)
                .HasMaxLength(100);
            entity.Property(e => e.Description)
                .HasColumnType("text");

            entity.HasOne(e => e.Event)
                .WithMany(e => e.ContestNotices)
                .HasForeignKey(e => e.EventId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Attachment configuration
        modelBuilder.Entity<Attachment>(entity =>
        {
            entity.ToTable("Attachment");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255);
        });

        // ContestDocsPackage configuration
        modelBuilder.Entity<ContestDocsPackage>(entity =>
        {
            entity.ToTable("ContestDocsPackage");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.IsRequired)
                .IsRequired()
                .HasDefaultValue(false);
            entity.Property(e => e.Comment)
                .HasColumnType("text");

            entity.HasOne(e => e.ContestNotice)
                .WithMany(e => e.ContestDocsPackages)
                .HasForeignKey(e => e.ContestNoticeId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Attachment)
                .WithMany(e => e.ContestDocsPackages)
                .HasForeignKey(e => e.AttachmentId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    public void SeedData()
    {
        DbSeeder.SeedData(this);
    }
}

