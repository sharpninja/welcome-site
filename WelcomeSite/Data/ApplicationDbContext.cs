using System;
using System.Linq;
using System.Net;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WelcomeSite.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Respondent>()
                .HasKey(c => new { c.RespondentID })
                .HasName("PK_Respondent_RespondentId");

            modelBuilder.Entity<SurveyQuestion>()
                .HasKey(c => new { c.QuestionID })
                .HasName("PK_SurveyQuestion_QuestionId");

            var question = modelBuilder.Entity<SurveyResponse>();
            question.HasKey(c => new { c.ResponseID })
                .HasName("PK_SurveyResponse_ResponseId");

            modelBuilder.Entity<SurveyResponse>()
                .HasOne(p => p.Respondent)
                .WithMany(b => b.Responses)
                .HasForeignKey(p => p.RespondentID)
                .HasConstraintName("FK_SurveyResponse_Respondent");

            modelBuilder.Entity<SurveyResponse>()
                .HasOne(p => p.Question)
                .WithMany(b => b.Responses)
                .HasForeignKey(p => p.QuestionID)
                .HasConstraintName("FK_SurveyResponse_SurveyQuestion");            
        }

        public DbSet<Respondent> Respondents { get; set; }
        public DbSet<SurveyQuestion> SurveyQuestions { get; set; }
        public DbSet<SurveyResponse> SurveyResponses { get; set; }
        public decimal NextQuestionOrder => SurveyQuestions.Any()
            ? SurveyQuestions.Max(sq => sq.QuestionOrder) + 1.0m
            : 0.0m;

        public decimal HighestOrder
        {
            get
            {
                if (!SurveyQuestions.Any()) return 0.0m;

                return SurveyQuestions
                    .Max(q => q.QuestionOrder);
            }
        }

        public decimal LowestOrder
        {
            get
            {
                if (!SurveyQuestions.Any()) return 0.0m;

                return SurveyQuestions
                    .Min(q => q.QuestionOrder);
            }
        }
    }
}