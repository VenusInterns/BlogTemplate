using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogTemplate.Models
{
    public class SlugGenerator
    {
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
