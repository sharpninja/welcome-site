using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

using Newtonsoft.Json;

using Syncfusion.Blazor.Navigations;
using Syncfusion.Blazor.Popups;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using WelcomeSite.Data;
using WelcomeSite.Services;

namespace WelcomeSite.Shared
{
    public partial class NavMenu
    {
        // Property Backing Fields
        private SidebarPosition _sbPosition = SidebarPosition.Left;
        private bool _sidebarVisibility;

        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }

        [Inject]
        public NavManager NavManager { get; set; }

        [Inject]
        public ApplicationDbContext DefaultContext { get; set; }

        SfSidebar SidebarObj { get; set; }
        SfDialog SettingsDialog { get; set; }

        private Dictionary<string, object> HtmlAttribute { get; set; } =
            new Dictionary<string, object>
            {
                 {"class", "default-sidebar" }
            };

        private bool IsLoggedIn { get; set; }
        private bool IsAdminUser { get; set; }
        private string Hamburgerclass { get; set; } = "e-icons e-nav default";
        private string Toprowclass { get; set; } = "top-row left";

        private SidebarPosition Position
        {
            get => _sbPosition;
            set
            {
                if (_sbPosition != value)
                {
                    _sbPosition = value;

                    IsChecked = _sbPosition == SidebarPosition.Left;

                    Toprowclass = $"top-row {value.ToString().ToLower()}";

                    StateHasChanged();

                    Settings.Save(this);
                }
            }
        }

        private bool SidebarVisibility
        {
            get => _sidebarVisibility;
            set
            {
                if (_sidebarVisibility != value)
                {
                    _sidebarVisibility = value;
                    Settings.Save(this);
                }
            }
        }

        private bool DialogVisibility { get; set; }
        private bool ButtonVisibility { get; set; } = true;
        private string CenterX { get; set; }
        private string CenterY { get; set; }
        private bool IsChecked { get; set; }

        private Respondent Respondent { get; set; }

        public class Settings
        {
            public SidebarPosition Position { get; set; }
            public bool SidebarVisibility { get; set; }

            public static void Save(NavMenu parent)
            {
                if (parent.Respondent is null)
                {
                    return;
                }

                var settings = new Settings
                {
                    Position = parent.Position,
                    SidebarVisibility = parent.SidebarVisibility
                };

                var json = JsonConvert.SerializeObject(settings);

                parent.Respondent.Preferneces = json;

                parent.DefaultContext.Update(parent.Respondent);
                parent.DefaultContext.SaveChanges(true);

            }

            public static void Load(NavMenu parent)
            {
                var json = parent.Respondent.Preferneces;

                if (string.IsNullOrWhiteSpace(json))
                {
                    return;
                }

                var settings = JsonConvert.DeserializeObject<Settings>(json);

                parent.Position = settings.Position;
                parent.SidebarVisibility = settings.SidebarVisibility;
            }
        }

        /// <summary>
        /// Set the <see cref="IsAdminUser"/> value;
        /// </summary>
        /// <returns><seealso cref="Task"/></returns>
        private async Task SetIsAdminUser()
        {
            var authState = await authenticationStateTask.ConfigureAwait(false);
            var user = authState.User;

            if (user.Identity.IsAuthenticated)
            {
                IsLoggedIn = true;

                var admin = Startup.ApplicationConfiguration["AdminEmail"];
                IsAdminUser = user.Identity.Name == admin;

            }
            else
            {
                IsLoggedIn = false;
                IsAdminUser = false;
            }

            StateHasChanged();
        }

        /// <summary>
        /// Save Contents of Dialog
        /// </summary>
        private void OnBtnSaveClick()
        {
            Position = IsChecked ? SidebarPosition.Left : SidebarPosition.Right;
            SettingsDialog.Hide();
        }

        /// <summary>
        /// Cancel Dialog.
        /// </summary>
        private void OnCancel()
        {
            SettingsDialog.Hide();
        }

        /// <summary>
        /// Show Settings from Button Click
        /// </summary>
        private void OnShowSettingsBtnClick()
        {
            SettingsDialog.Show(false);
        }

        /// <summary>
        /// Close the Sidebar.
        /// </summary>
        private void CloseSidebar()
        {
            SidebarVisibility = false;
        }

        /// <summary>
        /// Toggle the Sidebar.
        /// </summary>
        private void ToggleSidebar()
        {
            SidebarVisibility = !SidebarVisibility;
        }

        /// <inheritdoc/>
        protected override async Task OnInitializedAsync()
        {
            var authState = await authenticationStateTask.ConfigureAwait(false);
            var user = authState.User;

            if (user.Identity.IsAuthenticated)
            {
                var email = user.Identity.Name;
                Respondent = DefaultContext.Respondents.FirstOrDefault(r => r.EmailAddress == email);

                Settings.Load(this);
            }

            await SetIsAdminUser().ConfigureAwait(false);

            await base.OnInitializedAsync();
        }

        /// <inheritdoc/>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            base.OnAfterRender(firstRender);

            var dims = await Service.GetDimensions();
            CenterX = (dims.Width / 2.0f - 125.0f).ToString("N1");
            CenterY = (dims.Height / 2.0f - 125.0f).ToString("N1");
        }

        private void GoHome()
        {
            NavManager.NavigateTo<Welcome>();
        }

        private void GoLinks()
        {
            NavManager.NavigateTo<Links>();
        }

        private void GoSurvey()
        {
            NavManager.NavigateTo<Survey>();
        }

        private void GoList()
        {
            NavManager.NavigateTo<ListQuestions>();
        }

        private void GoResults()
        {
            NavManager.NavigateTo<Report>();
        }

    }
}
