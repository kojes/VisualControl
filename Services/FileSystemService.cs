using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace VueExample.Services
{
    public class FileSystemService
    {
        public static string FindFolderInTemporaryFolder(string folderName)
        {
            var directories = Directory.GetDirectories(Path.GetTempPath()).Select(Path.GetFileName).ToList();
            return directories.FirstOrDefault(x => x == folderName);
        }

        public static void DeleteFolderInTemporaryFolder(string folderName)
        {
            Directory.Delete(Path.GetTempPath() + folderName, true);
        }

        public static string GetFirstFilePathFromFolderInTemporaryFolder(string folderName)
        {
            var di = new DirectoryInfo(Path.GetTempPath() + folderName);
            return Path.GetTempPath()  + folderName + "\\" + di.GetFiles().Select(fi => fi.Name).FirstOrDefault(name => name != "Thumbs.db");
        }

        public static string CreateNewFolder(string path, string folderName)
        {
             return System.IO.Directory.CreateDirectory(path + "\\" + folderName).FullName;
        }
    }
}
