// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using BootstrapBlazor.Components;
using Bunit;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnitTest.Core;
using Xunit;

namespace UnitTest.Components
{
    public class MenuTest : BootstrapBlazorTestBase
    {
        private List<MenuItem> Items { get; set; }

        public MenuTest()
        {
            Items = new List<MenuItem>
            {
                new("Menu1")
                {
                    IsActive = true,
                    Url = "https://www.blazor.zone"
                },
                new("Menu2")
                {
                    Items = new List<MenuItem>
                    {
                        new("Menu21")
                        {
                            IsDisabled = true
                        },
                        new("Menu22")
                        {
                            Url = "/menu22"
                        }
                    }
                }
            };
        }

        [Fact]
        public void Items_Ok()
        {
            // 未设置 Items
            var cut = Context.RenderComponent<Menu>();
            Assert.DoesNotContain("Menu1", cut.Markup);

            cut.SetParametersAndRender(pb =>
            {
                pb.Add(m => m.Items, Items);
            });
            Assert.Contains("Menu1", cut.Markup);
        }

        [Fact]
        public void DisableNavigation_Ok()
        {
            var cut = Context.RenderComponent<Menu>(pb =>
            {
                pb.Add(m => m.Items, Items);
                pb.Add(m => m.DisableNavigation, true);
            });
        }

        [Fact]
        public void IsVertical_Ok()
        {
            var cut = Context.RenderComponent<Menu>(pb =>
            {
                pb.Add(m => m.Items, Items);
                pb.Add(m => m.IsVertical, true);
            });

            Assert.Contains("is-vertical", cut.Markup);
        }

        [Fact]
        public void IsBottom_Ok()
        {
            var cut = Context.RenderComponent<Menu>(pb =>
            {
                pb.Add(m => m.Items, Items);
                pb.Add(m => m.IsBottom, true);
            });

            Assert.Contains("is-bottom", cut.Markup);
        }

        [Fact]
        public void IndentSize_Ok()
        {
            var cut = Context.RenderComponent<Menu>(pb =>
            {
                pb.Add(m => m.Items, Items);
                pb.Add(m => m.IndentSize, 32);
            });
            Assert.DoesNotContain("padding-left: 32px;", cut.Markup);

            cut.SetParametersAndRender(pb =>
            {
                pb.Add(m => m.IsVertical, true);
            });
            Assert.Contains("padding-left: 32px;", cut.Markup);
        }

        [Fact]
        public void IsCollapsed_Ok()
        {
            var cut = Context.RenderComponent<Menu>(pb =>
            {
                pb.Add(m => m.Items, Items);
                pb.Add(m => m.IsCollapsed, true);
            });
            Assert.DoesNotContain("is-collapsed", cut.Markup);

            cut.SetParametersAndRender(pb =>
            {
                pb.Add(m => m.IsVertical, true);
            });
            Assert.Contains("is-collapsed", cut.Markup);
        }

        [Fact]
        public void IsAccordion_Ok()
        {
            var cut = Context.RenderComponent<Menu>(pb =>
            {
                pb.Add(m => m.Items, Items);
                pb.Add(m => m.IsAccordion, true);
            });
            Assert.DoesNotContain("accordion", cut.Markup);

            cut.SetParametersAndRender(pb =>
            {
                pb.Add(m => m.IsVertical, true);
            });
            Assert.Contains("accordion", cut.Markup);
        }

        [Fact]
        public void IsExpandAll_Ok()
        {
            var cut = Context.RenderComponent<Menu>(pb =>
            {
                pb.Add(m => m.Items, Items);
                pb.Add(m => m.IsExpandAll, true);
            });
            Assert.DoesNotContain("expaned", cut.Markup);

            cut.SetParametersAndRender(pb =>
            {
                pb.Add(m => m.IsVertical, true);
            });
            Assert.Contains("expaned", cut.Markup);
        }

        [Fact]
        public void OnClick_Ok()
        {
            var clicked = false;
            var cut = Context.RenderComponent<Menu>(pb =>
            {
                pb.Add(m => m.Items, Items);
                pb.Add(m => m.OnClick, item =>
                {
                    clicked = true;
                    return Task.CompletedTask;
                });
            });

            // 查找第一个 li 节点
            var menuItems = cut.Find("li");
            menuItems.Click(new MouseEventArgs());
            Assert.True(clicked);

            // 设置禁止导航 
            // 顶栏模式
            cut.SetParametersAndRender(pb =>
            {
                pb.Add(m => m.DisableNavigation, true);
            });
            menuItems.Click(new MouseEventArgs());
            Assert.True(clicked);

            // 再次点击
            menuItems.Click(new MouseEventArgs());

            // 侧边栏模式
            cut.SetParametersAndRender(pb =>
            {
                pb.Add(m => m.IsVertical, true);
                pb.Add(m => m.IsCollapsed, true);
            });
            menuItems.Click(new MouseEventArgs());
            Assert.True(clicked);

            // 再次点击
            menuItems.Click(new MouseEventArgs());
        }

        [Fact]
        public void ActiveItem_Ok()
        {
            // 设置 后通过菜单激活 ActiveItem 不为空
            var nav = Context.Services.GetRequiredService<FakeNavigationManager>();
            nav.NavigateTo("/menu22");
            var cut = Context.RenderComponent<Menu>(pb =>
            {
                pb.Add(m => m.Items, Items);
            });
            var menu = cut.Find("li");
        }
    }
}
