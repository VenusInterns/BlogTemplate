using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BlogTemplate._1.Models;
using Xunit;

namespace BlogTemplate._1.Tests.Fakes
{
    public class FakeFileSystem : IFileSystem
    {
        HashSet<string> _directories = new HashSet<string>();
        Dictionary<string, MemoryStream> _files = new Dictionary<string, MemoryStream>();

        public void AddFile(string filePath)
        {
            AddFile(filePath, new byte[0]);
        }

        public void AddFile(string filePath, string content)
        {
            // TODO: Should we be using default encoding?  If we use something specific, it should probably
            // be consistenly used by the data store to read the content correctly as well.
            byte[] contentBytes = Encoding.Default.GetBytes(content);

            AddFile(filePath, contentBytes);
        }

        public void AddFile(string filePath, byte[] content)
        {
            _files.Add(filePath, new MemoryStream(content.Length));
            _files[filePath].Write(content, 0, content.Length);
            AddDirectory(Path.GetDirectoryName(filePath));
        }

        public void AddDirectory(string path)
        {
            while(!string.IsNullOrEmpty(path))
            {
                _directories.Add(path);
                path = Path.GetDirectoryName(path);
            }
        }


        #region IFileSystem
        void IFileSystem.WriteFile(string path, byte[] data)
        {
            AddFile(path, data);
        }

        void IFileSystem.CreateDirectory(string path)
        {
            AddDirectory(path);
        }

        void IFileSystem.DeleteFile(string path)
        {
            if(!_files.ContainsKey(path))
            {
                throw new FileNotFoundException(path);
            }

            _files.Remove(path);
        }

        bool IFileSystem.DirectoryExists(string path)
        {
            return _directories.Contains(path);
        }

        IEnumerable<string> IFileSystem.EnumerateFiles(string directoryPath)
        {
            IEnumerable<string> filenames = _files.Keys.Where(key => string.Equals(Path.GetDirectoryName(key), directoryPath, StringComparison.OrdinalIgnoreCase));
            return filenames;
        }

        bool IFileSystem.FileExists(string path)
        {
            return _files.ContainsKey(path);
        }

        DateTime IFileSystem.GetFileLastWriteTime(string path)
        {
            return DateTime.UtcNow;
        }

        string IFileSystem.ReadFileText(string path)
        {
            if(!_files.ContainsKey(path))
            {
                throw new FileNotFoundException(path);
            }

            _files[path].Seek(0, SeekOrigin.Begin);
            StreamReader reader = new StreamReader(_files[path]);
            return reader.ReadToEnd();
        }

        void IFileSystem.WriteFileText(string path, string text)
        {
            if(!_files.ContainsKey(path))
            {
                AddFile(path);
            }

            _files[path].Seek(0, SeekOrigin.Begin);
            StreamWriter writer = new StreamWriter(_files[path]);
            writer.Write(text);
            writer.Flush();
        }

        void IFileSystem.AppendFile(string path, byte[] data)
        {
            ((IFileSystem)this).AppendFile(path, data, 0, data.Length);
        }

        void IFileSystem.AppendFile(string path, byte[] data, int offset, int count)
        {
            if (!_files.ContainsKey(path))
            {
                AddFile(path);
            }

            _files[path].Seek(0, SeekOrigin.Begin);
            MemoryStream writer = _files[path];
            writer.Write(data, offset, count);
            writer.Flush();
        }
        #endregion

        #region Tests
        public class FakeFileSystemTests
        {
            [Fact]
            public void EmptyFileSystem_DirectoryExists_ReturnsFalse()
            {
                IFileSystem ut = new FakeFileSystem();

                Assert.False(ut.DirectoryExists("test"));
            }

            [Fact]
            public void EmptyFileSystem_FileExists_ReturnsFalse()
            {
                IFileSystem ut = new FakeFileSystem();

                Assert.False(ut.FileExists("test"));
            }

            [Fact]
            public void CreateDirectory_AddsParentDirectories()
            {
                IFileSystem ut = new FakeFileSystem();

                ut.CreateDirectory(@"test\path");

                Assert.True(ut.DirectoryExists(@"test\path"));
                Assert.True(ut.DirectoryExists(@"test"));
            }

            [Fact]
            public void AddFile_AddsParentDirectories()
            {
                IFileSystem ut = new FakeFileSystem();

                ((FakeFileSystem)ut).AddFile(@"test\path\file.txt");

                Assert.True(ut.FileExists(@"test\path\file.txt"));
                Assert.True(ut.DirectoryExists(@"test\path"));
                Assert.True(ut.DirectoryExists(@"test"));
            }

            [Fact]
            public void AddFile_CanRetrieveTextContent()
            {
                IFileSystem ut = new FakeFileSystem();
                string filePath = @"test\path\file.txt";

                ((FakeFileSystem)ut).AddFile(filePath, "sample content");

                Assert.Equal("sample content", ut.ReadFileText(filePath));
            }

            [Fact]
            public void EnumerateFiles_OnlyReturnsFilesInDirectory()
            {
                IFileSystem ut = new FakeFileSystem();

                ((FakeFileSystem)ut).AddFile(@"test\fail.txt");
                ((FakeFileSystem)ut).AddFile(@"test\path\subfolder\fail.txt");
                ((FakeFileSystem)ut).AddFile(@"test\path\file1.txt");
                ((FakeFileSystem)ut).AddFile(@"test\path\file2.txt");

                Assert.Equal(2, ut.EnumerateFiles(@"test\path").Count());
                Assert.False(ut.EnumerateFiles(@"test\path").Any(s => s.Equals(Path.GetFileName("fail.txt"), StringComparison.OrdinalIgnoreCase)));
            }

            [Fact]
            public void WriteFileText_IfFileDoesNotExit_CreatesNewFile()
            {
                IFileSystem ut = new FakeFileSystem();
                string filePath = @"test\path\file.txt";

                ut.WriteFileText(filePath, "test");

                Assert.True(ut.FileExists(filePath));
                Assert.Equal("test", ut.ReadFileText(filePath));
            }
        }
        #endregion

    }
}
