using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogTemplate.Models
{
    public class SlugGenerator
    {
        public string CreateSlug(string title)
        {
            BlogDataStore dataStore = new BlogDataStore();
            string slug = title.Replace(" ", "-");
            int count = 0;
            while (dataStore.CheckSlugExists(slug))
            {
                count++;
                slug = $"{slug}-{count}";
            }   
            return slug;
        }
    }
}
