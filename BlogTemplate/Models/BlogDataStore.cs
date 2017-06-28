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
        
        //public void SaveComment(Comment comment, Post post)
        //{
        //    //Find XmlDocument corresponding to the post
        //    string postFilePath = $"{StorageFolder}\\{post.Slug}.xml";
        //    string fileContent = File.ReadAllText(postFilePath);

        //    XmlDocument doc = new XmlDocument();
        //    doc.LoadXml(fileContent);

        //    //If this is the first comment to the post, then we need to create an XmlElement in the XmlDocument corresponding to the post
        //    XmlElement rootNode;
        //    if (post.Comments.Count() == 0)
        //    {
        //        rootNode = doc.CreateElement("Comments");
        //    }
        //    else
        //    {
        //        XmlNodeList nodeList = doc.GetElementsByTagName("Comments");
        //        rootNode = (XmlElement)nodeList.Item(0);
        //    }

        //    //Append information
        //    rootNode.AppendChild(doc.CreateElement("AuthorName")).InnerText = comment.AuthorName;
        //    rootNode.AppendChild(doc.CreateElement("AuthorEmail")).InnerText = comment.AuthorEmail;
        //    rootNode.AppendChild(doc.CreateElement("PubDate")).InnerText = comment.PubDate.ToString();
        //    rootNode.AppendChild(doc.CreateElement("CommentBody")).InnerText = comment.Body;

        //    doc.Save(postFilePath);
        //}


        public void SavePost(Post post)
        {
            Directory.CreateDirectory(StorageFolder);

            //format attributes of the post
            post.Tags = post.Tags.ToList();
            post.Slug = post.Title.Replace(" ", "-");

            string outputFilePath = $"{StorageFolder}\\{post.Slug}.xml";
            int count = 0;
            while(File.Exists(outputFilePath))
            {
                count++;
                outputFilePath = $"{StorageFolder}\\{post.Slug}-{count}.xml";
                //throw new InvalidOperationException("A post with this slug already exists");
            }
            post.Slug = $"{post.Slug}-{count}";

            XmlDocument doc = new XmlDocument();

            XmlElement rootNode = doc.CreateElement("Post");
            doc.AppendChild(rootNode);
            rootNode.AppendChild(doc.CreateElement("Slug")).InnerText = post.Slug;
            rootNode.AppendChild(doc.CreateElement("Title")).InnerText = post.Title;
            rootNode.AppendChild(doc.CreateElement("Body")).InnerText = post.Body;
            rootNode.AppendChild(doc.CreateElement("PubDate")).InnerText = post.PubDate.ToString();
            rootNode.AppendChild(doc.CreateElement("LastModified")).InnerText = post.LastModified.ToString();
            rootNode.AppendChild(doc.CreateElement("IsPublic")).InnerText = post.IsPublic.ToString();
            rootNode.AppendChild(doc.CreateElement("Excerpt")).InnerText = post.Excerpt;
            XmlElement tagsNode = doc.CreateElement("Tags");
            rootNode.AppendChild(tagsNode);
            foreach(string tag in post.Tags)
                tagsNode.AppendChild(doc.CreateElement("Tag")).InnerText = tag;
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
