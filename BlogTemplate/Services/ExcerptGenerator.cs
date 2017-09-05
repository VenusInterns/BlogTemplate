using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogTemplate._1.Services
{
    public class ExcerptGenerator
    {
        private readonly int _maxLength;

        public ExcerptGenerator(int maxLength)
        {
            _maxLength = maxLength;
        }

        public string CreateExcerpt(string body)
        {
            string excerpt;
            if (body.Length > _maxLength)
            {
                excerpt = body.Substring(0, _maxLength) + "...";
            }
            else
            {
                excerpt = body;
            }
            return excerpt;
        }
    }
}
