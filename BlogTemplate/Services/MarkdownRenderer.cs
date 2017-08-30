using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Markdig;
using Microsoft.AspNetCore.Html;

namespace BlogTemplate._1.Services
{
    public class MarkdownRenderer
    {
        private static MarkdownPipeline pipeline = new MarkdownPipelineBuilder()
               .UseDiagrams()
               .UseAdvancedExtensions()
               .UseYamlFrontMatter()
               .DisableHtml()
               .Build();

        public HtmlString RenderMarkdownToHtml(string bodyText)
        {
            var html = Markdown.ToHtml(bodyText, pipeline);
            return new HtmlString(html);
        }
    }
}
