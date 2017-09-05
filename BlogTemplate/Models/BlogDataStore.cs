using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http;

namespace BlogTemplate._1.Models
{
    public class BlogDataStore
    {
        const string UploadsFolder = "wwwroot\\Uploads";
        const string PostsFolder = "BlogFiles\\Posts";
        const string DraftsFolder = "BlogFiles\\Drafts";
        private static Object thisLock = new object();
        protected static int CurrentId = 0;

        private readonly IFileSystem _fileSystem;

        public BlogDataStore(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
            InitStorageFolders();
            InitCurrentId();
        }
        public void InitStorageFolders()
        {
            _fileSystem.CreateDirectory(PostsFolder);
            _fileSystem.CreateDirectory(DraftsFolder);
            _fileSystem.CreateDirectory(UploadsFolder);
        }

        private void InitCurrentId()
        {
            if (CurrentId == 0)
            {
                lock (thisLock)
                {
                    if (CurrentId == 0)
                    {
                        int max = 0;
                        IEnumerable<string> postfiles = _fileSystem.EnumerateFiles(PostsFolder).Select(f => Path.GetFileName(f));
                        foreach (var file in postfiles)
                        {
                            int start = file.IndexOf("_");
                            int end = file.IndexOf(".");
                            int currId = Convert.ToInt32(file.Substring(start + 1, end - start - 1));
                            if (currId > max)
                            {
                                max = currId;
                            }
                        }

                        IEnumerable<string> draftfiles = _fileSystem.EnumerateFiles(DraftsFolder).Select(f => Path.GetFileName(f));
                        foreach (var file in draftfiles)
                        {
                            int start = 0;
                            int end = file.IndexOf(".");
                            int currId = Convert.ToInt32(file.Substring(start, end));
                            if (currId > max)
                            {
                                max = currId;
                            }
                        }

                        CurrentId = max;
                    }
                }
            }
        }

