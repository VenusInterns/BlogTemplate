using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BlogTemplate._1.Models;

namespace BlogTemplate._1.Services
{
    public class SlugGenerator
    {
        private BlogDataStore _dataStore;
        private static Regex AllowList = new Regex("([^A-Za-z0-9-])", RegexOptions.None, TimeSpan.FromSeconds(1));

        public SlugGenerator(BlogDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        public string CreateSlug(string title)
        {
            string tempTitle = title;
            tempTitle = tempTitle.Replace(" ", "-");
        
            string slug = AllowList.Replace(tempTitle, "");
            return slug;
        }
    }
}

