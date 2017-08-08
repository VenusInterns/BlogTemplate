using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogTemplate.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlogTemplate.Pages
{
    [Authorize]
    public class ManageCommentModel : PageModel
    {
        private BlogDataStore _dataStore;

        public ManageCommentModel(BlogDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        [ValidateAntiForgeryToken]
        public IActionResult OnPostDeleteComment(Guid commentId)
        {
            long id = Convert.ToInt64(RouteData.Values["id"]);
            string slug = RouteData.Values["slug"].ToString();
            Post post = _dataStore.GetPost(id);

            Comment foundComment = _dataStore.FindComment(commentId, post);
            foundComment.IsPublic = false;

            _dataStore.SavePost(post);
            return Redirect($"/Post/{id}/{slug}");
        }

        [ValidateAntiForgeryToken]
        public IActionResult OnPostUndeleteComment(Guid commentId)
        {
            long id = Convert.ToInt64(RouteData.Values["id"]);
            string slug = RouteData.Values["slug"].ToString();
            Post post = _dataStore.GetPost(id);

            Comment foundComment = _dataStore.FindComment(commentId, post);
            foundComment.IsPublic = true;

            _dataStore.SavePost(post);
            return Redirect($"/Post/{id}/{slug}");
        }
    }
}
