using System;
using System.Collections.Generic;
using System.IO;

namespace BlogTemplate._1.Models
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

        public void WriteFile(string path, byte[] data)
        {
            File.WriteAllBytes(path, data);
        }

        public void AppendFile(string path, byte[] data)
        {
            Stream outStream = File.OpenWrite(path);
            outStream.Seek(0, SeekOrigin.End);
            outStream.Write(data, 0, data.Length);
        }

        public void AppendFile(string path, byte[] data, int offset, int count)
        {
            using (Stream outStream = File.OpenWrite(path))
            {
                outStream.Seek(0, SeekOrigin.End);
                outStream.Write(data, offset, count);
            }
        }
    }
}
