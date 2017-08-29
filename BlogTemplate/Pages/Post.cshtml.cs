using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BlogTemplate.Models;
using Microsoft.AspNetCore.Mvc;
using System.Xml;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authorization;
using Markdig;
using Microsoft.AspNetCore.Html;

namespace BlogTemplate.Pages
{

    public class PostModel : PageModel
    {
        private readonly BlogDataStore _dataStore;

        private static MarkdownPipeline pipeline = new MarkdownPipelineBuilder()
               .UseDiagrams()
               .UseAdvancedExtensions()
               .UseYamlFrontMatter()
               .DisableHtml()
               .Build();

        public PostModel(BlogDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        [BindProperty]
        public Comment Comment { get; set; }

        public Post Post { get; set; }

        public HtmlString HtmlBody()
        {

            var html = Markdown.ToHtml(Post.Body, pipeline);
            return new HtmlString(html);
        }

        public void OnGet([FromRoute] int id)
        {
            Post = _dataStore.GetPost(id);

            if (Post == null)
            {
                RedirectToPage("/Index");
            }
        }

        [ValidateAntiForgeryToken]
        public IActionResult OnPostPublishComment([FromRoute] int id)
        {
            Post = _dataStore.GetPost(id);

            if (Post == null)
            {
                RedirectToPage("/Index");
            }
            else if (ModelState.IsValid)
            {
                Comment.IsPublic = true;
                Comment.UniqueId = Guid.NewGuid();
                Post.Comments.Add(Comment);
                _dataStore.SavePost(Post);
            }
            return Page();
        }
    }
}
