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

        private static XmlNode GetCommentsRootNode(XmlDocument doc)
        {
            XmlNode commentsNode;
            XmlNodeList nodeList = doc.GetElementsByTagName("Comments");
            if (nodeList.Count == 0)
            {
                commentsNode = doc.CreateElement("Comments");
                doc.DocumentElement.AppendChild(commentsNode);
            }
            else
            {
                commentsNode = nodeList.Item(0);
            }
            return commentsNode;
        }

        public void AppendInfo(Comment comment, Post Post, XmlDocument doc)
        {
            XmlNode commentsNode = GetCommentsRootNode(doc);
            XmlNode commentNode = doc.CreateElement("Comment");

            commentNode.AppendChild(doc.CreateElement("AuthorName")).InnerText = comment.AuthorName;
            commentNode.AppendChild(doc.CreateElement("AuthorEmail")).InnerText = comment.AuthorEmail;
            commentNode.AppendChild(doc.CreateElement("PubDate")).InnerText = comment.PubDate.ToString();
            commentNode.AppendChild(doc.CreateElement("CommentBody")).InnerText = comment.Body;

            commentsNode.AppendChild(commentNode);
        }

        public XmlDocument LoadInfo(string postFilePath)
        {
            string fileContent = File.ReadAllText(postFilePath);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(fileContent);
            return doc;
        }

        public void SaveComment(Comment comment, Post Post)
        {
            string postFilePath = $"{StorageFolder}\\{Post.Slug}.xml";
            XmlDocument doc = LoadInfo(postFilePath);
            AppendInfo(comment, Post, doc);
            doc.Save(postFilePath);
        }

        public void SavePost(Post post)
        {
            string outputFilePath = $"{StorageFolder}\\{post.Slug}.xml";

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
            foreach (string tag in post.Tags)
                tagsNode.AppendChild(doc.CreateElement("Tag")).InnerText = tag;
            doc.Save(outputFilePath);
        }

        public Post GetPost(string slug)
        {
            string expectedFilePath = $"{StorageFolder}\\{slug}.xml";
            IFormatProvider culture = new System.Globalization.CultureInfo("en-US", true);

            if (File.Exists(expectedFilePath))
            {
                //string fileContent = File.ReadAllText(expectedFilePath);

                //XmlDocument doc = new XmlDocument();
                //doc.LoadXml(fileContent);
                XmlDocument doc = LoadInfo(expectedFilePath);

                Post post = new Post();

                post.Slug = doc.GetElementsByTagName("Slug").Item(0).InnerText;
                post.Title = doc.GetElementsByTagName("Title").Item(0).InnerText;
                post.Body = doc.GetElementsByTagName("Body").Item(0).InnerText;
                post.PubDate = DateTime.Parse((doc.GetElementsByTagName("PubDate").Item(0).InnerText), culture, System.Globalization.DateTimeStyles.AssumeLocal);
                post.LastModified = DateTime.Parse((doc.GetElementsByTagName("LastModified").Item(0).InnerText), culture, System.Globalization.DateTimeStyles.AssumeLocal);
                post.IsPublic = Convert.ToBoolean(doc.GetElementsByTagName("IsPublic").Item(0).InnerText);
                post.Excerpt = doc.GetElementsByTagName("Excerpt").Item(0).InnerText;

                //load comments into post's list of comments

                post.Comments = GetAllComments(post.Slug);
                return post;
            }

            return null;
        }

        public List<Comment> GetAllComments(string slug)
        {
            IFormatProvider culture = new System.Globalization.CultureInfo("en-US", true);

            string filePath = $"{StorageFolder}//{slug}.xml";
            XDocument xDoc = XDocument.Load(filePath);
            IEnumerable<XElement> comments = xDoc.Root
                                                .Element("Comments")
                                                    .Elements("Comment");

            List<Comment> allComments = new List<Comment>();
            foreach (XElement comment in comments)
            {
                Comment newComment = new Comment();
                newComment.AuthorName = comment.Element("AuthorName").Value;
                newComment.Body = comment.Element("CommentBody").Value;
                newComment.AuthorEmail = comment.Element("AuthorEmail").Value;
                newComment.PubDate = DateTime.Parse((comment.Element("PubDate").Value), culture, System.Globalization.DateTimeStyles.AssumeLocal); 
                allComments.Add(newComment);
            }
            return allComments;
        }

        //public List<Post> GetAllPosts()
        //{
        //    IFormatProvider culture = new System.Globalization.CultureInfo("en-US", true);

        //    string filePath = $"{StorageFolder}";
        //    XDocument xDoc = XDocument.Load(filePath);
        //    IEnumerable<XElement> posts = (IEnumerable<XElement>)xDoc.Root.Element("Posts");

        //    List<Post> TestAllPosts = new List<Post>();
        //    foreach(XElement post in posts)
        //    {
        //        Post newPost = new Post();
        //        newPost.Title = post.Element("Title").Value;
        //        newPost.Body = post.Element("Body").Value;
        //        newPost.PubDate = DateTime.Parse((post.Element("PubDate").Value), culture, System.Globalization.DateTimeStyles.AssumeLocal);
        //        newPost.LastModified = DateTime.Parse((post.Element("LastModified").Value), culture, System.Globalization.DateTimeStyles.AssumeLocal);
        //        newPost.Slug = post.Element("Slug").Value;
        //        newPost.IsPublic = Convert.ToBoolean(post.Element("IsPublic").Value);
        //        newPost.Excerpt = post.Element("Excerpt").Value;
        //        TestAllPosts.Add(newPost);
        //    }
        //    return TestAllPosts;
        //}


        public List<Post> GetAllPosts()
        {
            string filePath = $"{StorageFolder}";
            List<FileInfo> files = new DirectoryInfo(filePath).GetFiles().OrderBy(f => f.LastWriteTime).ToList();
            List<Post> allPosts = new List<Post>();
            IFormatProvider culture = new System.Globalization.CultureInfo("en-US", true);

            for (int i = 0; i < files.Count; i++)
            {
                var file = files[files.Count - i - 1];
                string fileContent = File.ReadAllText($"{StorageFolder}\\{file.Name}");
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(fileContent);
                Post post = new Post();

                post.Title = doc.GetElementsByTagName("Title").Item(0).InnerText;
                post.Body = doc.GetElementsByTagName("Body").Item(0).InnerText;
                post.PubDate = DateTime.Parse((doc.GetElementsByTagName("PubDate").Item(0).InnerText), culture, System.Globalization.DateTimeStyles.AssumeLocal);
                post.LastModified = DateTime.Parse((doc.GetElementsByTagName("LastModified").Item(0).InnerText), culture, System.Globalization.DateTimeStyles.AssumeLocal);
                post.Slug = doc.GetElementsByTagName("Slug").Item(0).InnerText;
                post.IsPublic = Convert.ToBoolean(doc.GetElementsByTagName("IsPublic").Item(0).InnerText);
                post.Excerpt = doc.GetElementsByTagName("Excerpt").Item(0).InnerText;

                allPosts.Add(post);
            }
            return allPosts;
        }

        public void InitStorageFolder()
        {
            Directory.CreateDirectory(StorageFolder);
        }

        public bool CheckSlugExists(string slug)
        {
            return File.Exists($"{StorageFolder}\\{slug}");
        }
    }
}
