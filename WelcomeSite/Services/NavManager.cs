using Microsoft.AspNetCore.Razor.TagHelpers;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using WelcomeSite.Shared;
using WelcomeSite.Pages;
using Microsoft.AspNetCore.Components;

namespace WelcomeSite.Services
{
    public class NavManager
    {
        public Type NavigateTo<TComponent>(params object[] args)
            where TComponent : Microsoft.AspNetCore.Components.IComponent
        {
            Args = args;

            var t = _widgets.TryGetValue(typeof(TComponent).Name, out var value)
                ? value
                : null;

            RenderWidget?.Invoke(this, t);

            return t;
        }

        public object[] Args { get; set; }

        public Type NavigateTo(string typeName)
        {
            var t = _widgets[typeName];

            RenderWidget?.Invoke(this, t);

            return t;
        }

        public event EventHandler<Type> RenderWidget;

        private readonly Dictionary<string, Type> _widgets =
            new()
            {
                [nameof(ListQuestions)] = typeof(ListQuestions),
                [nameof(Report)] = typeof(Report),
                [nameof(Thanks)] = typeof(Thanks),
                [nameof(Survey)] = typeof(Survey),
                [nameof(Editor)] = typeof(Editor),
                [nameof(Contact)] = typeof(Contact),
                [nameof(Links)] = typeof(Links),
                [nameof(Welcome)] = typeof(Welcome),
            };
    }
}
