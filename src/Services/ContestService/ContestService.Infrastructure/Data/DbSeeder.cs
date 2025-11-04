using ContestService.Domain.Entities;

namespace ContestService.Infrastructure.Data;

public static class DbSeeder
{
    public static void SeedData(ContestDbContext context)
    {
        SeedEvents(context);
        SeedEventStages(context);
        SeedEventStageCriterias(context);
        SeedAttachments(context);
        SeedContestNotices(context);
        SeedContestDocsPackages(context);
    }

    private static void SeedEvents(ContestDbContext context)
    {
        if (context.Events.Any())
        {
            return; // Events already seeded
        }

        var programmingCompetition = new Event
        {
            Name = "Annual Programming Championship 2024",
            Description = "A prestigious programming competition bringing together the best coders from universities and tech companies. This event features multiple stages, challenging algorithmic problems, and exciting prizes."
        };

        var hackathonEvent = new Event
        {
            Name = "Innovation Hackathon 2024",
            Description = "A 48-hour hackathon focused on building innovative solutions for real-world problems. Participants work in teams to create prototypes and present their ideas to industry experts."
        };

        var aiContest = new Event
        {
            Name = "AI & Machine Learning Challenge",
            Description = "An advanced competition for AI enthusiasts to showcase their skills in machine learning, deep learning, and neural networks. Features datasets, model training challenges, and real-world applications."
        };

        context.Events.AddRange(programmingCompetition, hackathonEvent, aiContest);
        context.SaveChanges();
    }

    private static void SeedEventStages(ContestDbContext context)
    {
        if (context.EventStages.Any())
        {
            return; // EventStages already seeded
        }

        // Get events (assuming they exist or were just seeded)
        var programmingCompetition = context.Events.FirstOrDefault(e => e.Name.Contains("Programming Championship"));
        var hackathonEvent = context.Events.FirstOrDefault(e => e.Name.Contains("Hackathon"));

        if (programmingCompetition == null || hackathonEvent == null)
        {
            return; // Cannot seed stages without events
        }

        // Programming Competition stages
        var registrationStage = new EventStage
        {
            EventId = programmingCompetition.Id,
            PreviousStageId = null,
            Name = "Registration Phase",
            Description = "Participants register for the competition and submit required documentation.",
            DateStart = new DateTime(2024, 1, 1, 9, 0, 0, DateTimeKind.Utc),
            DateEnd = new DateTime(2024, 1, 31, 23, 59, 59, DateTimeKind.Utc)
        };
        context.EventStages.Add(registrationStage);
        context.SaveChanges();

        var qualificationStage = new EventStage
        {
            EventId = programmingCompetition.Id,
            PreviousStageId = registrationStage.Id,
            Name = "Qualification Round",
            Description = "Online qualification round with algorithmic challenges. Top performers advance to the next stage.",
            DateStart = new DateTime(2024, 2, 5, 10, 0, 0, DateTimeKind.Utc),
            DateEnd = new DateTime(2024, 2, 5, 18, 0, 0, DateTimeKind.Utc)
        };
        context.EventStages.Add(qualificationStage);
        context.SaveChanges();

        var semiFinalStage = new EventStage
        {
            EventId = programmingCompetition.Id,
            PreviousStageId = qualificationStage.Id,
            Name = "Semi-Final Round",
            Description = "Regional semi-final competition held at multiple locations. Winners proceed to the grand finale.",
            DateStart = new DateTime(2024, 3, 15, 9, 0, 0, DateTimeKind.Utc),
            DateEnd = new DateTime(2024, 3, 15, 17, 0, 0, DateTimeKind.Utc)
        };
        context.EventStages.Add(semiFinalStage);
        context.SaveChanges();

        var finalStage = new EventStage
        {
            EventId = programmingCompetition.Id,
            PreviousStageId = semiFinalStage.Id,
            Name = "Grand Finale",
            Description = "The ultimate competition round where finalists compete for the championship title and prizes.",
            DateStart = new DateTime(2024, 4, 20, 9, 0, 0, DateTimeKind.Utc),
            DateEnd = new DateTime(2024, 4, 20, 18, 0, 0, DateTimeKind.Utc)
        };
        context.EventStages.Add(finalStage);
        context.SaveChanges();

        // Hackathon stages
        var hackathonRegistration = new EventStage
        {
            EventId = hackathonEvent.Id,
            PreviousStageId = null,
            Name = "Team Registration",
            Description = "Teams of 2-4 members register for the hackathon.",
            DateStart = new DateTime(2024, 5, 1, 0, 0, 0, DateTimeKind.Utc),
            DateEnd = new DateTime(2024, 5, 15, 23, 59, 59, DateTimeKind.Utc)
        };
        context.EventStages.Add(hackathonRegistration);
        context.SaveChanges();

        var hackathonMain = new EventStage
        {
            EventId = hackathonEvent.Id,
            PreviousStageId = hackathonRegistration.Id,
            Name = "Hackathon Development",
            Description = "48-hour intensive development phase where teams build their solutions.",
            DateStart = new DateTime(2024, 6, 1, 9, 0, 0, DateTimeKind.Utc),
            DateEnd = new DateTime(2024, 6, 3, 9, 0, 0, DateTimeKind.Utc)
        };
        context.EventStages.Add(hackathonMain);
        context.SaveChanges();

        var hackathonPresentation = new EventStage
        {
            EventId = hackathonEvent.Id,
            PreviousStageId = hackathonMain.Id,
            Name = "Project Presentation",
            Description = "Teams present their solutions to judges and compete for awards.",
            DateStart = new DateTime(2024, 6, 3, 10, 0, 0, DateTimeKind.Utc),
            DateEnd = new DateTime(2024, 6, 3, 16, 0, 0, DateTimeKind.Utc)
        };
        context.EventStages.Add(hackathonPresentation);
        context.SaveChanges();
    }

