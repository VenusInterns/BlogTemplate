using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            Regex allowList = new Regex("([^A-Za-z0-9-])");
            tempTitle = allowList.Replace(tempTitle, "");
            string slug = tempTitle.Replace(" ", "-");
            return slug;
        }
    }
}

