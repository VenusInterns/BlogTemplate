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
            // Figure out how to use Regex
            Regex allowList = new Regex("[a-zA-Z0-9,.;:_'\\s-]*");
            foreach (char c in tempTitle)
            {
                if (!allowList.IsMatch(c.ToString()))
                {
                    tempTitle = tempTitle.Replace(c.ToString(), "");
                }
            }
            string slug = tempTitle.Replace(" ", "-");
            int count = 0;
            string tempSlug = slug;
            while (_dataStore.CheckSlugExists(tempSlug))
            {
                count++;
                tempSlug = $"{slug}-{count}";
            }
            return tempSlug;
        }
    }
}

