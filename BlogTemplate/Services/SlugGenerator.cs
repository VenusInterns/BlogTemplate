using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogTemplate.Models;

namespace BlogTemplate.Services
{
    public class SlugGenerator
    {
        private BlogDataStore _dataStore;

        public SlugGenerator(BlogDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        public string CreateSlug(string title)
        {
            string tempTitle = title;
            char[] invalidChars = Path.GetInvalidFileNameChars();
            foreach (char c in invalidChars)
            {
                tempTitle = tempTitle.Replace(c.ToString(), "");
            }
            string slug = tempTitle.Replace(" ", "-");
            return slug;
        }
    }
}

