using Microsoft.AspNetCore.Components;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WelcomeSite.Shared
{
    public partial class Editor
    {
        [Inject]
        public WelcomeSite.Services.NavManager NavManager
        {
            get;
            set;
        }

        protected override void OnInitialized()
        {
            _questionId = NavManager.Args.Cast<Guid>().FirstOrDefault();
            base.OnInitialized();
        }

        private Guid _questionId { get; set; }


        [Parameter]
        public String QuestionID
        {
            get => _questionId.ToString() ?? Guid.Empty.ToString();
            set => _questionId = Guid.Parse(value ?? Guid.Empty.ToString());
        }
    }
}