        private void SetId(Post post)
        {
            if (post.Id == 0)
            {
                lock (thisLock)
                {
                    post.Id = ++CurrentId;
                }

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
            commentNode.Add(new XElement("PubDate", comment.PubDate.ToString("o")));
            commentNode.Add(new XElement("CommentBody", comment.Body));
            commentNode.Add(new XElement("IsPublic", true));
            commentNode.Add(new XElement("UniqueId", comment.UniqueId));

            commentsNode.Add(commentNode);
        }

        public void IterateComments(IEnumerable<XElement> comments, List<Comment> listAllComments)
        {
            foreach (XElement comment in comments)
            {
                Comment newComment = new Comment
                {
                    AuthorName = comment.Element("AuthorName").Value,
                    Body = comment.Element("CommentBody").Value,
                    PubDate = DateTimeOffset.Parse(comment.Element("PubDate").Value),
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

        public XElement AddComments(Post post, XElement rootNode)
        {
            XElement commentsNode = new XElement("Comments");

            foreach (Comment comment in post.Comments)
            {
                XElement commentNode = new XElement("Comment");
                commentNode.Add(new XElement("AuthorName", comment.AuthorName));
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
            rootNode.Add(new XElement("IsDeleted", post.IsDeleted.ToString()));
            rootNode.Add(new XElement("Excerpt", post.Excerpt));
        }

        public void SavePost(Post post)
        {
            SetId(post);
            string outputFilePath;
            if (post.IsPublic == true)
            {
                string date = post.PubDate.UtcDateTime.ToString("s").Replace(":", "-");
                outputFilePath = $"{PostsFolder}\\{date}_{post.Id}.xml";
            }
            else
            {
                outputFilePath = $"{DraftsFolder}\\{post.Id}.xml";
            }
            XDocument doc = new XDocument();
            XElement rootNode = new XElement("Post");

            AppendPostInfo(post, rootNode);
            AddComments(post, rootNode);
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
            XDocument doc = LoadPostXml(expectedFilePath);
            Post post = new Post();
            if (doc.Root.Element("Id") != null && !doc.Root.Element("Id").IsEmpty)
            {
                post.Id = Convert.ToInt32(doc.Root.Element("Id").Value);
            }
            else
            {
                SetId(post);
            }
            post.Slug = (doc.Root.Element("Slug") != null && !doc.Root.Element("Slug").IsEmpty) ?
                doc.Root.Element("Slug").Value : "";
            post.Title = (doc.Root.Element("Title") != null && !doc.Root.Element("Title").IsEmpty) ?
                doc.Root.Element("Title").Value : "";
            post.Body = (doc.Root.Element("Body") != null && !doc.Root.Element("Body").IsEmpty) ?
                doc.Root.Element("Body").Value : "";
            post.PubDate = (doc.Root.Element("PubDate") != null && !doc.Root.Element("PubDate").IsEmpty) ?
                DateTimeOffset.Parse(doc.Root.Element("PubDate").Value) : default(DateTimeOffset);
            post.LastModified = (doc.Root.Element("LastModified") != null && !doc.Root.Element("LastModified").IsEmpty) ?
                DateTimeOffset.Parse(doc.Root.Element("LastModified").Value) : default(DateTimeOffset);
            post.IsPublic = (doc.Root.Element("IsPublic") != null && !doc.Root.Element("IsPublic").IsEmpty) ?
                Convert.ToBoolean(doc.Root.Element("IsPublic").Value) : true;
            post.IsDeleted = (doc.Root.Element("IsDeleted") != null && !doc.Root.Element("IsDeleted").IsEmpty) ?
                Convert.ToBoolean(doc.Root.Element("IsDeleted").Value) : false;
            post.Excerpt = (doc.Root.Element("Excerpt") != null && !doc.Root.Element("Excerpt").IsEmpty) ?
                doc.Root.Element("Excerpt").Value : "";
            post.Comments = GetAllComments(doc);
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
                List<string> files = _fileSystem.EnumerateFiles($"{PostsFolder}").ToList();
                foreach (var file in files)
                {
                    int start = file.IndexOf("_");
                    int end = file.IndexOf(".");
                    string element = file.Substring(start + 1, end - start - 1);
                    if (element == id.ToString())
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
            string filePath = $"{PostsFolder}";
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

        public void UpdatePost(Post post, bool wasPublic)
        {
            if (wasPublic)
            {
                string date = post.PubDate.UtcDateTime.ToString("s").Replace(":", "-");
                _fileSystem.DeleteFile($"{PostsFolder}\\{date}_{post.Id}.xml");
            }
            else
            {
                _fileSystem.DeleteFile($"{DraftsFolder}\\{post.Id}.xml");
            }
            SavePost(post);
        }

        public void SaveFiles(List<IFormFile> files)
        {
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    using (Stream uploadedFileStream = file.OpenReadStream())
                    {
                        byte[] buffer = new byte[uploadedFileStream.Length];
                        uploadedFileStream.Read(buffer, 0, buffer.Length);
                        string name = CreateFileName(file.FileName);
                        _fileSystem.WriteFile($"{UploadsFolder}\\{name}", buffer);
                    }
                }
            }
        }

        public IEnumerable<string> GetFileNames()
        {
            IEnumerable<string> fileNames = _fileSystem.EnumerateFiles(UploadsFolder);
            return fileNames;
        }

        private bool CheckFileNameExists(string fileName)
        {
            return _fileSystem.FileExists($"{UploadsFolder}\\{fileName}");
        }

        private string CreateFileName(string fileName)
        {
            string tempName = fileName;
            string[] elements = fileName.Split(".");
            int count = 0;
            while (CheckFileNameExists(tempName))
            {
                count++;
                if (elements.Length > 1)
                {
                    tempName = $"{elements[0]}-{count}.{elements[1]}";
                }
                else
                {
                    tempName = $"{fileName}-{count}";
                }
            }
            return tempName;
        }

    }
}
