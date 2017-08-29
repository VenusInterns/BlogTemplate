using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogTemplate._1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlogTemplate._1.Pages
{
    [Authorize]
    public class ManageCommentModel : PageModel
    {
        private readonly BlogDataStore _dataStore;

        public ManageCommentModel(BlogDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        [ValidateAntiForgeryToken]
        public IActionResult OnPostDeleteComment(Guid commentId, int id)
        {
            Post post = _dataStore.GetPost(id);

            Comment foundComment = _dataStore.FindComment(commentId, post);
            foundComment.IsPublic = false;

            _dataStore.SavePost(post);
            return Redirect($"/Post/{id}/{post.Slug}");
        }

        [ValidateAntiForgeryToken]
        public IActionResult OnPostUndeleteComment(Guid commentId, int id)
        {
            Post post = _dataStore.GetPost(id);

            Comment foundComment = _dataStore.FindComment(commentId, post);
            foundComment.IsPublic = true;

            _dataStore.SavePost(post);
            return Redirect($"/Post/{id}/{post.Slug}");
        }
    }
}
