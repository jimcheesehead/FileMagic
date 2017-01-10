using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace FileMagic
{
    public partial class Form1 : Form
    {
        [Serializable]
        class FormData
        {
            public BindingList<string> srcInputList = new BindingList<string>();
            public BindingList<string> dstInputList = new BindingList<string>();
        }

        FormData formData = new FormData();
        string formDataFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "FileMagic.dat");

        List<string> diskDrives = new List<string>();

        string srcPath, dstPath;
        int fileCount;
        string text; // Temporary storage

        public static int FileCount { get; set; }


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (File.Exists(formDataFile))
                Deserialize(formDataFile);

            //Bind the comboboxes data source
            txtSrcInput.DataSource = formData.srcInputList;
            txtDstInput.DataSource = formData.dstInputList;

            object obj = txtSrcInput.SelectedItem;

            srcPath = txtSrcInput.SelectedItem != null ? txtSrcInput.SelectedItem.ToString() : null;
            dstPath = txtDstInput.SelectedItem != null ? txtDstInput.SelectedItem.ToString() : null;

            //srcPath = txtSrcInput.SelectedItem.ToString();
            //dstPath = txtDstInput.SelectedItem.ToString();

            DriveInfo[] allDrives = DriveInfo.GetDrives();

            // Create list of system disk drive letters
            foreach (DriveInfo d in allDrives)
            {
                diskDrives.Add(d.Name);
            }

            ShowSatus("Ready", "");

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Serialize(formDataFile);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSrcBrowse_Click(object sender, EventArgs e)
        {
            // Start browse at last used path
            if (formData.srcInputList.Count() > 0)
            {
                string path = formData.srcInputList.First();
                if (!string.IsNullOrEmpty(path) && Directory.Exists(path))
                {
                    folderBrowserDialog1.SelectedPath = path;
                }
            }

            folderBrowserDialog1.ShowNewFolderButton = false;
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txtSrcInput.Text = srcPath = folderBrowserDialog1.SelectedPath;
            }

            // The path was browsed so it exists. Save source path.
            Push(formData.srcInputList, srcPath);
            txtSrcInput.SelectedIndex = 0;
        }

        private void btnDstBrowse_Click(object sender, EventArgs e)
        {
            // Start browse at last used path
            if (formData.dstInputList.Count() > 0)
            {
                string path = formData.dstInputList.First();
                if (!string.IsNullOrEmpty(path) && Directory.Exists(path))
                {
                    folderBrowserDialog1.SelectedPath = path;
                }
            }

            folderBrowserDialog1.ShowNewFolderButton = true;
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txtDstInput.Text = dstPath = folderBrowserDialog1.SelectedPath;
            }
        }

        private void clearHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show(
                "Clear all available history?", "Some Title",
                MessageBoxButtons.YesNoCancel);
            if (dialogResult == DialogResult.Yes)
            {
                formData.srcInputList.Clear();
                formData.dstInputList.Clear();
                Serialize(formDataFile);
            }
        }

        private void btnAnalyze_Click(object sender, EventArgs e)
        {
            srcPath = txtSrcInput.Text.TrimEnd(new[] { '\\', '/' });

            if (string.IsNullOrEmpty(srcPath))
            {
                MessageBox.Show("No source directory specified");
                return;
            }

            if (!Directory.Exists(srcPath))
            {
                MessageBox.Show(String.Format("{0} is not a valid directory", srcPath));
                return;
            }

            DirOps.DirInfo info;
            DirOps.Options options = GetOptions();

            info = DirOps.GetDirInfo(srcPath, options);

            // Count the files
            // fileOps.countDirectoryFiles(srcPath);
            text = String.Format("\"{0}\" contains {1} files, {2} folders ({3})",
                srcPath, info.totalFiles, info.totalDirs,
                GetBytesReadable(info.totalBytes));

            if (info.badLinks.Count> 0)
                text += String.Format(" - {0} bad links", info.badLinks.Count);

            // Save the processed source path.
            Push(formData.srcInputList, srcPath);
            txtSrcInput.SelectedIndex = 0;

            ShowSatus("", text);
        }

        private async void btnCopy_Click(object sender, EventArgs e)
        {
            //FileOps fileOps = new FileOps();

            //srcPath = txtSrcInput.Text.TrimEnd(new[] { '\\', '/' });
            //dstPath = txtDstInput.Text.TrimEnd(new[] { '\\', '/' });

            // The source path must be vaild and exist. The source and destination paths cannot
            // be the same.
            if (checkPathErrors())
            {
                return;
            }

            // If destination directory does not exist ask to create it
            if (!Directory.Exists(dstPath))
            {
                DialogResult dialogResult = MessageBox.Show(
                   String.Format("Destination directory {0} doesn\'t exist\nCreate it?", dstPath),
                   "Warning!", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.No)
                    return;

                // Create the directory
                try
                {
                    Directory.CreateDirectory(dstPath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error");
                    return;
                }
            }

            DirOps.DirInfo info;
            DirOps.Options options = GetOptions();

            info = DirOps.GetDirInfo(srcPath, options); 
            
            // Show the the status of the background copying

            if (options.HasFlag(DirOps.Options.TopDirectoryOnly))
            {
                text = String.Format("Copying {0} files ({1})",
                    info.totalFiles, GetBytesReadable(info.totalBytes));
            }
            else
            {
                text = String.Format("Copying {0} files, {1} folders ({2})",
                    info.totalFiles, info.totalDirs, GetBytesReadable(info.totalBytes));
                if (info.badLinks.Count() > 0)
                    text += String.Format(" - {0} bad links", info.badLinks.Count());
            }
            ShowSatus("Working ", text);

            //DirOps.DirInfo inf = DirOps.GetDirectorySizes(srcPath);
            //text = String.Format("Contains {0} files, {1} folders", inf.totalFiles, inf.totalDirs);
            //MessageBox.Show(text);

            progressBar.Visible = true;
            lblPct.Visible = true;

            // Start the background worker which will monitor the current file count
            fileCount = 0;
            lblPct.Visible = true;
            backgroundWorker1.RunWorkerAsync(info.totalFiles);

            // Copy the source directory to the destination directory asynchronously
            info = await DirOps.AsyncDirectoryCopy(srcPath, dstPath,
                progressCallback, options);
            Task.WaitAll(); // Is this needed?

            if (info.badLinks.Count() > 0)
            {
                MessageBox.Show(String.Format("{0} bad links found", info.badLinks.Count()));
            }

            // Operation complete. 
            // Stop the background worker here if it's running
            backgroundWorker1.CancelAsync();

            //Save source and destination paths
            Push(formData.srcInputList, srcPath);
            txtSrcInput.SelectedIndex = 0;
            Push(formData.dstInputList, dstPath);
            txtDstInput.SelectedIndex = 0;

            // Show destination results
            DirOps.DirInfo dstInfo = DirOps.GetDirectorySizes(dstPath);
            text = String.Format("\"{0}\" Contains {1} files, {2} folders ({3})",
                dstPath, dstInfo.totalFiles, dstInfo.totalDirs,
                GetBytesReadable(dstInfo.totalBytes));

            ShowSatus("Ready", text);

        }

        private void progressCallback(DirOps.DirInfo obj)
        {
            fileCount = obj.totalFiles;
        }

        private void ShowSatus(string txtStatus, string txtResult)
        {
            lblStatus.Text = txtStatus;
            lblResult.Text = txtResult;
            progressBar.Visible = false;
            lblPct.Visible = false;
            this.Refresh();
        }

        DirOps.Options GetOptions()
        {
            DirOps.Options options = new DirOps.Options();

            options = DirOps.Options.None;
            if (chkBoxOverwrite.Checked)
            {
                options |= DirOps.Options.OverWriteFiles;
            }
            if (chkBoxTopDirOnly.Checked)
            {
                options |= DirOps.Options.TopDirectoryOnly;
            }
            return options;
        }

        private void Push(BindingList<string> list, string item)
        {
            // Remove any instances of the item
            // then add it to the top of the list
            while (list.Remove(item)) ;
            list.Insert(0, item);
        }

    }
}
