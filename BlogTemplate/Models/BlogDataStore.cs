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
        const string StorageFolder = "BlogFiles\\Posts";
        const string DraftsFolder = "BlogFiles\\Drafts";
        private static Object thisLock = new object();
        private static int CurrentId = 0;

        private IFileSystem _fileSystem;

        public BlogDataStore(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
            InitStorageFolder();
            InitCurrentId();
        }
        public void InitStorageFolder()
        {
            _fileSystem.CreateDirectory(StorageFolder);
            _fileSystem.CreateDirectory(DraftsFolder);
        }

        public void InitCurrentId()
        {
            int max = 0;
            List<string> postfiles = _fileSystem.EnumerateFiles($"{StorageFolder}").ToList();
            foreach (var file in postfiles)
            {
                int start = file.LastIndexOf("\\");
                int end = file.IndexOf(".");
                int currId = Convert.ToInt32(file.Substring(start + 1, end - start - 1));
                if(currId > max)
                {
                    max = currId;
                }
            }

            List<string> draftfiles = _fileSystem.EnumerateFiles($"{DraftsFolder}").ToList();
            foreach(var file in draftfiles)
            {
                int start = file.IndexOf("_");
                int end = file.IndexOf(".");
                int currId = Convert.ToInt32(file.Substring(start + 1, end - start - 1));
                if (currId > max)
                {
                    max = currId;
                }
            }

            CurrentId = max;
        }

        public void SetId(Post post)
        {
            lock(thisLock)
            {
                post.Id = ++CurrentId;
            }
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

        private XDocument LoadPostXml(string filePath)
        {
            string text = _fileSystem.ReadFileText(filePath);
            StringReader reader = new StringReader(text);

            return XDocument.Load(reader);
        }

        public IEnumerable<XElement> GetCommentRoot(XDocument doc)
        {
            IEnumerable<XElement> commentRoot = doc.Root.Elements("Comments");
            return commentRoot;
        }

        public void AppendCommentInfo(Comment comment, Post Post, XDocument doc)
        {
            XElement commentsNode = GetCommentsRootNode(doc);
            XElement commentNode = new XElement("Comment");
            commentNode.Add(new XElement("AuthorName", comment.AuthorName));
            commentNode.Add(new XElement("AuthorEmail", comment.AuthorEmail));
            commentNode.Add(new XElement("PubDate", comment.PubDate.ToString("o")));
            commentNode.Add(new XElement("CommentBody", comment.Body));

            commentNode.Add(new XElement("IsPublic", true));
            commentNode.Add(new XElement("UniqueId", comment.UniqueId));

            commentsNode.Add(commentNode);
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

                    PubDate = DateTime.Parse(comment.Element("PubDate").Value),
                    IsPublic = Convert.ToBoolean(comment.Element("IsPublic").Value),
                    UniqueId = (Guid.Parse(comment.Element("UniqueId").Value)),

                };
                listAllComments.Add(newComment);
            }
        }

        public List<Comment> GetAllComments(XDocument doc)
        {
            IEnumerable<XElement> commentRoot = GetCommentRoot(doc);
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
                commentNode.Add(new XElement("PubDate", comment.PubDate.ToString("o")));
                commentNode.Add(new XElement("CommentBody", comment.Body));
                commentNode.Add(new XElement("IsPublic", comment.IsPublic));
                commentNode.Add(new XElement("UniqueId", comment.UniqueId));
                commentsNode.Add(commentNode);
            }
            rootNode.Add(commentsNode);

            return rootNode;
        }
        public List<string> GetTags(XDocument doc)
        {
            List<string> tags = new List<string>();
            IEnumerable<XElement> tagElements = doc.Root.Element("Tags").Elements("Tag");
            if (tagElements.Any())
            {
                foreach (string tag in tagElements)
                {
                    tags.Add(tag);
                }
            }

            return tags;
        }


        public void AppendPostInfo(Post post, XElement rootNode)
        {
            rootNode.Add(new XElement("Id", post.Id.ToString()));
            rootNode.Add(new XElement("Slug", post.Slug));
            rootNode.Add(new XElement("Title", post.Title));
            rootNode.Add(new XElement("Body", post.Body));
            rootNode.Add(new XElement("PubDate", post.PubDate.ToString("o")));
            rootNode.Add(new XElement("LastModified", post.LastModified.ToString("o")));
            rootNode.Add(new XElement("IsPublic", post.IsPublic.ToString()));
            rootNode.Add(new XElement("Excerpt", post.Excerpt));
        }

        public void SavePost(Post post)
        {
            SetId(post);
            string outputFilePath;
            if (post.IsPublic == true)
            {
                outputFilePath = $"{StorageFolder}\\{post.PubDate.ToFileTimeUtc()}_{post.Id}.xml";
            }
            else
            {
                outputFilePath = $"{DraftsFolder}\\{post.Id}.xml";
            }
            XDocument doc = new XDocument();
            XElement rootNode = new XElement("Post");

            AppendPostInfo(post, rootNode);
            AddComments(post, rootNode);
            AddTags(post, rootNode);
            doc.Add(rootNode);

            using (MemoryStream ms = new MemoryStream())
            {
                doc.Save(ms);
                ms.Seek(0, SeekOrigin.Begin);
                using (StreamReader reader = new StreamReader(ms))
                {
                    string text = reader.ReadToEnd();
                    _fileSystem.WriteFileText(outputFilePath, text);
                }
            }
        }


        public Post CollectPostInfo(string expectedFilePath)
        {
            IFormatProvider culture = new System.Globalization.CultureInfo("en-US", true);
            XDocument doc = LoadPostXml(expectedFilePath);
            Post post = new Post
            {
                Id = Convert.ToInt32(doc.Root.Element("Id").Value),
                Slug = doc.Root.Element("Slug").Value,
                Title = doc.Root.Element("Title").Value,
                Body = doc.Root.Element("Body").Value,
                PubDate = DateTime.Parse(doc.Root.Element("PubDate").Value),
                LastModified = DateTime.Parse(doc.Root.Element("LastModified").Value),
                IsPublic = Convert.ToBoolean(doc.Root.Element("IsPublic").Value),
                Excerpt = doc.Root.Element("Excerpt").Value,
            };
            post.Comments = GetAllComments(doc);
            post.Tags = GetTags(doc);
            return post;
        }

        public Post GetPost(int id)
        {
            string expectedFilePath = $"{DraftsFolder}\\{id}.xml";
            if (_fileSystem.FileExists(expectedFilePath))
            {
                return CollectPostInfo(expectedFilePath);
            }
            else
            {
                List<string> files = _fileSystem.EnumerateFiles($"{StorageFolder}").ToList();
                foreach(var file in files)
                {
                    int start = file.IndexOf("_");
                    int end = file.IndexOf(".");
                    string element = file.Substring(start + 1, end - start - 1);
                    if(element == id.ToString())
                    {
                        return CollectPostInfo(file);
                    }
                }
            }
            return null;
        }

        private List<Post> IteratePosts(List<string> files, List<Post> allPosts)
        {
            foreach (var file in files)
            {
                Post post = CollectPostInfo(file);
                allPosts.Add(post);
            }
            return allPosts;
        }

        public List<Post> GetAllPosts()
        {
            string filePath = $"{StorageFolder}";
            List<string> files = _fileSystem.EnumerateFiles(filePath).OrderByDescending(f => f).ToList();
            List<Post> allPosts = new List<Post>();
            return IteratePosts(files, allPosts);
        }

        public List<Post> GetAllDrafts()
        {
            string filePath = $"{DraftsFolder}";
            List<string> files = _fileSystem.EnumerateFiles(filePath).OrderByDescending(f => f).ToList();
            List<Post> allDrafts = new List<Post>();
            return IteratePosts(files, allDrafts);
        }

        public void UpdatePost(Post newPost, Post oldPost)
        {
            if(oldPost.IsPublic)
            {
                _fileSystem.DeleteFile($"{StorageFolder}\\{oldPost.PubDate.ToFileTimeUtc()}_{oldPost.Id}.xml");
            }
            else
            {
                _fileSystem.DeleteFile($"{DraftsFolder}\\{oldPost.Id}.xml");
            }
            SavePost(newPost);
        }

        public bool CheckSlugExists(string slug)
        {
            return _fileSystem.FileExists($"{StorageFolder}\\{slug}.xml");
        }
    }
}
