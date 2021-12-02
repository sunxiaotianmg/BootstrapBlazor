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
using System.Text;
using System.Threading.Tasks;

namespace BootstrapBlazor.Components
{
    /// <summary>
    /// 
    /// </summary>
    public class BlazorLogger
#if NET5_0
        : BootstrapComponentBase, IBlazorLogger
#elif NET6_0_OR_GREATER
        : ErrorBoundaryBase, IBlazorLogger
#endif
    {
        /// <summary>
        /// 
        /// </summary>
        [Inject]
        [NotNull]
        public ILogger<BlazorLogger>? Logger { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Inject]
        [NotNull]
        public IConfiguration? Configuration { get; set; }

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
            builder.OpenComponent<CascadingValue<IBlazorLogger>>(0);
            builder.AddAttribute(1, nameof(CascadingValue<IBlazorLogger>.Value), this);
            builder.AddAttribute(2, nameof(CascadingValue<IBlazorLogger>.IsFixed), true);
#if NET5_0
            builder.AddAttribute(3, nameof(CascadingValue<IBlazorLogger>.ChildContent), ChildContent);
#elif NET6_0_OR_GREATER
            if (CurrentException != null)
            {
                builder.AddAttribute(3, nameof(CascadingValue<IBlazorLogger>.ChildContent), ErrorContent?.Invoke(CurrentException) ?? ChildContent);
            }
            else
            {
                builder.AddAttribute(4, nameof(CascadingValue<IBlazorLogger>.ChildContent), ChildContent);
            }
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
        /// OnErrorAsync 方法
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        protected override async Task OnErrorAsync(Exception exception)
        {
            Recover();

            await ErrorBoundaryLogger.LogErrorAsync(exception);
            await ToastService.Error("App", exception.Message);
        }

        /// <summary>
        /// OnInitialized 方法
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();

#if DEBUG
            if (ErrorContent == null)
            {
                ErrorContent = ex => builder =>
                {
                    var index = 0;
                    builder.OpenElement(index++, "div");
                    builder.AddContent(index++, ex.Message);
                    builder.CloseElement();
                    builder.OpenElement(index++, "div");
                    builder.AddContent(index++, ex.StackTrace);
                    builder.CloseElement();
                };
            }
#endif
        }
#endif

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logLevel"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public Task Log(LogLevel logLevel, string message, params object[] args)
        {
            Logger.Log(logLevel, message, args);
            return Task.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logLevel"></param>
        /// <param name="exception"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public Task Log(LogLevel logLevel, Exception exception, string message = "", params object[] args)
        {
            var logger = new StringBuilder();
            var infos = Configuration.GetEnvironmentInformation();
            foreach (string key in infos)
            {
                logger.AppendFormat("{0}: {1}", key, infos[key]);
                logger.AppendLine();
            }
            if (!string.IsNullOrEmpty(message))
            {
                logger.AppendFormat("{0}: {1}", "Message", message);
                logger.AppendLine();
            }

            logger.AppendFormat("{0}: {1}", "Exception", exception.Message);
            logger.AppendLine();

            logger.Append(new string('*', 45));
            Logger.Log(logLevel, exception, logger.ToString(), args);
            return Task.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logLevel"></param>
        /// <param name="exception"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        public Task Log(LogLevel logLevel, Exception exception, NameValueCollection? collection = null)
        {
            Logger.Log(logLevel, FormatException(exception, collection));
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
