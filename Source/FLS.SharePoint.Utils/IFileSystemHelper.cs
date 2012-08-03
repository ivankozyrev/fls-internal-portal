namespace FLS.SharePoint.Utils
{
    public interface IFileSystemHelper
    {
        byte[] ReadFile(string path);

        string CombinePath(string directory, string fileName);
    }
}
