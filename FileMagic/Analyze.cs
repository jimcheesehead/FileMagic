using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using FileShortcutHelper;

using System.Windows.Forms;


namespace FileMagic
{
    class Analyze
    {
        public Analyze() // Constructor
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
                        DirectoryCount(srcPath);
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

                directorySize += fileToCopy.Length;

                fileCount++;
                filesCopied++;
            }

            // If copying subdirectories, copy them and their contents to new location.
            foreach (DirectoryInfo subdir in dirs)
            {
                DirectoryCount(subdir.FullName);
            }
        }
    }
}
