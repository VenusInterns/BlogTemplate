using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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



        private static XElement GetCommentsRootNode(XDocument doc)
        {
            XElement commentsNode;
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

        public IEnumerable<XElement> GetCommentRoot(string slug)
        {
            string filePath = $"{StorageFolder}\\{slug}.xml";
            XDocument xDoc = XDocument.Load(filePath);      
            IEnumerable<XElement> commentRoot = xDoc.Root.Elements("Comments");
            return commentRoot;
        }

        public void AppendCommentInfo(Comment comment, Post Post, XDocument doc)
        {
            XElement commentsNode = GetCommentsRootNode(doc);
            XElement commentNode = new XElement("Comment");
            commentNode.Add(new XElement("AuthorName", comment.AuthorName));
            commentNode.Add(new XElement("AuthorEmail", comment.AuthorEmail));
            commentNode.Add(new XElement("PubDate", comment.PubDate.ToString()));
            commentNode.Add(new XElement("CommentBody", comment.Body));
            commentNode.Add(new XElement("IsPublic", true));
            commentNode.Add(new XElement("UniqueId", comment.UniqueId));
            commentsNode.Add(commentNode);
        }

        public void SaveComment(Comment comment, Post Post)
        {
            string postFilePath = $"{StorageFolder}\\{Post.Slug}.xml";
            XDocument doc = XDocument.Load(postFilePath);
            if (comment.UniqueId == default(Guid))
            {
                comment.UniqueId = Guid.NewGuid();
            }
            AppendCommentInfo(comment, Post, doc);
            doc.Save(postFilePath);



        public IEnumerable<XElement> GetCommentRoot(string slug)
        {
            string filePath = $"{StorageFolder}\\{slug}.xml";
            XDocument xDoc = XDocument.Load(filePath);
            IEnumerable<XElement> commentRoot = xDoc.Root.Elements("Comments");
            return commentRoot;
        }

        public void IterateComments(IEnumerable<XElement> comments, List<Comment> listAllComments)
        {
            IFormatProvider culture = new System.Globalization.CultureInfo("en-US", true);
            foreach (XElement comment in comments)
            {
                Comment newComment = new Comment
                {
                    AuthorName = comment.Element("AuthorName").Value,
                    Body = comment.Element("CommentBody").Value,
                    AuthorEmail = comment.Element("AuthorEmail").Value,

                    PubDate = DateTime.Parse((comment.Element("PubDate").Value), culture, System.Globalization.DateTimeStyles.AssumeLocal),
                    IsPublic = Convert.ToBoolean(comment.Element("IsPublic").Value),
                    UniqueId = (Guid.Parse(comment.Element("UniqueId").Value)),
                    PubDate = DateTime.Parse(comment.Element("PubDate").Value, culture, System.Globalization.DateTimeStyles.AssumeLocal)

                };
                listAllComments.Add(newComment);
            }
        }

        public List<Comment> GetAllComments(string slug)
        {
            IEnumerable<XElement> commentRoot = GetCommentRoot(slug);
            IEnumerable<XElement> comments;
            List<Comment> listAllComments = new List<Comment>();
            if (commentRoot.Any())
            {
                comments = commentRoot.Elements("Comment");
                IterateComments(comments, listAllComments);
            }
            return listAllComments;
        }

        public Comment FindComment(Guid UniqueId, Post post)
        {
            List<Comment> commentsList = post.Comments;
            foreach (Comment comment in commentsList)
            {
                if (comment.UniqueId.Equals(UniqueId))
                {
                    return comment;
                }
            }
            return null;
        }

        public XElement AddTags(Post post, XElement rootNode)
        {
            XElement tagsNode = new XElement("Tags");
            foreach (string tag in post.Tags)
            {
                tagsNode.Add(new XElement("Tag", tag));
            }
            rootNode.Add(tagsNode);

            return rootNode;
        }

        public XElement AddComments(Post post, XElement rootNode)
        {
            XElement commentsNode = new XElement("Comments");

            foreach (Comment comment in post.Comments)
            {
                XElement commentNode = new XElement("Comment");
                commentNode.Add(new XElement("AuthorName", comment.AuthorName));
                commentNode.Add(new XElement("AuthorEmail", comment.AuthorEmail));
                commentNode.Add(new XElement("PubDate", comment.PubDate.ToString()));
                commentNode.Add(new XElement("CommentBody", comment.Body));
                commentNode.Add(new XElement("IsPublic", comment.IsPublic));
                commentNode.Add(new XElement("UniqueId", comment.UniqueId));
                commentsNode.Add(commentNode);
            }
            rootNode.Add(commentsNode);

            return rootNode;
        }

        public void AppendPostInfo(XElement rootNode, Post post)
        {
            rootNode.Add(new XElement("Slug", post.Slug));
            rootNode.Add(new XElement("Title", post.Title));
            rootNode.Add(new XElement("Body", post.Body));
            rootNode.Add(new XElement("PubDate", post.PubDate.ToString()));
            rootNode.Add(new XElement("LastModified", post.LastModified.ToString()));
            rootNode.Add(new XElement("IsPublic", post.IsPublic.ToString()));
            rootNode.Add(new XElement("Excerpt", post.Excerpt));
        }

        public void SavePost(Post post)
        {
            string outputFilePath = $"{StorageFolder}\\{post.Slug}.xml";
            XDocument doc = new XDocument();
            XElement rootNode = new XElement("Post");

            AppendPostInfo(rootNode, post);
            AddComments(post, rootNode);
            AddTags(post, rootNode);

            doc.Add(rootNode);
            doc.Save(outputFilePath);
        }


        public Post CollectPostInfo(string expectedFilePath)

        {
            IFormatProvider culture = new System.Globalization.CultureInfo("en-US", true);
            XDocument doc = XDocument.Load(expectedFilePath);
            Post post = new Post
            {
                Slug = doc.Root.Element("Slug").Value,
                Title = doc.Root.Element("Title").Value,
                Body = doc.Root.Element("Body").Value,
                PubDate = DateTime.Parse(doc.Root.Element("PubDate").Value, culture, System.Globalization.DateTimeStyles.AssumeLocal),
                LastModified = DateTime.Parse(doc.Root.Element("LastModified").Value, culture, System.Globalization.DateTimeStyles.AssumeLocal),
                IsPublic = Convert.ToBoolean(doc.Root.Element("IsPublic").Value),
                Excerpt = doc.Root.Element("Excerpt").Value,
            };
            post.Comments = GetAllComments(post.Slug);
            return post;
        }

        public Post GetPost(string slug)
        {
            string expectedFilePath = $"{StorageFolder}\\{slug}.xml";
            if (File.Exists(expectedFilePath))
            {
                return CollectPostInfo(expectedFilePath, slug);
            }
            return null;
        }

        public List<Post> IteratePosts(List<FileInfo> files, List<Post> allPosts)
        {
            for (int i = 0; i < files.Count; i++)
            {
                IFormatProvider culture = new System.Globalization.CultureInfo("en-US", true);
                var file = files[files.Count - i - 1];
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

        public List<Post> GetAllPosts()
        {
            string filePath = $"{StorageFolder}";
            List<FileInfo> files = new DirectoryInfo(filePath).GetFiles().OrderBy(f => f.LastWriteTime).ToList();
            List<Post> allPosts = new List<Post>();
            IFormatProvider culture = new System.Globalization.CultureInfo("en-US", true);
            return IteratePosts(files, allPosts);
        }


        public void InitStorageFolder()
        {
            Directory.CreateDirectory(StorageFolder);
        }

        public bool CheckSlugExists(string slug)
        {
            return File.Exists($"{StorageFolder}\\{slug}.xml");
        }
    }
}
