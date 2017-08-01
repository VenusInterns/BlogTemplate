using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BlogTemplate.Models
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
            char[] inv = Path.GetInvalidPathChars();
            char[] real = inv;
            foreach (char c in Path.GetInvalidPathChars())
            {
                if (tempTitle.Contains(c))
                {
                    int removeIdx = tempTitle.IndexOf(c);
                    tempTitle = tempTitle.Remove(removeIdx);
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
