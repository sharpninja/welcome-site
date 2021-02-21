using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using WelcomeSite.Data;

namespace WelcomeSite.Shared
{
    public partial class ListQuestions
    {
        private IEnumerable<SurveyQuestion> _dataSource;

        public IEnumerable<SurveyQuestion> DataSource => _dataSource ??=
            DefaultContext.SurveyQuestions.OrderBy(q => q.QuestionOrder);

        protected override void OnInitialized()
        {
            _dataSource ??= DefaultContext.SurveyQuestions.OrderBy(q => q.QuestionOrder);
            base.OnInitialized();
        }

        public void OnClick()
        {
            NavManager.NavigateTo<Editor>();
        }
    }
}
