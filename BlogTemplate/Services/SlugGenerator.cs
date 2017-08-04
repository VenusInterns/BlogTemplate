using System;
using System.Collections.Generic;
using System.Linq;
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
