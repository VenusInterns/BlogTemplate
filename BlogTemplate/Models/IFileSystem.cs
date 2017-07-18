using System;
using System.Collections.Generic;
using System.IO;

namespace BlogTemplate.Models
{
    public interface IFileSystem
    {
        bool FileExists(string path);
        string ReadFileText(string path);
        void WriteFileText(string path, string text);
        void DeleteFile(string path);
        DateTime GetFileLastWriteTime(string path);

        bool DirectoryExists(string path);
        void CreateDirectory(string path);
        IEnumerable<string> EnumerateFiles(string directoryPath);
    }
}
