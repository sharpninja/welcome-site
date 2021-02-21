using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Syncfusion.Blazor.RichTextEditor;
using Syncfusion.Blazor.Notifications;

using System;
using System.Linq;

using WelcomeSite.Data;

namespace WelcomeSite.Shared
{
    /// <summary>
    /// Code behind of the editor.
    /// </summary>
    public partial class EditQuestion
    {
        private bool _isNew = true;
        private Guid _questionId;
        private SurveyQuestion _question;

        /// <summary>
        /// Injected logger.
        /// </summary>
        [Inject]
        public ILogger<EditQuestion> Logger { get; set; }

        /// <summary>
        /// Question Key Property
        /// </summary>
        [Parameter]
        public Guid QuestionID
        {
            get => _questionId;
            set
            {
                if (_questionId == value) return;

                _isNew = value == default;

                _questionId = value;
                _question = DefaultContext.SurveyQuestions.FirstOrDefault(sq => sq.QuestionID == value);
            }
        }

        /// <summary>
        /// Question Entity Property
        /// </summary>
        [Parameter]
        public SurveyQuestion Question
        {
            get => _question ??= new SurveyQuestion { QuestionOrder = DefaultContext.NextQuestionOrder };
            set
            {
                _question = value;
                StateHasChanged();
            }
        }

        /// <summary>
        /// Question Title property.
        /// </summary>
        public string QuestionTitle
        {
            get => Question.QuestionTitle;
            set
            {
                Question.QuestionTitle = value;
                StateHasChanged();
            }
        }

        /// <summary>
        /// Question Text property
        /// </summary>
        public string QuestionText
        {
            get => Question.QuestionText;
            set
            {
                Question.QuestionText = value;
                StateHasChanged();
            }
        }

        /// <summary>
        /// Question Order property.
        /// </summary>
        public decimal QuestionOrder
        {
            get => Question.QuestionOrder;
            set
            {
                QuestionOrderClass =
                (DefaultContext.SurveyQuestions.Any(q => q.QuestionOrder == value)) ? "Error" : "";

                Question.QuestionOrder = value;
                StateHasChanged();
            }
        }

        /// <summary>
        /// Css based on the QuestionOrder value.
        /// </summary>
        public string QuestionOrderClass { get; set; } = "";

        /// <summary>
        /// Database Context
        /// </summary>
        [Inject]
        ApplicationDbContext DefaultContext
        {
            get; set;
        }

        /// <summary>
        /// Ref to the editor.
        /// </summary>
        SfRichTextEditor Editor { get; set; }

        /// <summary>
        /// Don't allow saving an empty question.
        /// </summary>
        public bool IsDisabled
        {
            get;
            set;
        }

        /// <summary>
        /// Syncs the model and manages the Save button state.
        /// </summary>
        public void OnChange()
        {
            Question.QuestionText = Editor.Value;

            IsDisabled = string.IsNullOrWhiteSpace(Question.QuestionText);
            StateHasChanged();
        }

        /// <summary>
        /// Saves current question.
        /// </summary>
        public void Save()
        {
            if (!IsDisabled)
            {
                IsDisabled = true;

                bool wasNew = _isNew;

                if (QuestionID == default)
                {
                    QuestionID = Guid.NewGuid();                    
                }

                if(QuestionID != Question.QuestionID)
                {
                    Question.QuestionID = QuestionID;
                }

                Question.QuestionText = Editor.Value;

                var result = wasNew switch
                {
                    true => DefaultContext.Add<SurveyQuestion>(Question),
                    _ => DefaultContext.Update<SurveyQuestion>(Question)
                };

                if (result != null)
                {
                    var rows = DefaultContext.SaveChanges(true);

                    if (rows != 1)
                    {
                        throw new ApplicationException($"Saved {rows} rows for {nameof(SurveyQuestion)}.");
                    }

                    Logger.LogInformation($"Updated {rows} rows for {nameof(SurveyQuestion)} :: {QuestionID}");
                    Logger.LogDebug(result.DebugView.LongView);

                    Status = $"Saved {rows} records at {DateTime.Now.ToLongTimeString()}";
                    StateHasChanged();

                    Toast.Show();

                    NavManager.NavigateTo<ListQuestions>();
                }
            }
        }

        /// <summary>
        /// Status of the operation.
        /// </summary>
        public string Status
        {
            get;
            set;
        }

        /// <summary>
        /// Accessor for the Toast notifications.
        /// </summary>
        public SfToast Toast { get; set; }
    }
}