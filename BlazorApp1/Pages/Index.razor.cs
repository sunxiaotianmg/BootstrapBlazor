// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using BootstrapBlazor.Components;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Xml.Linq;

namespace BlazorApp1.Pages
{
    public partial class Index
    {
        protected override void OnInitialized()
        {
            base.OnInitialized();
            CultureInfo.CurrentUICulture = new CultureInfo("zh-CN");
            var val = Utility.GetDisplayName(typeof(Dummy), nameof(Dummy.Name));
        }
    }

    class Dummy
    {
        [Display(Name = "Name1")]
        public string? Name { get; set; }

        [Display(Name = "Address1")]
        public string? Address { get; set; }
    }
}
