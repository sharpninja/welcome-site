using System;
using System.Linq;
using System.Net;

using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;
using WelcomeSite.Data;

namespace WelcomeSite
{
    /// <summary>
    /// Contains a single Survey Question and its
    /// metadata.
    /// </summary>
    public class SurveyQuestion
    {
        /// <summary>
        /// Primary key.
        /// </summary>
        [Key]
        public Guid QuestionID { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Creation <seealso cref="DateTimeOffset"/>.
        /// </summary>
        public DateTimeOffset QuestionCreatedDateTimeUtc { get; set; } = DateTimeOffset.UtcNow;

        /// <summary>
        /// Title of the question.  User sees this.
        /// </summary>
        public string QuestionTitle { get; set; }

        /// <summary>
        /// Text of the question.  May include Markdown.
        /// </summary>
        public string QuestionText { get; set; }

        /// <summary>
        /// Hides a question.
        /// </summary>
        public bool QuestionIsPrivate { get; set; }

        /// <summary>
        /// Order that question displays in relative to other
        /// questions.  Lowest is displayed first.
        /// </summary>
        public decimal QuestionOrder { get; set; }

        /// <summary>
        /// List of all <see cref="SurveyResponse"/> for the question.
        /// </summary>
        public List<SurveyResponse> Responses { get; set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
