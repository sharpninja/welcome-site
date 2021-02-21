
using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WelcomeSite.Data
{
    /// <summary>
    /// Respondent is an individual responding to the survey.
    /// </summary>
    public class Respondent
    {
        private Guid respondentID = Guid.NewGuid();

        /// <summary>
        /// Primary Key
        /// </summary>
        [Key]
        public Guid RespondentID
        {
            get => respondentID;
            set
            {
                respondentID = value;
                if (string.IsNullOrEmpty(EmailAddress))
                {
                    EmailAddress = $"{value}@mailinator.com";
                }
            }
        }

        /// <summary>
        /// Creation <seealso cref="DateTimeOffset"/>.
        /// </summary>
        public DateTimeOffset RespondentCreatedDateTimeUtc { get; set; } = DateTimeOffset.UtcNow;

        /// <summary>
        /// Email address of the User Identity
        /// associated with this Respondent.
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Preferences stored in JSON format.
        /// </summary>
        public string Preferneces { get; set; }

        /// <summary>
        /// List of <see cref="Response"/> from this
        /// Respondent.
        /// </summary>
        public List<SurveyResponse> Responses { get; set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
