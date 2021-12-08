﻿// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using BootstrapBlazor.Components;
using Bunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UnitTest.Core;
using Xunit;

namespace UnitTest.Components
{
    public class ColorPickerTest : BootstrapBlazorTestBase
    {
        [Fact]
        public void PlaceHolder_OK()
        {
            var cut = Context.RenderComponent<ColorPicker>(builder => builder.Add(a => a.AdditionalAttributes, new Dictionary<string, object>
            {
                ["placeholder"] = "Please pick"
            }));

            Assert.Contains("Please pick", cut.Markup);
        }

        [Fact]
        public void DisplayText_OK()
        {
            var cut = Context.RenderComponent<ColorPicker>(builder =>
            {
                builder.Add(a => a.ShowLabel, true);
                builder.Add(a => a.DisplayText, "Color picker");
            });

            Assert.Equal("Color picker", cut.Find("label").TextContent);
        }

        private string? PlaceHolder { get; set; } = "Please pick";

        [Fact]
        public void Display_OK()
        {
            var cut = Context.RenderComponent<ColorPicker>(builder =>
            {
                builder.Add(a => a.ValueExpression, () => PlaceHolder);
            });
        }
    }
}
