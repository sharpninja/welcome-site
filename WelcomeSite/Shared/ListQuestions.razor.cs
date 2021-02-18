using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WelcomeSite.Shared
{
    public partial class ListQuestions
    {
        private IEnumerable<SurveyQuestion> _dataSource = null;

        public IEnumerable<SurveyQuestion> DataSource => _dataSource ??=
            DefaultContext.SurveyQuestions.OrderBy(q => q.QuestionOrder);

        protected override void OnInitialized()
        {
            _dataSource ??= DefaultContext.SurveyQuestions.OrderBy(q => q.QuestionOrder);
            base.OnInitialized();
        }

        public void OnClick()
        {
            //NavigationManager.NavigateTo("/editor");
            NavManager.NavigateTo<Editor>();
        }
    }
}
