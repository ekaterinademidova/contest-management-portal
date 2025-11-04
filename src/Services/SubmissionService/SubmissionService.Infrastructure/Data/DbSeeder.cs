using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SubmissionService.Domain.Entities;

namespace SubmissionService.Infrastructure.Data;

public static class DbSeeder
{
    private static ILogger? _logger;

    public static void SeedData(SubmissionDbContext context, ILogger? logger = null)
    {
        _logger = logger;
        SeedSubmissions(context);
        SeedAppeals(context);
    }

    // Helper method to convert UTC DateTime to Unspecified (required for PostgreSQL timestamp without time zone)
    private static DateTime ToUnspecified(DateTime utcDateTime)
    {
        return DateTime.SpecifyKind(utcDateTime, DateTimeKind.Unspecified);
    }

    private static void SeedSubmissions(SubmissionDbContext context)
    {
        if (context.Submissions.Any())
        {
            _logger?.LogInformation("Submissions already exist, skipping seeding.");
            return; // Submissions already seeded
        }

        _logger?.LogInformation("Starting to seed Submissions...");

        // Try to get available ContestNoticeIds, but don't fail if table doesn't exist
        var availableContestNoticeIds = GetAvailableContestNoticeIds(context);
        
        // If no ContestNotices found, use default IDs (1, 2, 3) - they might be created later
        // This allows seeding to work even if ContestService hasn't seeded yet
        var contestNoticeId1 = availableContestNoticeIds.Contains(1) ? 1 : (availableContestNoticeIds.Any() ? availableContestNoticeIds.First() : 1);
        var contestNoticeId2 = availableContestNoticeIds.Contains(2) ? 2 : (availableContestNoticeIds.Count > 1 ? availableContestNoticeIds.Skip(1).First() : 2);
        var contestNoticeId3 = availableContestNoticeIds.Contains(3) ? 3 : (availableContestNoticeIds.Count > 2 ? availableContestNoticeIds.Skip(2).First() : 3);

        _logger?.LogInformation($"Using ContestNoticeIds: {contestNoticeId1}, {contestNoticeId2}, {contestNoticeId3}");

        var submissions = new List<Submission>();

        // Create submissions for different contest notices
        submissions.Add(new Submission
        {
            ContestNoticeId = contestNoticeId1,
            ParticipantId = 1,
            DateTime = ToUnspecified(DateTime.UtcNow.AddDays(-10)),
            CoverLetter = "I am excited to participate in this programming championship. I have been preparing for months and look forward to the challenges ahead.",
            Comment = "Documents submitted successfully",
            DocsPackageIsValid = true,
            SubmissionState = "Approved"
        });

        submissions.Add(new Submission
        {
            ContestNoticeId = contestNoticeId1,
            ParticipantId = 2,
            DateTime = ToUnspecified(DateTime.UtcNow.AddDays(-8)),
            CoverLetter = "As an experienced developer, I am eager to compete and showcase my algorithmic problem-solving skills.",
            Comment = "All required documents attached",
            DocsPackageIsValid = true,
            SubmissionState = "Pending"
        });

        submissions.Add(new Submission
        {
            ContestNoticeId = contestNoticeId1,
            ParticipantId = 3,
            DateTime = ToUnspecified(DateTime.UtcNow.AddDays(-5)),
            CoverLetter = "This competition represents an excellent opportunity to test my coding abilities and learn from other talented participants.",
            Comment = "Submission complete",
            DocsPackageIsValid = true,
            SubmissionState = "Approved"
        });

        // ContestNoticeId 2 - Hackathon
        submissions.Add(new Submission
        {
            ContestNoticeId = contestNoticeId2,
            ParticipantId = 4,
            DateTime = ToUnspecified(DateTime.UtcNow.AddDays(-7)),
            CoverLetter = "Our team is ready to build innovative solutions during the hackathon. We have experience in full-stack development and are excited to participate.",
            Comment = "Team registration documents included",
            DocsPackageIsValid = true,
            SubmissionState = "Approved"
        });

        submissions.Add(new Submission
        {
            ContestNoticeId = contestNoticeId2,
            ParticipantId = 5,
            DateTime = ToUnspecified(DateTime.UtcNow.AddDays(-3)),
            CoverLetter = "We are a team of passionate developers looking forward to creating something impactful during the 48-hour hackathon.",
            Comment = null,
            DocsPackageIsValid = false,
            SubmissionState = "Rejected"
        });

        // ContestNoticeId 3 - AI Contest
        submissions.Add(new Submission
        {
            ContestNoticeId = contestNoticeId3,
            ParticipantId = 1,
            DateTime = ToUnspecified(DateTime.UtcNow.AddDays(-6)),
            CoverLetter = "I have extensive experience in machine learning and deep learning. Excited to participate in this AI challenge.",
            Comment = "ML models and documentation attached",
            DocsPackageIsValid = true,
            SubmissionState = "Pending"
        });

        try
        {
            context.Submissions.AddRange(submissions);
            var savedCount = context.SaveChanges();
            _logger?.LogInformation($"Successfully seeded {savedCount} submissions.");
        }
        catch (DbUpdateException ex)
        {
            _logger?.LogWarning(ex, "Failed to seed submissions. This may be due to missing foreign key references (ContestNotice or User tables). Error: {Message}", ex.Message);
            // If foreign key constraints fail, log the error but continue
            // Data will be seeded when dependencies are available
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Unexpected error while seeding submissions: {Message}", ex.Message);
        }
    }

