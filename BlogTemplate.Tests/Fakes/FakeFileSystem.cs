using BlogTemplate.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using Xunit;

namespace BlogTemplate.Tests.Fakes
{
    public class FakeFileSystem : IFileSystem
    {
        HashSet<string> _directories = new HashSet<string>();
        Dictionary<string, byte[]> _files = new Dictionary<string, byte[]>();

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
            _files.Add(filePath, content);
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
            throw new NotSupportedException();
        }

        Stream IFileSystem.OpenFileRead(string path)
        {
            if(!_files.ContainsKey(path))
            {
                throw new FileNotFoundException(path);
            }

            MemoryStream fileStream = new MemoryStream(_files[path], false);
            return fileStream;
        }

        Stream IFileSystem.OpenFileWrite(string path)
        {
            if(!_files.ContainsKey(path))
            {
                throw new FileNotFoundException(path);
            }

            // Initialize by length but not buffer content so that it will be resizeable.
            // This is a writeable stream, afterall.
            MemoryStream fileStream = new MemoryStream(_files[path].Length);
            fileStream.Write(_files[path], 0, _files[path].Length);
            fileStream.Seek(0, SeekOrigin.Begin);

            return fileStream;
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

                using (StreamReader reader = new StreamReader(ut.OpenFileRead(filePath)))
                {
                    Assert.Equal("sample content", reader.ReadToEnd());
                }
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
            public void OpenFileRead_StreamIsNotWriteable()
            {
                IFileSystem ut = new FakeFileSystem();
                string filePath = @"test\path\file.txt";

                ((FakeFileSystem)ut).AddFile(filePath);

                Assert.False(ut.OpenFileRead(filePath).CanWrite);
            }

            [Fact]
            public void OpenFileWrite_StreamIsWriteableAndResizeable()
            {
                IFileSystem ut = new FakeFileSystem();
                string filePath = @"test\path\file.txt";

                ((FakeFileSystem)ut).AddFile(filePath, "");
                Stream writeStream = ut.OpenFileWrite(filePath);

                Assert.True(writeStream.CanWrite);
                // this should not throw
                writeStream.Write(new byte[] { 0x7e, 0x57 }, 0, 2);
            }
        }
        #endregion

    }
}
