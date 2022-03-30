using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace NetMind.TagHelpers
{
    [HtmlTargetElement("input")]
    public class InputTagHelper: TagHelper 
    {
        private const string ForAttributeName = "asp-for";

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }
        protected IHtmlGenerator Generator { get; }

        [HtmlAttributeName("ca-labeled")]
        public bool Labeled { get; set; }

        [HtmlAttributeName(ForAttributeName)]
        public ModelExpression For { get; set; }
        public InputTagHelper(IHtmlGenerator generator)
        {
            Generator = generator;
        }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (Labeled)
            {
                var tagBuilder = Generator.GenerateLabel(
                   ViewContext,
                   For.ModelExplorer,
                   For.Name,
                   labelText: null,
                   htmlAttributes: null);

                if (tagBuilder != null)
                {
                    tagBuilder.AddCssClass("col-sm-1 col-form-label col-form-label-sm");
                    output.PreElement.SetHtmlContent(tagBuilder);
                }
            }
        }
    }
}