    private static List<int> GetAvailableContestNoticeIds(SubmissionDbContext context)
    {
        try
        {
            var sql = "SELECT \"Id\" FROM \"ContestNotice\" LIMIT 10;";
            var connection = context.Database.GetDbConnection();
            var wasOpen = connection.State == System.Data.ConnectionState.Open;
            
            if (!wasOpen)
            {
                connection.Open();
            }

            using var command = connection.CreateCommand();
            command.CommandText = sql;
            using var reader = command.ExecuteReader();
            var ids = new List<int>();
            while (reader.Read())
            {
                ids.Add(reader.GetInt32(0));
            }

            if (!wasOpen)
            {
                connection.Close();
            }

            _logger?.LogInformation($"Found {ids.Count} ContestNotice records: {string.Join(", ", ids)}");
            return ids;
        }
        catch (Exception ex)
        {
            _logger?.LogWarning(ex, "Could not query ContestNotice table. Using default IDs. Error: {Message}", ex.Message);
            return new List<int>();
        }
    }

    private static void SeedAppeals(SubmissionDbContext context)
    {
        if (context.Appeals.Any())
        {
            _logger?.LogInformation("Appeals already exist, skipping seeding.");
            return; // Appeals already seeded
        }

        _logger?.LogInformation("Starting to seed Appeals...");

        // Get a rejected submission to create an appeal for
        var rejectedSubmission = context.Submissions.FirstOrDefault(s => s.SubmissionState == "Rejected");
        
        if (rejectedSubmission == null)
        {
            _logger?.LogInformation("No rejected submissions found, skipping appeal seeding.");
            return; // No rejected submissions to appeal
        }

        // Check if appeal already exists for this submission (1:1 relationship)
        var existingAppeal = context.Appeals.FirstOrDefault(a => a.SubmissionId == rejectedSubmission.Id);
        if (existingAppeal != null)
        {
            _logger?.LogInformation("Appeal already exists for rejected submission, skipping.");
            return; // Appeal already exists
        }

        var appeal1 = new Appeal
        {
            SubmissionId = rejectedSubmission.Id,
            DateTime = ToUnspecified(DateTime.UtcNow.AddDays(-1)),
            CoverLetter = "I would like to appeal the rejection of my submission. I believe there was a misunderstanding regarding the document package validation. All required documents were submitted correctly.",
            AppealState = "Pending",
            Comment = "Requesting reconsideration of submission status"
        };

        try
        {
            context.Appeals.Add(appeal1);
            var savedCount = context.SaveChanges();
            _logger?.LogInformation($"Successfully seeded {savedCount} appeal.");
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Failed to seed appeal: {Message}", ex.Message);
        }
    }
}

