using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogTemplate._1.Services
{
    public class ExcerptGenerator
    {
        public string CreateExcerpt(string body, int maxLength)
        {
            string excerpt;
            if (body.Length > maxLength)
            {
                excerpt = body.Substring(0, maxLength) + "...";
            }
            else
            {
                excerpt = body;
            }
            return excerpt;
        }
    }
}
