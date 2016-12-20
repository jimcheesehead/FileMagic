using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using FileShortcutHelper;

using System.Windows.Forms; // Only for MessageBox

namespace FileMagic
{
    class FileOps
    {
        public FileOps() // Constructor
        { }

        public int fileCount { get; private set; }
        public int folderCount { get; private set; }
        public int filesCopied { get; private set; }
        public long directorySize { get; private set; }

        // private int maxFileCount = 0;
        List<string> badLinks = new List<string>();

        public int badLinkCount
        {
            get
            {
                return badLinks.Count;
            }
        }

        void InitData()
        {
            fileCount = folderCount = filesCopied = 0;
            directorySize = 0;
            badLinks.Clear();
        }

        public int countDirectoryFiles(string dirPath)
        {
            InitData();
            DirectoryCount(dirPath);
            return fileCount;
        }

        private void DirectoryCount(string path)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(path);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + path);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            folderCount += dirs.Count();

            // Get the files in the directory
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                FileInfo currentFile = file; // Copy it so it can be changed

                if (file.Extension == @".lnk")
                {
                    // Change the current file info to the linked target file
                    currentFile = new FileInfo(ShortcutHelper.ResolveShortcut(file.FullName));

                    // Check to see if file is a directory
                    if (currentFile.Extension == String.Empty)
                    {
                        MessageBox.Show(String.Format("File {0} is a linked directory", currentFile.FullName));

                        // Recursively process this directory
                        string srcPath = ShortcutHelper.ResolveShortcut(file.FullName);
                        DirectoryCount(srcPath);
                        continue;
                    }

                    if (!currentFile.Exists)
                    {
                        // This file had a bad or missing target link
                        // MessageBox.Show(String.Format("File {0} does not exist", currentFile.FullName));
                        badLinks.Add(file.FullName);
                        fileCount++; // Count it anyway
                        continue;
                    }
                }

                directorySize += currentFile.Length;

                fileCount++;
                filesCopied++;
            }

            // If copying subdirectories, copy them and their contents to new location.
            foreach (DirectoryInfo subdir in dirs)
            {
                DirectoryCount(subdir.FullName);
            }
        }

        public void copyDirectoryFiles(string sourceDirName, string destDirName, bool copySubDirs)
        {
            InitData();
            DirectoryCopy(sourceDirName, destDirName, copySubDirs);
        }

        private void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            //int maxFileCount = this.maxFileCount;

            DirectoryInfo[] dirs = dir.GetDirectories();

            folderCount += dirs.Count();

            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                FileInfo fileToCopy;
                fileToCopy = file;

                if (file.Extension == @".lnk")
                {
                    fileToCopy = new FileInfo(ShortcutHelper.ResolveShortcut(file.FullName));

                    // Check to see if file is a directory
                    if (fileToCopy.Extension == String.Empty)
                    {
                        MessageBox.Show(String.Format("File {0} is a linked directory", fileToCopy.FullName));

                        string srcPath = ShortcutHelper.ResolveShortcut(file.FullName);
                        string temppath = Path.Combine(destDirName, fileToCopy.Name);
                        DirectoryCopy(srcPath, temppath, copySubDirs);
                        continue;
                    }

                    if (!fileToCopy.Exists)
                    {
                        // This file had a bad or missing target link
                        // MessageBox.Show(String.Format("File {0} does not exist", fileToCopy.FullName));
                        badLinks.Add(file.FullName);
                        fileCount++; // Count it anyway
                        continue;
                    }
                }

                string destPath = Path.Combine(destDirName, fileToCopy.Name);
                fileCount++;

                if (!File.Exists(destPath)) // Ignore if the file already exists
                {
                    //fileToCopy.CopyTo(destPath, false);
                    async_copy(fileToCopy.FullName, destDirName);

                    filesCopied++;
                    directorySize += fileToCopy.Length;
                }

                //if (maxFileCount > 0)
                //{
                //    if (--maxFileCount == 0)
                //        break;
                //}
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }

        private async void async_copy(string filename, string dstDirectory)
        {
            using (FileStream SourceStream = File.Open(filename, FileMode.Open))
            {
                using (FileStream DestinationStream = File.Create(dstDirectory + filename.Substring(filename.LastIndexOf('\\'))))
                {
                    await SourceStream.CopyToAsync(DestinationStream);
                }
            }
        }

    }
}
