// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using BootstrapBlazor.Components;
using Bunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnitTest.Core;
using Xunit;

namespace UnitTest.Components
{
    public class QRCodeTest : BootstrapBlazorTestBase
    {
        [Fact]
        public void ShowButtons()
        {
            var cut = Context.RenderComponent<QRCode>(builder => builder.Add(a => a.ShowButtons, true));

            Assert.Contains("button", cut.Markup);
        }

        [Fact]
        public void PlaceHolder()
        {
            var cut = Context.RenderComponent<QRCode>(builder =>
            {
                builder.Add(a => a.PlaceHolder, "Please input");
                builder.Add(a => a.ShowButtons, true);
            });

            Assert.Contains("Please input", cut.Markup);
        }

        [Fact]
        public void ClearButtonText()
        {
            var cut = Context.RenderComponent<QRCode>(builder =>
            {
                builder.Add(a => a.ShowButtons, true);
                builder.Add(a => a.ClearButtonText, "Clear");
            });

            Assert.Contains("Clear", cut.Markup);
        }

        [Fact]
        public void GenerateButtonText()
        {
            var cut = Context.RenderComponent<QRCode>(builder =>
            {
                builder.Add(a => a.ShowButtons, true);
                builder.Add(a => a.GenerateButtonText, "Generate");
            });

            Assert.Contains("Generate", cut.Markup);
        }

        [Fact]
        public void Content()
        {
            var cut = Context.RenderComponent<QRCode>(builder =>
            {
                builder.Add(a => a.Content, "https://www.blazor.zone");
            });
        }

        [Fact]
        public void OnGenerated()
        {
            var message = "";
            var cut = Context.RenderComponent<QRCode>(builder =>
            {
                builder.Add(a => a.OnGenerated, () => { message = "complete"; return Task.CompletedTask; });
                builder.Add(a => a.ShowButtons, true);
            });

            var input = cut.Find("input");
            input.TextContent = "qrcode";

            var button = cut.Find(".btn-success");
            button.Click();
        }

        [Fact]
        public void Clear()
        {
            var cut = Context.RenderComponent<QRCode>(builder =>
            {
                builder.Add(a => a.ShowButtons, true);
            });

            var input = cut.Find("input");
            input.TextContent = "qrcode";

            var button = cut.Find(".btn-success");
            button.Click();

            var clearbutton = cut.Find(".btn-secondary");
            clearbutton.Click();

            var cc = cut.Instance.GetType().GetProperty("MethodName", BindingFlags.Default | BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(cut.Instance, null);
            //Assert.DoesNotContain("clear",);
        }
    }
}
