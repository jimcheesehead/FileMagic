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

        string srcPath, dstPath;

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
            srcPath = txtSrcInput.SelectedText;
            dstPath = txtDstInput.SelectedText;

            // ShowSatus("Ready", "");

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Serialize(formDataFile);
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
            Analyze analyze = new Analyze();

            if (string.IsNullOrEmpty(txtSrcInput.Text))
            {
                MessageBox.Show("No source directory specified");
                return;
            }
            srcPath = txtSrcInput.Text.TrimEnd(new[] { '\\', '/' });
            if (!Directory.Exists(srcPath))
            {
                MessageBox.Show(String.Format("{0} is not a valid directory", srcPath));
                return;
            }

            // Count the files
            analyze.countDirectoryFiles(srcPath);

            string text = String.Format("Contains {0} files, {1} folders ({2})",
                analyze.fileCount, analyze.folderCount,
                GetBytesReadable(analyze.directorySize));

            if (analyze.badLinkCount > 0)
                text += String.Format(" - {0} bad links", analyze.badLinkCount);

            MessageBox.Show(text);
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {

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
