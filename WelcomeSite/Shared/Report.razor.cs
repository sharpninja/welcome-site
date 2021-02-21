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
        IEnumerable<SurveyResponse> _responses;
        private SfGrid<SurveyResponse> _grid;
        private SfButton _button;

        private IEnumerable<SurveyResponse> Responses
        {
            // Populate Responses if necessary.
            get => _responses ??= GetResponses();                
            set => _responses = value;
        }

        private IEnumerable<SurveyResponse> GetResponses() =>
            DefaultContext.SurveyResponses
                    .Where(r => !string.IsNullOrWhiteSpace(r.ResponseText))
                    .Include(r => r.Question)
                    .Include(r => r.Respondent)
                    .OrderBy(r => r.Respondent.EmailAddress)
                    .ThenBy(r => r.Question.QuestionOrder)
                    .ToList()
                    .Where(r => DefaultContext.Entry<SurveyResponse>(r).State != EntityState.Deleted);

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

                _responses = GetResponses();

                _grid.Refresh();
            }
        }
    }
}
