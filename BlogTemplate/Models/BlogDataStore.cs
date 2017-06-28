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

            if(File.Exists(outputFilePath))
            {
                throw new InvalidOperationException("A post with this slug already exists");
            }

            XmlDocument doc = new XmlDocument();

            XmlElement rootNode = doc.CreateElement("Post");
            doc.AppendChild(rootNode);
            rootNode.AppendChild(doc.CreateElement("Slug")).InnerText = post.Slug;
            rootNode.AppendChild(doc.CreateElement("Title")).InnerText = post.Title;
            rootNode.AppendChild(doc.CreateElement("Body")).InnerText = post.Body;

            doc.Save(outputFilePath);
        }

        public Post GetPost(string slug)
        {
            string expectedFilePath = $"{StorageFolder}\\{slug}.xml";

            if(File.Exists(expectedFilePath))
            {
                string fileContent = File.ReadAllText(expectedFilePath);

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(fileContent);

                Post post = new Post();

                post.Slug = doc.GetElementsByTagName("Slug").Item(0).InnerText;
                post.Title = doc.GetElementsByTagName("Title").Item(0).InnerText;
                post.Body = doc.GetElementsByTagName("Body").Item(0).InnerText;

                return post;
            }

            return null;
        }
    }
}
