// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using BootstrapBlazor.Components;
using BootstrapBlazor.Shared;
using Bunit;
using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
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
            var cut = Context.RenderComponent<BootstrapBlazorRoot>(pb =>
            {
                pb.AddChildContent<MockDialogTest>();
            });
            var dialog = cut.FindComponent<MockDialogTest>().Instance.DialogService;

            var closed = false;
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

            // 代码覆盖模板单元测试
            Assert.Contains("Test-BodyTemplate", cut.Markup);
            Assert.Contains("Test-HeaderTemplate", cut.Markup);
            Assert.Contains("Test-FooterTemplate", cut.Markup);
            Assert.Contains("test-class", cut.Markup);

            // 测试关闭逻辑
            var modal = cut.FindComponent<Modal>();
            cut.InvokeAsync(() => modal.Instance.Close());
            Assert.True(closed);

            // 测试 Component 赋值逻辑
            cut.InvokeAsync(() => dialog.Show(new DialogOption()
            {
                Component = BootstrapDynamicComponent.CreateComponent<Button>(),
                BodyTemplate = null
            }));
            Assert.Contains("class=\"btn btn-primary\"", cut.Markup);
            modal = cut.FindComponent<Modal>();
            cut.InvokeAsync(() => modal.Instance.Close());

            // 测试 Component 与 BodyTemplate 均为 null 逻辑
            cut.InvokeAsync(() => dialog.Show(new DialogOption()
            {
                Component = null,
                BodyTemplate = null
            }));
            cut.InvokeAsync(() => modal.Instance.Close());

            // 无按钮回调赋值
            var option = new SearchDialogOption<Foo>()
            {
                Title = "Test-SearchDialogTitle",
                Model = new Foo(),
                ItemsPerRow = 2,
                RowType = RowType.Inline
            };
            cut.InvokeAsync(() => dialog.ShowSearchDialog(option));
            cut.InvokeAsync(() => modal.Instance.Close());

            // 重置按钮
            var reset = false;
            option.OnResetSearchClick = () =>
            {
                reset = true;
                return Task.CompletedTask;
            };
            cut.InvokeAsync(() => dialog.ShowSearchDialog(option));
            var button = cut.FindComponents<Button>().First(b => b.Instance.Text == "重置");
            cut.InvokeAsync(() => button.Instance.OnClickWithoutRender!.Invoke());
            Assert.True(reset);
            cut.InvokeAsync(() => modal.Instance.Close());

            // 搜索按钮
            var search = false;
            option.DialogBodyTemplate = foo => builder => builder.AddContent(0, foo.Name);
            option.OnSearchClick = () =>
            {
                search = true;
                return Task.CompletedTask;
            };
            cut.InvokeAsync(() => dialog.ShowSearchDialog(option));
            button = cut.FindComponents<Button>().First(b => b.Instance.Text == "查询");
            cut.InvokeAsync(() => button.Instance.OnClickWithoutRender!.Invoke());
            Assert.True(search);
            cut.InvokeAsync(() => modal.Instance.Close());
        }

        private class MockDialogTest : ComponentBase
        {
            [Inject]
            [NotNull]
            public DialogService? DialogService { get; set; }
        }
    }
}
