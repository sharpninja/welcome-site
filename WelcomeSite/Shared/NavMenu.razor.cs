using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Syncfusion.Blazor.Navigations;
using Syncfusion.Blazor.Buttons;
using Syncfusion.Blazor.Popups;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components;

using WelcomeSite.Services;
using WelcomeSite.Pages;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Http;

namespace WelcomeSite.Shared
{
    public partial class NavMenu
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public NavManager NavManager { get; set; }

        SfSidebar SidebarObj;
        SfDialog Dialog;
        public string Leftbtn = "Left";
        public string Hamburgerclass = "e-icons e-nav default";
        public string Toprowclass { get; set; } = "top-row left";

        private SidebarPosition _sbPosition = SidebarPosition.Left;

        public SidebarPosition Position 
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

                    Task.Run(async () =>
                    {
                        await JSRuntime.InvokeAsync<string>(
                            "blazorExtensions.WriteCookie",
                            "Position",
                            value.ToString(),
                            30);
                    });
                }
            }
        } 
        public bool SidebarToggle = false;
        private bool Visibility { get; set; } = false;
        private bool ShowButton { get; set; } = true;

        private SfRadioButton<bool> LeftRadio { get; set; }

        private void CheckedChanged(ChangeArgs<bool> e)
        {
            IsChecked = !IsChecked;
        }

        private void OnBtnSaveClick()
        {
            Position = IsChecked ? SidebarPosition.Left : SidebarPosition.Right;
            Dialog.Hide();
        }
        private void OnCancel()
        {
            Dialog.Hide();
        }
        private void OnBtnClick()
        {
            this.Visibility = true;
        }
        private void DialogOpen(Object args)
        {
            this.ShowButton = false;
        }
        private void DialogClose(Object args)
        {
            this.ShowButton = true;
        }

        public void Show()
        {
            SidebarToggle = true;
        }
        public void Close()
        {
            SidebarToggle = false;
        }
        public void Toggle()
        {
            SidebarToggle = !SidebarToggle;
        }

        [Inject]
        public IHttpContextAccessor HttpContextAccessor { get; set; }
        private HttpRequest Request => HttpContextAccessor.HttpContext.Request;

        protected override async Task OnInitializedAsync()
        {
            var cookie = Request.Cookies["Position"];

            if (cookie is not null)
            {
                Position = Enum.Parse<SidebarPosition>(cookie);
            }

            await SetIsAdminUser();

            await base.OnInitializedAsync();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);

            Task.Run(async () => { 
                var dims = await Service.GetDimensions();
                CenterX = (dims.Width / 2.0f - 125.0f).ToString("N1");
                CenterY = (dims.Height / 2.0f - 125.0f).ToString("N1");
            });
        }

        public void PositionChange(Syncfusion.Blazor.Buttons.ChangeArgs<string> args)
        {
            if (args.Value == "Left")
            {
                this.Position = SidebarPosition.Left;
                this.Hamburgerclass = "e-icons e-nav default";
                Toprowclass = "top-row left";
            }
            else
            {
                this.Position = SidebarPosition.Right;
                this.Hamburgerclass = "e-icons e-nav default e-rtl";
                Toprowclass = "top-row right";
            }
        }
        Dictionary<string, object> HtmlAttribute = new Dictionary<string, object>()
{
        {"class", "default-sidebar" }
    };

        private void GoHome()
        {
            //NavigationManager.NavigateTo("/", true);
            NavManager.NavigateTo<Welcome>();
        }

        private void GoLinks()
        {
            //NavigationManager.NavigateTo("/links", true);
            NavManager.NavigateTo<Links>();
        }

        private void GoSurvey()
        {
            //NavigationManager.NavigateTo("/survey", true);
            NavManager.NavigateTo<Survey>();
        }

        private void GoList()
        {
            //NavigationManager.NavigateTo("/list", true);
            NavManager.NavigateTo<ListQuestions>();
        }

        private void GoResults()
        {
            //NavigationManager.NavigateTo("/report", true);
            NavManager.NavigateTo<Report>();
        }

        private string CenterX { get; set; }
        private string CenterY { get; set; }
        public bool IsChecked { get; private set; }
    }
}
