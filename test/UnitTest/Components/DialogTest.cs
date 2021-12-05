// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using BootstrapBlazor.Components;
using Bunit;
using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using UnitTest.Core;
using Xunit;

namespace UnitTest.Components
{
    public class DialogTest : BootstrapBlazorTestBase
    {
        [Fact]
        public void Show_Ok()
        {
            var closed = false;
            var cut = Context.RenderComponent<BootstrapBlazorRoot>(pb =>
            {
                pb.AddChildContent<MockDialogTest>();
            });

            var dialog = cut.FindComponent<MockDialogTest>().Instance.DialogService;
            cut.InvokeAsync(() => dialog.Show(new DialogOption()
            {
                BodyTemplate = builder => builder.AddContent(0, "Test-BodyTemplate"),
                HeaderTemplate = builder => builder.AddContent(0, "Test-HeaderTemplate"),
                FooterTemplate = builder => builder.AddContent(0, "Test-FooterTemplate"),
                Class = "test-class",
                OnCloseAsync = () =>
                {
                    closed = true;
                    return Task.CompletedTask;
                }
            }));
            Assert.Contains("Test-BodyTemplate", cut.Markup);
            Assert.Contains("Test-HeaderTemplate", cut.Markup);
            Assert.Contains("Test-FooterTemplate", cut.Markup);
            Assert.Contains("test-class", cut.Markup);
            var modal = cut.FindComponent<Modal>();
            cut.InvokeAsync(() => modal.Instance.Close());
            Assert.True(closed);

            cut.InvokeAsync(() => dialog.Show(new DialogOption()
            {
                Component = BootstrapDynamicComponent.CreateComponent<Button>(),
                BodyTemplate = null
            }));
            Assert.Contains("class=\"btn btn-primary\"", cut.Markup);

            cut.InvokeAsync(() => dialog.Show(new DialogOption()
            {
                Component = null,
                BodyTemplate = null
            }));
            Assert.Contains("class=\"btn btn-primary\"", cut.Markup);
        }

        private class MockDialogTest : ComponentBase
        {
            [Inject]
            [NotNull]
            public DialogService? DialogService { get; set; }
        }
    }
}
