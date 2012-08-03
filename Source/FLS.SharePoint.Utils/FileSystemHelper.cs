using System.IO;

namespace FLS.SharePoint.Utils
{
    public class FileSystemHelper : IFileSystemHelper
    {
        public byte[] ReadFile(string path)
        {
            return File.ReadAllBytes(path);
        }

        public string CombinePath(string directory, string fileName)
        {
            return Path.Combine(directory, fileName);
        }
    }
}
