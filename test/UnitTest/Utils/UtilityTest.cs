// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using BootstrapBlazor.Components;
using BootstrapBlazor.Shared;
using UnitTest.Core;
using Xunit;

namespace UnitTest.Utils
{
    public class UtilityTest : BootstrapBlazorTestBase
    {
        public Foo Model { get; set; }

        public UtilityTest()
        {
            Model = new Foo()
            {
                Name = "Test"
            };
        }

        [Fact]
        public void GetPropertyValue_Ok()
        {
            var v = Utility.GetPropertyValue(Model, nameof(Foo.Name));
            Assert.Equal("Test", v);
        }
    }
}
