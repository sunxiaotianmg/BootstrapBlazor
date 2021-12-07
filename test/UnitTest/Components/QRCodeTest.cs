// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using BootstrapBlazor.Components;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
