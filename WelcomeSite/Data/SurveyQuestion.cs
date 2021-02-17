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
    public class SurveyQuestion
    {
        [Key]
        public Guid QuestionID { get; set; } = Guid.NewGuid();
        public DateTimeOffset QuestionCreatedDateTimeUtc { get; set; } = DateTimeOffset.UtcNow;
        public string QuestionTitle { get; set; }
        public string QuestionText { get; set; }
        public bool QuestionIsPrivate { get; set; }
        public decimal QuestionOrder { get; set; }
        public List<SurveyResponse> Responses { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
