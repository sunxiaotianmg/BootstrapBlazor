// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
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

#if NET5_0
        /// <summary>
        /// 获得/设置 自组件
        /// </summary>
        [Parameter]
        public RenderFragment? ChildContent { get; set; }

#elif NET6_0_OR_GREATER
        [Inject]
        [NotNull]
        private IErrorBoundaryLogger? ErrorBoundaryLogger { get; set; }

        [Inject]
        [NotNull]
        private ToastService? ToastService { get; set; }
#endif

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

            if (OperatingSystem.IsBrowser())
            {
                builder.AddContent(index++, RenderChildContent());
            }
            else
            {
                builder.OpenElement(index++, "app");
                builder.AddContent(index++, RenderChildContent());
                builder.CloseElement();
            }

            builder.OpenComponent<Dialog>(index++);
            builder.CloseComponent();

            builder.OpenComponent<Download>(index++);
            builder.CloseComponent();

            builder.OpenComponent<FullScreen>(index++);
            builder.CloseComponent();

            builder.OpenComponent<Message>(index++);
            builder.AddComponentReferenceCapture(index++, com => MessageContainer = (Message)com);
            builder.CloseComponent();

            builder.OpenComponent<PopoverConfirm>(index++);
            builder.CloseComponent();

            builder.OpenComponent<Print>(index++);
            builder.CloseComponent();

            builder.OpenComponent<SweetAlert>(index++);
            builder.CloseComponent();

            builder.OpenComponent<Title>(index++);
            builder.CloseComponent();

            builder.OpenComponent<Toast>(index++);
            builder.AddComponentReferenceCapture(index++, com => ToastContainer = (Toast)com);
            builder.CloseComponent();


            RenderFragment RenderChildContent() => builder =>
            {
                builder.OpenComponent<CascadingValue<BootstrapBlazorRoot>>(0);
                builder.AddAttribute(1, nameof(CascadingValue<BootstrapBlazorRoot>.IsFixed), true);
                builder.AddAttribute(2, nameof(CascadingValue<BootstrapBlazorRoot>.Value), this);
#if NET5_0
                builder.AddAttribute(3, nameof(CascadingValue<BootstrapBlazorRoot>.ChildContent), ChildContent);
#elif NET6_0_OR_GREATER
                if (CurrentException != null)
                {
                    builder.AddAttribute(3, nameof(CascadingValue<BootstrapBlazorRoot>.ChildContent), ErrorContent?.Invoke(CurrentException) ?? ChildContent);
                }
                else
                {
                    builder.AddAttribute(4, nameof(CascadingValue<BootstrapBlazorRoot>.ChildContent), ChildContent);
                }
#endif
                builder.CloseComponent();
            };
        }

#if NET6_0_OR_GREATER
        /// <summary>
        /// OnParametersSet 方法
        /// </summary>
        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            Recover();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        protected override async Task OnErrorAsync(Exception exception)
        {
            await ErrorBoundaryLogger.LogErrorAsync(exception);
            await ToastService.Error("App", exception.Message);
        }
#endif
    }
}
