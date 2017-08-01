using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogTemplate.Models
{
    public class ExcerptGenerator
    {
        private BlogDataStore _dataStore;

        public ExcerptGenerator(BlogDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        public string CreateExcerpt(string body)
        {
            string excerpt;
            if (body.Length > 140)
            {
                excerpt = body.Substring(0, 139) + "...";
            }
            else
            {
                excerpt = body;
            }
            return excerpt;
        }
    }
}
