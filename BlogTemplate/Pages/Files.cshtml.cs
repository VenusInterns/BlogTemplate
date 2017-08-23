using System;
using BlogTemplate._1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Xml;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authorization;
using Markdig;
using Microsoft.AspNetCore.Html;

namespace BlogTemplate._1.Pages
{
    public class FilesModel : PageModel
    {
        private readonly BlogDataStore _dataStore;

        public FilesModel(BlogDataStore dataStore)
        {
            _dataStore = dataStore;
        }
        public void OnGet([FromRoute] string fileName)
        {
            _dataStore.GetFile(fileName);
        }
    }
}
