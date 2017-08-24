using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BlogTemplate._1.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BlogTemplate.Controllers
{
    public class FilesController : Controller
    {
        private readonly BlogDataStore _dataStore;
        protected FilesController(BlogDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        // GET: /<controller>/
        [Route("BlogFiles/Uploads/{filename}")]
        public IActionResult GetFile(string filename)
        {
            byte[] data = _dataStore.GetFileData(filename);
            return File(data, "PNG");
        }
    }
}
