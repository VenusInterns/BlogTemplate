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

        public BlogDataStore()
        {
            InitStorageFolder();
        }

        public void InitStorageFolder()
        {
            Directory.CreateDirectory(StorageFolder);
        }
        private static XElement GetCommentsRootNode(XDocument doc)
        {
            XElement commentsNode;
            //XElement nodeList = doc.Root.Element("Comments");
            if (doc.Root.Elements("Comments").Any())
            {
                commentsNode = doc.Root.Element("Comments");                
            }
            else
            {
                commentsNode = new XElement("Comments");
                doc.Root.Add(commentsNode);
            }
            return commentsNode;
        }

        public void AppendInfo(Comment comment, Post Post, XDocument doc)
        {
            XElement commentsNode = GetCommentsRootNode(doc);
            XElement commentNode = new XElement("Comment");

            commentNode.Add(new XElement("AuthorName", comment.AuthorName));
            commentNode.Add(new XElement("AuthorEmail", comment.AuthorEmail));
            commentNode.Add(new XElement("PubDate", comment.PubDate.ToString()));
            commentNode.Add(new XElement("CommentBody", comment.Body));
            
            commentsNode.Add(commentNode);
        }

        public XDocument LoadInfo(string postFilePath)
        {
            //string fileContent = File.ReadAllText(postFilePath);
            XDocument doc = XDocument.Load(postFilePath);
            //doc.LoadXml(fileContent);
            return doc;
        }



        public void SaveComment(Comment comment, Post Post)
        {
            string postFilePath = $"{StorageFolder}\\{Post.Slug}.xml";
            XDocument doc = LoadInfo(postFilePath);
            AppendInfo(comment, Post, doc);
            doc.Save(postFilePath);
        }




        public List<Comment> appendInfor(IEnumerable<XElement> comments, List<Comment> allComments)
        {
            IFormatProvider culture = new System.Globalization.CultureInfo("en-US", true);
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

        public void SavePost(Post post)
        {
            string outputFilePath = $"{StorageFolder}\\{post.Slug}.xml";

            XDocument doc = new XDocument();

            XElement rootNode = new XElement("Post");
            
            rootNode.Add(new XElement("Slug", post.Slug));
            rootNode.Add(new XElement("Title", post.Title));
            rootNode.Add(new XElement("Body", post.Body));
            rootNode.Add(new XElement("PubDate", post.PubDate.ToString()));
            rootNode.Add(new XElement("LastModified", post.LastModified.ToString()));
            rootNode.Add(new XElement("IsPublic", post.IsPublic.ToString()));
            rootNode.Add(new XElement("Excerpt", post.Excerpt));
            //tags
            XElement tagsNode = new XElement("Tags");
            foreach(string tag in post.Tags)
            {
                tagsNode.Add(new XElement("Tag", tag));
            }
            rootNode.Add(tagsNode);
            doc.Add(rootNode);
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
                XDocument doc = LoadInfo(expectedFilePath);

                Post post = new Post();

                post.Slug = doc.Root.Element("Slug").Value;
                post.Title = doc.Root.Element("Title").Value;
                post.Body = doc.Root.Element("Body").Value;
                post.PubDate = DateTime.Parse(doc.Root.Element("PubDate").Value, culture, System.Globalization.DateTimeStyles.AssumeLocal);
                post.LastModified = DateTime.Parse(doc.Root.Element("LastModified").Value, culture, System.Globalization.DateTimeStyles.AssumeLocal);
                post.IsPublic = Convert.ToBoolean(doc.Root.Element("IsPublic").Value);
                post.Excerpt = doc.Root.Element("Excerpt").Value;
                //comments
                post.Comments = GetAllComments(post.Slug);

                return post;
            }

            return null;
        }

        public XDocument loadDocumentFromSlug(string slug)
        {
            string filePath = $"{StorageFolder}\\{slug}.xml";
            XDocument xDoc = XDocument.Load(filePath);

            if (File.Exists(filePath))
            {
                return xDoc;
            }
            return null;
        }

        public List<Comment> GetAllComments(string slug)
        {
            if (slug == null) return null;
            IFormatProvider culture = new System.Globalization.CultureInfo("en-US", true);

            List<Comment> allComments = new List<Comment>();

            string filePath = $"{StorageFolder}\\{slug}.xml";
            XDocument xDoc = XDocument.Load(filePath);
            IEnumerable<XElement> comments;
            if (xDoc.Root.Elements("Comments").Any())
            {
                comments = xDoc.Root.Element("Comments").Elements("Comment");
                foreach (XElement comment in comments)
                {
                    Comment newComment = new Comment();
                    newComment.AuthorName = comment.Element("AuthorName").Value;
                    newComment.Body = comment.Element("CommentBody").Value;
                    newComment.AuthorEmail = comment.Element("AuthorEmail").Value;
                    newComment.PubDate = DateTime.Parse((comment.Element("PubDate").Value), culture, System.Globalization.DateTimeStyles.AssumeLocal);
                    allComments.Add(newComment);
                }
            }
            
            return allComments;
        }

        public List<Post> GetAllPosts()
        {
            string filePath = $"{StorageFolder}";
            List<FileInfo> files = new DirectoryInfo(filePath).GetFiles().OrderBy(f => f.LastWriteTime).ToList();
            List<Post> allPosts = new List<Post>();
            IFormatProvider culture = new System.Globalization.CultureInfo("en-US", true);

            for (int i = 0; i < files.Count; i++)
            {
                var file = files[files.Count - i - 1];
                //string fileContent = File.ReadAllText($"{StorageFolder}\\{file.Name}");
                //XmlDocument doc = new XmlDocument();
                //doc.LoadXml(fileContent);
                //XDocument doc = LoadInfo($"{StorageFolder}\\{file.Name}");
                XDocument doc = XDocument.Load($"{StorageFolder}\\{file.Name}");
                Post post = new Post();

                post.Title = doc.Root.Element("Title").Value;
                post.Body = doc.Root.Element("Body").Value;
                post.PubDate = DateTime.Parse(doc.Root.Element("PubDate").Value, culture, System.Globalization.DateTimeStyles.AssumeLocal);
                post.LastModified = DateTime.Parse(doc.Root.Element("LastModified").Value, culture, System.Globalization.DateTimeStyles.AssumeLocal);
                post.Slug = doc.Root.Element("Slug").Value;
                post.IsPublic = Convert.ToBoolean(doc.Root.Element("IsPublic").Value);
                post.Excerpt = doc.Root.Element("Excerpt").Value;
                post.Comments = GetAllComments(post.Slug);

                allPosts.Add(post);
            }
            return allPosts;
        }



        public bool CheckSlugExists(string slug)
        {
            return File.Exists($"{StorageFolder}\\{slug}.xml");
        }
    }
}
