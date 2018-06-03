using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpiralWorks.Web.Helpers
{
    [HtmlTargetElement("div")]
    public class VisibilityTagHelper : TagHelper
    {
        public bool IsVisible { get; set; } = true;
        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (!IsVisible)
                output.SuppressOutput();

            return base.ProcessAsync(context, output);
        }
    }
}
