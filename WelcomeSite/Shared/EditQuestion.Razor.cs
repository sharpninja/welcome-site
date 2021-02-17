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
    public partial class EditQuestion
    {
        private bool _isNew = true;
        private Guid _questionId;
        private SurveyQuestion _question;

        [Inject]
        public ILogger<EditQuestion> Logger { get; set; }

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

        [Parameter]
        public SurveyQuestion Question
        {
            get => _question ??= new SurveyQuestion { QuestionOrder = DefaultContext.NextQuestionOrder };
            set
            {
                _question = value;
                this.StateHasChanged();
            }
        }

        public string QuestionTitle
        {
            get => Question.QuestionTitle;
            set
            {
                Question.QuestionTitle = value;
                this.StateHasChanged();
            }
        }

        public string QuestionText
        {
            get => Question.QuestionText;
            set
            {
                Question.QuestionText = value;
                this.StateHasChanged();
            }
        }

        public decimal QuestionOrder
        {
            get => Question.QuestionOrder;
            set
            {
                QuestionOrderClass =
                (DefaultContext.SurveyQuestions.Any(q => q.QuestionOrder == value)) ? "Error" : "";

                Question.QuestionOrder = value;
                this.StateHasChanged();
            }
        }

        public string QuestionOrderClass { get; set; } = "";

        [Inject]
        ApplicationDbContext DefaultContext
        {
            get; set;
        }

        [Parameter]
        public Action<Guid> QuestionIDChanged { get; set; }

        SfRichTextEditor Editor { get; set; }

        public bool IsDisabled
        {
            get;
            set;
        }

        public void OnChange()
        {
            Question.QuestionText = Editor.Value;

            IsDisabled = string.IsNullOrWhiteSpace(Question.QuestionText);
            this.StateHasChanged();
        }

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
                    this.StateHasChanged();

                    Toast.Content = Status;
                    Toast.Show();

                    NavManager.NavigateTo<WelcomeSite.Pages.ListQuestions>();
                }
            }
        }

        public string Status
        {
            get;
            set;
        }

        public SfToast Toast { get; set; }
    }
}