    private static void SeedEventStageCriterias(ContestDbContext context)
    {
        if (context.EventStageCriterias.Any())
        {
            return; // EventStageCriterias already seeded
        }

        // Get stages (assuming they exist)
        var qualificationStage = context.EventStages.FirstOrDefault(s => s.Name.Contains("Qualification"));
        var semiFinalStage = context.EventStages.FirstOrDefault(s => s.Name.Contains("Semi-Final"));
        var finalStage = context.EventStages.FirstOrDefault(s => s.Name.Contains("Grand Finale"));

        if (qualificationStage == null || semiFinalStage == null || finalStage == null)
        {
            return; // Cannot seed criteria without stages
        }

        var criteria1 = new EventStageCriteria
        {
            EventStageId = qualificationStage.Id,
            Name = "Problem Solving Accuracy",
            Description = "Correctness of solutions submitted for algorithmic problems.",
            IsRequired = true,
            MinimumThresholdScore = 70.00m,
            IsActiveUntil = new DateTime(2024, 2, 5, 18, 0, 0, DateTimeKind.Utc)
        };

        var criteria2 = new EventStageCriteria
        {
            EventStageId = qualificationStage.Id,
            Name = "Code Quality",
            Description = "Evaluation of code readability, efficiency, and best practices.",
            IsRequired = false,
            MinimumThresholdScore = 60.00m,
            IsActiveUntil = new DateTime(2024, 2, 5, 18, 0, 0, DateTimeKind.Utc)
        };

        var criteria3 = new EventStageCriteria
        {
            EventStageId = semiFinalStage.Id,
            Name = "Performance Optimization",
            Description = "Ability to optimize solutions for better time and space complexity.",
            IsRequired = true,
            MinimumThresholdScore = 75.00m,
            IsActiveUntil = new DateTime(2024, 3, 15, 17, 0, 0, DateTimeKind.Utc)
        };

        var criteria4 = new EventStageCriteria
        {
            EventStageId = finalStage.Id,
            Name = "Innovation & Creativity",
            Description = "Demonstration of innovative approaches and creative problem-solving.",
            IsRequired = true,
            MinimumThresholdScore = 80.00m,
            IsActiveUntil = new DateTime(2024, 4, 20, 18, 0, 0, DateTimeKind.Utc)
        };

        context.EventStageCriterias.AddRange(criteria1, criteria2, criteria3, criteria4);
        context.SaveChanges();
    }

    private static void SeedAttachments(ContestDbContext context)
    {
        if (context.Attachments.Any())
        {
            return; // Attachments already seeded
        }

        var attachment1 = new Attachment
        {
            Name = "Competition Rules and Regulations.pdf"
        };

        var attachment2 = new Attachment
        {
            Name = "Participant Registration Form.pdf"
        };

        var attachment3 = new Attachment
        {
            Name = "Code Submission Guidelines.pdf"
        };

        var attachment4 = new Attachment
        {
            Name = "Prize Structure and Awards.pdf"
        };

        var attachment5 = new Attachment
        {
            Name = "Technical Requirements.pdf"
        };

        context.Attachments.AddRange(attachment1, attachment2, attachment3, attachment4, attachment5);
        context.SaveChanges();
    }

