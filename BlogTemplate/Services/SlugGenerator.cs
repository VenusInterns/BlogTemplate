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

        public string CreateSlug(Post post)
        {
            BlogDataStore dataStore = new BlogDataStore();
            string slug = post.Title.Replace(" ", "-");
            int count = 0;
            string tempSlug = slug = $"{post.PubDate.ToFileTimeUtc()}_{slug}";
            while (dataStore.CheckSlugExists(tempSlug))
            {
                count++;
                tempSlug = $"{slug}-{count}";
            }
            return tempSlug;
        }
    }
}
