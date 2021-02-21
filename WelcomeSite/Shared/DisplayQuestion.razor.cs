using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components.Web;

using Ganss.XSS;

using Markdig;

using Syncfusion.Blazor.RichTextEditor;

using WelcomeSite.Data;

using WelcomeSite.Services;
using WelcomeSite.Pages;
using Microsoft.AspNetCore.Components.Authorization;

namespace WelcomeSite.Shared
{
    public partial class DisplayQuestion
    {
        private SurveyQuestion _question;
        private SurveyResponse _response;
        private Respondent _respondent;

        [Inject]
        public NavManager NavManager { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            await JSRuntime.InvokeAsync<string>(
                "blazorExtensions.WriteCookie",
                "Respondent",
                RespondentId.ToString(),
                30);

            await base.OnParametersSetAsync();
        }

        [Inject]
        public IHttpContextAccessor HttpContextAccessor { get; set; }

        [Inject]
        public ILogger<DisplayQuestion> Logger { get; set; }

        [Parameter]
        public SurveyQuestion Question
        {
            get => _question;
            set
            {
                _question = value;

                var response = DefaultContext.SurveyResponses
                    .FirstOrDefault(r => r.QuestionID == _question.QuestionID &&
                    r.RespondentID == _respondent.RespondentID);

                Response = response;

                StateHasChanged();
            }
        }

        [Parameter]
        public SurveyResponse Response
        {
            get => _response ??= GetResponse();
            set
            {
                _response = value ?? GetResponse();

                StateHasChanged();
            }
        }

        private SurveyResponse MakeResponse()
        {
            var response = new SurveyResponse
            {
                ResponseID = Guid.NewGuid(),
                QuestionID = Question.QuestionID,
                RespondentID = _respondent.RespondentID,
                ResponseCreatedDateTimeUtc = DateTimeOffset.UtcNow
            };

            DefaultContext.Add<SurveyResponse>(response);
            DefaultContext.SaveChanges(true);

            return response;
        }

        public string QuestionTitle
        {
            get => Question.QuestionTitle;
            set
            {
                Question.QuestionTitle = value;
                StateHasChanged();
            }
        }

        public string RespondentEmail
        {
            get => _respondent.EmailAddress;
            set
            {
                var existing = DefaultContext.Respondents
                    .FirstOrDefault(r => r.EmailAddress == value );

                if (existing != null)
                {
                    _respondent = existing;
                }
                else
                {
                    _respondent.EmailAddress = value;
                }

                StateHasChanged();
            }
        }

        public string QuestionText
        {
            get => Question.QuestionText;
            set
            {
                Question.QuestionText = value;
                StateHasChanged();
            }
        }

        public string ResponseText
        {
            get => Response.ResponseText;
            set
            {
                Response.ResponseText = value;
                StateHasChanged();
            }
        }

        [Inject]
        ApplicationDbContext DefaultContext
        {
            get; set;
        }

        [Parameter]
        public Action<Guid> QuestionIDChanged { get; set; }

        SfRichTextEditor Editor { get; set; }

        public Guid RespondentId
        {
            get
            {
                if (_respondent is null)
                {
                    _respondent = MakeRespondent(Guid.NewGuid());
                }

                return _respondent.RespondentID;
            }
            set
            {
                if (value == Guid.Empty)
                {
                    _respondent = MakeRespondent(Guid.NewGuid());
                }
                else if (_respondent?.RespondentID != value)
                {
                    _respondent = DefaultContext.Respondents
                        .FirstOrDefault(r => r.RespondentID == value) ??
                        MakeRespondent(value);

                    StateHasChanged();
                }
            }
        }

        private Respondent MakeRespondent(Guid id)
        {
            var rsp = new Respondent { RespondentID = id };

            if (DefaultContext.Add<Respondent>(rsp) != null)
            {
                DefaultContext.SaveChanges(true);
            }

            return rsp;
        }

        private HttpRequest Request => HttpContextAccessor.HttpContext.Request;
        private HttpResponse HttpResponse => HttpContextAccessor.HttpContext.Response;

        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var authState = await authenticationStateTask;
            var user = authState.User;

            var cookie = Request.Cookies["Respondent"];

            RespondentId = Guid.TryParse(cookie, out var value) ? value : Guid.Empty;

            if (user.Identity.IsAuthenticated)
            {
                RespondentEmail = user.Identity.Name;
            }

            Question = DefaultContext.SurveyQuestions.OrderBy(q => q.QuestionOrder).FirstOrDefault();

            await base.OnInitializedAsync();
        }

        public void OnChange()
        {
            Response.ResponseText = Editor.Value;

            StateHasChanged();
        }

        public void Back()
        {
            if (DefaultContext.SurveyQuestions.Min(q => q.QuestionOrder) < Question.QuestionOrder)
            {
                Question = DefaultContext.SurveyQuestions
                    .Where(sq => sq.QuestionOrder < Question.QuestionOrder)
                    .OrderBy(sq => sq.QuestionOrder)
                    .LastOrDefault();
            }
        }

        public void Next()
        {
            if (DefaultContext.SurveyQuestions.Max(q => q.QuestionOrder) > Question.QuestionOrder)
            {
                Question = DefaultContext.SurveyQuestions
                    .Where(sq => sq.QuestionOrder > Question.QuestionOrder)
                    .OrderBy(sq => sq.QuestionOrder)
                    .FirstOrDefault();
            }
        }

        private SurveyResponse GetResponse()
        {
            var response =
                DefaultContext
                    .SurveyResponses
                    .FirstOrDefault(r => r.RespondentID == _respondent.RespondentID
                        && r.QuestionID == _question.QuestionID);

            response ??= MakeResponse();

            return response;
        }

        private void SaveNext(MouseEventArgs args) => Save(true);
        private void Save(MouseEventArgs args) => Save(false);

        private void Save(bool goNext)
        {
            Response.RespondentID = _respondent.RespondentID;

            var result = DefaultContext.Update<SurveyResponse>(Response);
            DefaultContext.Update<Respondent>(_respondent);

            var rows = DefaultContext.SaveChanges(true);

            if (rows < 1)
            {
                throw new ApplicationException($"Saved {rows} rows for {nameof(SurveyQuestion)}.");
            }

            Logger.LogInformation($"Updated {rows} rows for {nameof(SurveyQuestion)} :: {Question.QuestionID}");
            Logger.LogDebug(result.DebugView.LongView);

            Status = $"Saved {rows} records at {DateTime.Now.ToLongTimeString()}";
            StateHasChanged();

            if (goNext)
            {
                Next();
                return;
            }

            NavManager.NavigateTo<Thanks>();
        }

        public string Status
        {
            get;
            set;
        }

        [Inject]
        public IHtmlSanitizer HtmlSanitizer { get; set; }


        public MarkupString HtmlContent { get; private set; }

        private MarkupString ConvertStringToMarkupString(string value)
        {
            value ??= "";

            // Convert markdown string to HTML
            var html = Markdown.ToHtml(value,
                new MarkdownPipelineBuilder()
                    .UseAdvancedExtensions()
                    .Build());

            // Return sanitized HTML as a MarkupString that Blazor can render
            return new MarkupString(html);
        }
    }
}