    private static void SeedContestNotices(ContestDbContext context)
    {
        if (context.ContestNotices.Any())
        {
            return; // ContestNotices already seeded
        }

        // Get events (assuming they exist)
        var programmingCompetition = context.Events.FirstOrDefault(e => e.Name.Contains("Programming Championship"));
        var hackathonEvent = context.Events.FirstOrDefault(e => e.Name.Contains("Hackathon"));
        var aiContest = context.Events.FirstOrDefault(e => e.Name.Contains("AI"));

        if (programmingCompetition == null || hackathonEvent == null || aiContest == null)
        {
            return; // Cannot seed notices without events
        }

        var notice1 = new ContestNotice
        {
            OrganizerId = 1,
            EventId = programmingCompetition.Id,
            Goal = "To identify and reward the most talented programmers while fostering innovation in algorithmic problem-solving.",
            CompetitionType = "Individual Competition",
            Description = "Join us for the most prestigious programming competition of the year. Test your skills, compete with the best, and win amazing prizes!"
        };

        var notice2 = new ContestNotice
        {
            OrganizerId = 1,
            EventId = hackathonEvent.Id,
            Goal = "Encourage collaborative innovation and rapid prototyping of solutions to real-world challenges.",
            CompetitionType = "Team Competition",
            Description = "Build something amazing in 48 hours! Work with a team, use cutting-edge technologies, and make a difference."
        };

        var notice3 = new ContestNotice
        {
            OrganizerId = 2,
            EventId = aiContest.Id,
            Goal = "Advance AI research and provide a platform for data scientists to showcase their expertise.",
            CompetitionType = "Individual Competition",
            Description = "Challenge yourself with complex machine learning problems. Work with real datasets and build state-of-the-art models."
        };

        context.ContestNotices.AddRange(notice1, notice2, notice3);
        context.SaveChanges();
    }

    private static void SeedContestDocsPackages(ContestDbContext context)
    {
        if (context.ContestDocsPackages.Any())
        {
            return; // ContestDocsPackages already seeded
        }

        // Get notices and attachments (assuming they exist)
        var notice1 = context.ContestNotices.FirstOrDefault(n => n.CompetitionType == "Individual Competition" && n.OrganizerId == 1);
        var notice2 = context.ContestNotices.FirstOrDefault(n => n.CompetitionType == "Team Competition");
        var notice3 = context.ContestNotices.FirstOrDefault(n => n.OrganizerId == 2);

        var attachment1 = context.Attachments.FirstOrDefault(a => a.Name.Contains("Rules"));
        var attachment2 = context.Attachments.FirstOrDefault(a => a.Name.Contains("Registration"));
        var attachment3 = context.Attachments.FirstOrDefault(a => a.Name.Contains("Code Submission"));
        var attachment4 = context.Attachments.FirstOrDefault(a => a.Name.Contains("Prize"));
        var attachment5 = context.Attachments.FirstOrDefault(a => a.Name.Contains("Technical"));

        if (notice1 == null || notice2 == null || notice3 == null ||
            attachment1 == null || attachment2 == null || attachment3 == null ||
            attachment4 == null || attachment5 == null)
        {
            return; // Cannot seed packages without notices and attachments
        }

        var docsPackage1 = new ContestDocsPackage
        {
            ContestNoticeId = notice1.Id,
            AttachmentId = attachment1.Id,
            IsRequired = true,
            Comment = "All participants must read and agree to the competition rules."
        };

        var docsPackage2 = new ContestDocsPackage
        {
            ContestNoticeId = notice1.Id,
            AttachmentId = attachment2.Id,
            IsRequired = true,
            Comment = "Required for registration process."
        };

        var docsPackage3 = new ContestDocsPackage
        {
            ContestNoticeId = notice1.Id,
            AttachmentId = attachment3.Id,
            IsRequired = true,
            Comment = "Important guidelines for code submission."
        };

        var docsPackage4 = new ContestDocsPackage
        {
            ContestNoticeId = notice1.Id,
            AttachmentId = attachment4.Id,
            IsRequired = false,
            Comment = "Information about prizes and awards."
        };

        var docsPackage5 = new ContestDocsPackage
        {
            ContestNoticeId = notice2.Id,
            AttachmentId = attachment1.Id,
            IsRequired = true,
            Comment = "Hackathon rules and regulations."
        };

        var docsPackage6 = new ContestDocsPackage
        {
            ContestNoticeId = notice2.Id,
            AttachmentId = attachment5.Id,
            IsRequired = true,
            Comment = "Technical requirements for hackathon projects."
        };

        var docsPackage7 = new ContestDocsPackage
        {
            ContestNoticeId = notice3.Id,
            AttachmentId = attachment1.Id,
            IsRequired = true,
            Comment = "AI competition rules."
        };

        var docsPackage8 = new ContestDocsPackage
        {
            ContestNoticeId = notice3.Id,
            AttachmentId = attachment3.Id,
            IsRequired = true,
            Comment = "Model submission guidelines."
        };

        context.ContestDocsPackages.AddRange(
            docsPackage1, docsPackage2, docsPackage3, docsPackage4,
            docsPackage5, docsPackage6, docsPackage7, docsPackage8);
        context.SaveChanges();
    }
}
