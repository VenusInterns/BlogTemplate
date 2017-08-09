using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogTemplate.Services
{
    public class ExcerptGenerator
    {
        public string CreateExcerpt(string body, int maxLength)
        {
            string excerpt;
            if (body.Length > maxLength)
            {
                excerpt = body.Substring(0, maxLength-1) + "...";
            }
            else
            {
                excerpt = body;
            }
            return excerpt;
        }
    }
}

