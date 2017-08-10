using System;
using System.Collections.Generic;
using System.IO;

namespace BlogTemplate.Models
{
    public class PhysicalFileSystem : IFileSystem
    {
        public void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }

        public bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        public void DeleteFile(string path)
        {
            File.Delete(path);
        }

        public bool FileExists(string path)
        {
            return File.Exists(path);
        }

        public string ReadFileText(string path)
        {
            return File.ReadAllText(path);
        }

        public void WriteFileText(string path, string text)
        {
            File.WriteAllText(path, text);
        }

        public IEnumerable<string> EnumerateFiles(string directoryPath)
        {
            return Directory.EnumerateFiles(directoryPath);
        }

        public DateTime GetFileLastWriteTime(string path)
        {
            return File.GetLastWriteTime(path);
        }
    }
}
