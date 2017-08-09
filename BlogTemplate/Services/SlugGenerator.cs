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
            Encoding utf8 = new UTF8Encoding(true);
            string tempTitle = title;
            char[] invalidChars = Path.GetInvalidPathChars();
            foreach (char c in invalidChars)
            {
                string s = c.ToString();
                string decodedS = utf8.GetString(c);
                if (tempTitle.Contains(s))
                {
                    int removeIdx = tempTitle.IndexOf(s);
                    tempTitle = tempTitle.Remove(removeIdx);
                }
            }
            string slug = title.Replace(" ", "-");
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
