using FileShortcutHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileMagic
{
    public partial class Form1
    {        /// <summary>
             /// For debug
             /// Create bad links in directory
             /// 
             /// </summary>
             /// <param name="sender"></param>
             /// <param name="e"></param>
        private void makeBadLinksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            srcPath = txtSrcInput.Text.TrimEnd(new[] { '\\', '/' });
            string changeDir = null;
            int changeCount = 0;

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

            //DirOps.DirInfo info;
            DirOps.Options options = GetOptions();

            string text = String.Format("Create bad links in directory \"{0}\"", srcPath);

            DialogResult dialogResult = MessageBox.Show(text, "Some Title",
                MessageBoxButtons.YesNoCancel);
            if (dialogResult != DialogResult.Yes)
            {
                return;
            }

            //PopupForm popup = new PopupForm();
            //popup.MakeBadLinks(srcPath);

            //popup.PopupForm_Initialize(file);


            var files = Directory.EnumerateFiles(srcPath);
            foreach (var file in files)
            {
                FileInfo path = new FileInfo(file);
                if (path.Extension == @".lnk")
                {
                    // Get the target file info
                    FileInfo targetFile = new FileInfo(ShortcutHelper.ResolveShortcut(file));
                    string targetName = targetFile.Name;

                    // Ignore linked directories (for now)
                    if (targetFile.Extension == String.Empty)
                    {
                        MessageBox.Show(String.Format("File {0} is a linked directory", file));
                        continue;
                    }
                    else
                    {
                        changeCount++;
                    }

                    // Set the "change directory" only once
                    if (string.IsNullOrEmpty(changeDir))
                    {
                        string targetDir = targetFile.DirectoryName;
                        PopupForm popup = new PopupForm();
                        popup.PopupForm_Initialize(file);

                        dialogResult = popup.ShowDialog();
                        if (dialogResult == DialogResult.Cancel)
                        {
                            return;
                        }
                        else if (dialogResult == DialogResult.No)
                        {
                            continue;
                        }

                        // Get the new shotcut directory
                        string newShortcut = popup.NewShortcut;

                        if (String.Equals(targetDir, newShortcut, StringComparison.OrdinalIgnoreCase))
                        {
                            // No change. Ingnore or abort
                            dialogResult = MessageBox.Show("Continue", "NO CHANGE", MessageBoxButtons.YesNo);
                            if (dialogResult == DialogResult.Yes)
                            {
                                continue;
                            }
                            else
                            {
                                return;
                            }
                        }

                        text = String.Format("Change all shortcuts directories to \"{0}\"", newShortcut);
                        dialogResult = MessageBox.Show(text, "CHANGE SHORTCUT",
                            MessageBoxButtons.YesNoCancel);
                        if (dialogResult != DialogResult.Yes)
                        {
                            return;
                        }

                        // Change all shortcuts to the new target
                        changeDir = newShortcut;
                    }

                    ShortcutHelper.ChangeShortcut(file, changeDir + "\\" + targetName);
                }
            }

            text = String.Format("{0} shortcuts changed", changeCount);
            MessageBox.Show(text);
        }
    }
}
