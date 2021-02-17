
using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WelcomeSite
{
    [Index(nameof(EmailAddress), IsUnique = true)]
    public class Respondent
    {
        private Guid respondentID = Guid.NewGuid();

        [Key]
        public Guid RespondentID
        {
            get => respondentID;
            set
            {
                respondentID = value;
                if(string.IsNullOrEmpty(EmailAddress))
                {
                    EmailAddress = $"{value}@mailinator.com";
                }
            }
        }
        public DateTimeOffset RespondentCreatedDateTimeUtc { get; set; } = DateTimeOffset.UtcNow;
        public string EmailAddress { get; set; }
        public string Preferneces { get; set; }

        public List<SurveyResponse> Responses { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
