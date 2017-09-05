using System.Text.RegularExpressions;
using BlogTemplate._1.Models;

namespace BlogTemplate._1.Services
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
            tempTitle = tempTitle.Replace(" ", "-");
            Regex allowList = new Regex("([^A-Za-z0-9-])");
            string slug = allowList.Replace(tempTitle, "");
            return slug;
        }
    }
}

