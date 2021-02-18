using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Syncfusion.Blazor.Buttons;
using Syncfusion.Blazor.Grids;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WelcomeSite.Shared
{
    public partial class Report
    {
        SfButton Button { get; set; }
        IEnumerable<SurveyResponse> _responses = null;

        private IEnumerable<SurveyResponse> Responses
        {
            get => _responses ??=
                DefaultContext.SurveyResponses
                    .Where(r => !string.IsNullOrWhiteSpace(r.ResponseText))
                    .Include(r => r.Question)
                    .Include(r => r.Respondent)
                    .OrderBy(r => r.Respondent.EmailAddress)
                    .ThenBy(r => r.Question.QuestionOrder)
                    .ToList()
                    .Where(r => DefaultContext.Entry<SurveyResponse>(r).State != EntityState.Deleted);
            set => _responses = value;
        }

        private SfGrid<SurveyResponse> _grid = null;

        private SfGrid<SurveyResponse> DataGrid
        {
            get => _grid;
            set => _grid = value;
        }

        private void DeleteSelected()
        {
            if (DataGrid.SelectedRecords.Any())
            {
                var toDelete = DataGrid.SelectedRecords;
                toDelete.ForEach(td =>
                {
                    td.Question = null;
                    td.Respondent = null;
                    td.QuestionID = default;
                    td.RespondentID = default;
                });

                DefaultContext.RemoveRange(toDelete);

                var rows = DefaultContext.SaveChanges(true);

                Logger.LogInformation($"Deleted {rows} SurveyResponses");

                Responses = DefaultContext.SurveyResponses
                    .Where(r => !string.IsNullOrWhiteSpace(r.ResponseText))
                    .Include(r => r.Question)
                    .Include(r => r.Respondent)
                    .OrderBy(r => r.Respondent.EmailAddress)
                    .ThenBy(r => r.Question.QuestionOrder)
                    .ToList()
                    .Where(r => DefaultContext.Entry<SurveyResponse>(r).State != EntityState.Deleted);

                DataGrid.Refresh();
            }
        }
    }
}
