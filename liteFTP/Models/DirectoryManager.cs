using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace liteFTP.Models
{
    public static class DirectoryManager
    {

        public static List<DirectoryItemModel> GetDrives()
        {
            return Directory.GetLogicalDrives().Select(drive => new DirectoryItemModel(DirectoryItems.Drive, drive, "C" )).ToList(); //TODO extract drive name from path
        }

        public static List<DirectoryItemModel> GetAllItems(string path)
        {
            var items = new List<DirectoryItemModel>();

            try
            {
                var directories = Directory.GetDirectories(path);

                if (directories.Length > 0)
                {
                    items.AddRange(directories.Select(directory => new DirectoryItemModel(DirectoryItems.Folder, directory, File.GetCreationTime(directory),
                       File.GetLastWriteTime(directory), GetNameFromPath(directory)
                       )));
                }
                   
            }
            catch { }

            try
            {
                var files = Directory.GetFiles(path);

                if (files.Length > 0)
                {
                    items.AddRange(files.Select(file => new DirectoryItemModel(DirectoryItems.File, file, File.GetCreationTime(file),
                       File.GetLastWriteTime(file), GetNameFromPath(file)
                       )));
                }

            }
            catch { }

            return items;
        }

        public static string GetNameFromPath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return string.Empty;

            var normalizedPath = path.Replace('/', '\\');

            var lastIndex = normalizedPath.LastIndexOf('\\');

            if (lastIndex <= 0)
                return path;

            return path.Substring(lastIndex + 1);
        }
    }
}
