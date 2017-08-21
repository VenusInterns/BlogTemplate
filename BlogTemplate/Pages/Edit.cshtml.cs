using System;
using BlogTemplate._1.Models;
using BlogTemplate._1.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace BlogTemplate._1.Pages
{
    [Authorize]
    public class EditModel : PageModel
    {

        private readonly BlogDataStore _dataStore;

        private readonly SlugGenerator _slugGenerator;
        private readonly ExcerptGenerator _excerptGenerator;


        public EditModel(BlogDataStore dataStore, SlugGenerator slugGenerator, ExcerptGenerator excerptGenerator)
        {
            _dataStore = dataStore;
            _slugGenerator = slugGenerator;
            _excerptGenerator = excerptGenerator;
        }

        [BindProperty]
        public Post newPost { get; set; }

        public Post oldPost { get; set; }

        public void OnGet([FromRoute] int id)
        {
            newPost = oldPost = _dataStore.GetPost(id);

            if (oldPost == null)
            {
                RedirectToPage("/Index");
            }
        }


        [ValidateAntiForgeryToken]
        public IActionResult OnPostPublish([FromRoute] int id)
        {
            oldPost = _dataStore.GetPost(id);
            newPost.IsPublic = true;
            UpdatePost(id);
            return Redirect($"/Post/{id}/{newPost.Slug}");
        }

        [ValidateAntiForgeryToken]
        public IActionResult OnPostSaveDraft([FromRoute] int id)
        {
            oldPost = _dataStore.GetPost(id);
            newPost.IsPublic = false;
            UpdatePost(id);
            return Redirect("/Drafts");
        }

        private void UpdatePost(int id)
        {
            newPost.Id = id;
            oldPost = _dataStore.GetPost(id);

            if (oldPost.PubDate.Equals(default(DateTimeOffset)))
            {
                if (newPost.IsPublic == true)
                {
                    newPost.PubDate = DateTimeOffset.Now;
                }
            }
            else
            {
                newPost.PubDate = oldPost.PubDate;
            }

            if (newPost.Excerpt == null)
            {
                newPost.Excerpt = _excerptGenerator.CreateExcerpt(newPost.Body, newPost.ExcerptMaxLength);
            }

            if (Request.Form["updateslug"] == "true")
            {
                newPost.Slug = _slugGenerator.CreateSlug(newPost.Title);
            }
            else
            {
                newPost.Slug = oldPost.Slug;
            }
            newPost.Comments = oldPost.Comments;

            _dataStore.UpdatePost(newPost, oldPost);
        }
    }
}
