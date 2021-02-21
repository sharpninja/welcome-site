using Microsoft.AspNetCore.Components;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WelcomeSite.Shared
{
    /// <summary>
    /// Code behind of the editor wrapper.
    /// </summary>
    public partial class Editor
    {
        /// <summary>
        /// Injected <see cref="NavManager"/>
        /// </summary>
        [Inject]
        public Services.NavManager NavManager
        {
            get;
            set;
        }

        /// <summary>
        /// Page Initialized
        /// </summary>
        protected override void OnInitialized()
        {
            QuestionId = NavManager.Args.Cast<Guid>().FirstOrDefault();
            base.OnInitialized();
        }

        private Guid QuestionId { get; set; }
    }
}
