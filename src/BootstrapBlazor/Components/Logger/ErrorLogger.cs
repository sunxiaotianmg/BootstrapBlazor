// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace BootstrapBlazor.Components
{
    /// <summary>
    /// 
    /// </summary>
    public class ErrorLogger
#if NET5_0
        : BootstrapComponentBase, IErrorLogger
#elif NET6_0_OR_GREATER
        : ErrorBoundaryBase, IErrorLogger
#endif
    {
        /// <summary>
        /// 
        /// </summary>
        [Inject]
        [NotNull]
        private ILogger<ErrorLogger>? Logger { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Inject]
        [NotNull]
        private IConfiguration? Configuration { get; set; }

#if NET5_0
        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public RenderFragment? ChildContent { get; set; }
#endif

        /// <summary>
        /// BuildRenderTree 方法
        /// </summary>
        /// <param name="builder"></param>
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenComponent<CascadingValue<IErrorLogger>>(0);
            builder.AddAttribute(1, nameof(CascadingValue<IErrorLogger>.Value), this);
            builder.AddAttribute(2, nameof(CascadingValue<IErrorLogger>.IsFixed), true);
#if NET5_0
            builder.AddAttribute(3, nameof(CascadingValue<IErrorLogger>.ChildContent), ChildContent);
#elif NET6_0_OR_GREATER
#if DEBUG
            if (CurrentException != null)
            {
                builder.AddAttribute(3, nameof(CascadingValue<IErrorLogger>.ChildContent), ErrorContent?.Invoke(CurrentException) ?? ChildContent);
            }
            else
            {
                builder.AddAttribute(4, nameof(CascadingValue<IErrorLogger>.ChildContent), ChildContent);
            }
#else
            builder.AddAttribute(4, nameof(CascadingValue<IErrorLogger>.ChildContent), ChildContent);
#endif
#endif
            builder.CloseComponent();
        }

#if NET6_0_OR_GREATER
        [Inject]
        [NotNull]
        private IErrorBoundaryLogger? ErrorBoundaryLogger { get; set; }

        [Inject]
        [NotNull]
        private ToastService? ToastService { get; set; }

        /// <summary>
        /// 获得/设置 是否显示弹窗 默认 true 显示
        /// </summary>
        [Parameter]
        public bool ShowToast { get; set; } = true;

        /// <summary>
        /// OnErrorAsync 方法
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        protected override async Task OnErrorAsync(Exception exception)
        {
            if (ShowToast)
            {
                await ToastService.Error("Application Error", exception.Message);
            }

            await ErrorBoundaryLogger.LogErrorAsync(exception);
            await Log(exception);
        }

        private bool ShowErrorDetails { get; set; }

        /// <summary>
        /// OnInitialized 方法
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();

            ShowErrorDetails = Configuration.GetValue<bool>("DetailedErrors", false);

            if (ErrorContent == null && ShowErrorDetails)
            {
                ErrorContent = ex => builder =>
                {
                    var index = 0;
                    builder.OpenElement(index++, "div");
                    builder.AddAttribute(index++, "class", "error-stack");
                    builder.AddContent(index++, ex.FormatMarkupString(Configuration.GetEnvironmentInformation()));
                    builder.CloseElement();
                };
            }
        }
#endif

#if NET6_0_OR_GREATER
        /// <summary>
        /// OnParametersSet 方法
        /// </summary>
        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            Recover();
        }
#endif

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        public Task Log(Exception exception)
        {
            Logger.LogError(FormatException(exception));
            return Task.CompletedTask;
        }

        /// <summary>
        /// 格式化异常信息
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        public string FormatException(Exception exception, NameValueCollection? collection = null)
        {
            collection ??= new NameValueCollection();
            collection.Add(Configuration.GetEnvironmentInformation());
            return exception.Format(collection);
        }
    }
}
