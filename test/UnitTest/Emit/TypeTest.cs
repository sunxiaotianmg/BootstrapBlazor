// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using BootstrapBlazor.Components;
using System;
using System.Linq;
using Xunit;

namespace UnitTest.Emit
{
    public class TypeTest
    {
        [Fact]
        public void CreateType_Ok()
        {
            var cols = new MockTableColumn[]
            {
                new("Id", typeof(int)),
                new("Name", typeof(string))
            };

            // 创建动态类型基类是 DynamicObject
            var instanceType = EmitHelper.CreateTypeByName("Test", cols, typeof(DynamicObject));
            Assert.NotNull(instanceType);

            if (instanceType != null)
            {
                Assert.Equal(typeof(DynamicObject), instanceType.BaseType);

                // 创建动态类型实例
                var instance = Activator.CreateInstance(instanceType);
                Assert.NotNull(instance);

                if (instance != null)
                {
                    var properties = instance.GetType().GetProperties().Select(p => p.Name);
                    Assert.Contains(nameof(DynamicObject.DynamicObjectPrimaryKey), properties);

                    // Utility
                    var v = Utility.GetPropertyValue(instance, "Name");
                    Assert.Null(v);
                }
            }
        }
    }
}
