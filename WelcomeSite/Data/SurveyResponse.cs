
using System;
using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

namespace WelcomeSite.Data
{
    /// <summary>
    /// A response by a single <see cref="Respondent"/>
    /// to a single <see cref="SurveyQuestion"/>.
    /// </summary>
    public class SurveyResponse
    {
        /// <summary>
        /// Primary Key.
        /// </summary>
        [Key]
        public Guid ResponseID { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Foriegn Key to <see cref="Respondent"/>
        /// </summary>
        public Guid RespondentID { get; set; }

        /// <summary>
        /// Related <see cref="Respondent"/>.
        /// </summary>
        public Respondent Respondent { get; set; }

        /// <summary>
        /// Foreign Key to <see cref="SurveyQuestion"/>.
        /// </summary>
        public Guid QuestionID { get; set; }

        /// <summary>
        /// Related <see cref="SurveyQuestion"/>.
        /// </summary>
        public SurveyQuestion Question { get; set; }

        /// <summary>
        /// Repondent's answer.  May contain Markdown.
        /// </summary>
        public string ResponseText { get; set; }

        /// <summary>
        /// Creation <seealso cref="DateTimeOffset"/>.
        /// </summary>
        public DateTimeOffset ResponseCreatedDateTimeUtc { get; set; } = DateTimeOffset.UtcNow;

        /// <inheritdoc/>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
