using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Syncfusion.Blazor.Buttons;
using Syncfusion.Blazor.Grids;

using System.Collections.Generic;
using System.Linq;

using WelcomeSite.Data;

namespace WelcomeSite.Shared
{
    /// <summary>
    /// Displays Survey Results
    /// </summary>
    public partial class Report
    {
        // Property backers
        IEnumerable<SurveyResponse> _responses = null;
        private SfGrid<SurveyResponse> _grid = null;
        private SfButton _button = null;

        private IEnumerable<SurveyResponse> Responses
        {
            // Populate Responses if necessary.
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

        public SfButton Button
        {
            get => _button;
            set => _button = value;
        }

        private SfGrid<SurveyResponse> DataGrid
        {
            get => _grid;
            set => _grid = value;
        }

        /// <summary>
        /// Delete the selected responses.
        /// </summary>
        private void DeleteSelected()
        {
            if (_grid.SelectedRecords.Any())
            {
                var toDelete = _grid.SelectedRecords;
                toDelete.ForEach(td =>
                {
                    // Remove foreign references first!!!
                    td.Question = null;
                    td.Respondent = null;
                    td.QuestionID = default;
                    td.RespondentID = default;
                });

                DefaultContext.RemoveRange(toDelete);

                var rows = DefaultContext.SaveChanges(true);

                Logger.LogInformation($"Deleted {rows} SurveyResponses");

                _responses = DefaultContext.SurveyResponses
                    .Where(r => !string.IsNullOrWhiteSpace(r.ResponseText))
                    .Include(r => r.Question)
                    .Include(r => r.Respondent)
                    .OrderBy(r => r.Respondent.EmailAddress)
                    .ThenBy(r => r.Question.QuestionOrder)
                    .ToList()
                    .Where(r => DefaultContext.Entry<SurveyResponse>(r).State != EntityState.Deleted);

                _grid.Refresh();
            }
        }
    }
}
