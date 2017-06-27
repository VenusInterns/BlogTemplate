using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace BlogTemplate.Models
{
    public class BlogDataStore
    {
        const string StorageFolder = "BlogFiles";
        
        public void SavePost(Post post)
        {
            Directory.CreateDirectory(StorageFolder);

            string outputFilePath = $"{StorageFolder}\\{post.Slug}.xml";

            XmlDocument doc = new XmlDocument();

            XmlElement rootNode = doc.CreateElement("Post");
            doc.AppendChild(rootNode);
            rootNode.AppendChild(doc.CreateElement("Slug")).InnerText = post.Slug;
            rootNode.AppendChild(doc.CreateElement("Title")).InnerText = post.Title;
            rootNode.AppendChild(doc.CreateElement("Body")).InnerText = post.Body;

            doc.Save(outputFilePath);
        }
    }
}
