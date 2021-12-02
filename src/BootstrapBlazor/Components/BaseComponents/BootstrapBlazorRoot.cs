// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace BootstrapBlazor.Components
{
    /// <summary>
    /// BootstrapBlazorRoot 组件
    /// </summary>
    public class BootstrapBlazorRoot
#if NET5_0
        : ComponentBase
#elif NET6_0_OR_GREATER
        : ErrorBoundaryBase
#endif
    {
        [Inject]
        [NotNull]
        private ICacheManager? Cache { get; set; }

        /// <summary>
        /// 获得/设置 自组件
        /// </summary>
        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        /// <summary>
        /// 获得 Message 组件实例
        /// </summary>
        [NotNull]
        public Message? MessageContainer { get; private set; }

        /// <summary>
        /// 获得 Toast 组件实例
        /// </summary>
        [NotNull]
        public Toast? ToastContainer { get; private set; }

        /// <summary>
        /// SetParametersAsync 方法
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public override async Task SetParametersAsync(ParameterView parameters)
        {
            Cache.SetStartTime();

            await base.SetParametersAsync(parameters);
        }

        /// <summary>
        /// BuildRenderTree 方法
        /// </summary>
        /// <param name="builder"></param>
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            var index = 0;


            RenderFragment RenderChildContent() => builder =>
            {
                builder.OpenComponent<CascadingValue<BootstrapBlazorRoot>>(0);
                builder.AddAttribute(1, nameof(CascadingValue<BootstrapBlazorRoot>.IsFixed), true);
                builder.AddAttribute(2, nameof(CascadingValue<BootstrapBlazorRoot>.Value), this);
                builder.AddContent(3, ChildContent);
                builder.CloseComponent();
            };
        }

#if NET6_0_OR_GREATER
        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        protected override Task OnErrorAsync(Exception exception)
        {
            return Task.CompletedTask;
        }
#endif
    }
}
