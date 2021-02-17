
using System;
using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

namespace WelcomeSite
{
    public class SurveyResponse
    {
        [Key]
        public Guid ResponseID { get; set; } = Guid.NewGuid();

        public Guid RespondentID { get; set; }
        public Respondent Respondent {get;set;}
        public Guid QuestionID { get; set; }
        public SurveyQuestion Question { get; set; }
        public string ResponseText { get; set; }

        public DateTimeOffset ResponseCreatedDateTimeUtc { get; set; } = DateTimeOffset.UtcNow;

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
