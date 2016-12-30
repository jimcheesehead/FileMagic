using FileShortcutHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using System.Windows.Forms; // Only for MessageBox

namespace FileMagic
{
    public partial class Form1
    {
        DirOps.DirInfo info;
        List<string> diskDrives = new List<string>();


        int currentTxtBoxLine;
        int totalTxtBoxLines;
        const string PointsTo = " -> ";

        private void fixShortcutsToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (checkPathErrors(false))
            {
                return;
            }

            EnableButtons(true);

            DirOps.Options options = GetOptions();
            info = DirOps.GetDirInfo(srcPath, options);
            if (info.badLinks.Count() == 0)
            {
                MessageBox.Show(String.Format("\"{0}\"\n Has no bad shortcuts!", srcPath));
                return;
            }

            DriveInfo[] allDrives = DriveInfo.GetDrives();

            // Create list of system disk drive letters
            foreach (DriveInfo d in allDrives)
            {
                diskDrives.Add(d.Name);
            }


            ShowBadLinks(info.badLinks, 0);
        }

        private void ShowBadLinks(List<string> badLinks, int line)
        {
            filesTextBox.Clear();
            currentTxtBoxLine = 0;

            foreach (var file in badLinks)
            {
                FileInfo shortcut = new FileInfo(file);
                FileInfo target = new FileInfo(ShortcutHelper.ResolveShortcut(file));

                string text = String.Format("{0}{1}{2}\n", shortcut.FullName, PointsTo, target.FullName);
                filesTextBox.AppendText(text);
            }

            string[] lines = filesTextBox.Lines;
            totalTxtBoxLines = lines.Count() - 1; // Last line is single \n

            SelectTextBoxLine(line);
            filesTextBox.Focus();
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            if (currentTxtBoxLine > 0)
            {
                currentTxtBoxLine--;
                SelectTextBoxLine(currentTxtBoxLine);
            }
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            if (currentTxtBoxLine < totalTxtBoxLines)
            {
                currentTxtBoxLine++;
                SelectTextBoxLine(currentTxtBoxLine);
            }
        }

        private void NavagationKey(Keys key)
        {
            if (key == Keys.Up)
            {
                if (currentTxtBoxLine > 0)
                {
                    currentTxtBoxLine--;
                    SelectTextBoxLine(currentTxtBoxLine);
                }
            }
            else if (key == Keys.Down)
            {
                if (currentTxtBoxLine < totalTxtBoxLines)
                {
                    currentTxtBoxLine++;
                    SelectTextBoxLine(currentTxtBoxLine);
                }

            }
        }

        private void filesTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;

                int line, index;
                int first, last;
                string text, link;

                index = filesTextBox.SelectionStart;
                currentTxtBoxLine = line = filesTextBox.GetLineFromCharIndex(index);
                SelectTextBoxLine(line);

                text = filesTextBox.SelectedText;
                first = text.IndexOf(PointsTo);
                last = first + PointsTo.Length;
                link = text.Substring(0, first);
                FixShortcut(link);
            }
            else if (e.KeyCode == Keys.Down)
            {
                // What is the line no.?
            }
        }

        private void FixShortcut(string path)
        {
            string target = ShortcutHelper.ResolveShortcut(path);
            string pathRoot = Path.GetPathRoot(target);

            if (!diskDrives.Contains(pathRoot))
            {
                MessageBox.Show(String.Format("{0} is not a valid drive", pathRoot));
                return;
            }

            //ShortcutHelper.ChangeShortcut(path, @"B:\foobar.txt");

            ShowBadLinks(info.badLinks, currentTxtBoxLine);
        }

        private void filesTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
            }
        }

        private void SelectTextBoxLine(int line)
        {
            if ((line < 0) || (line >= totalTxtBoxLines))
                return;

            filesTextBox.SelectionBackColor = filesTextBox.BackColor;

            int startPos = 0;
            for (int i = 0; i < line; i++)
            {
                startPos += filesTextBox.Lines[i].Length + 1; // add \n
            }

            filesTextBox.Select(startPos, filesTextBox.Lines[line].Length);
            filesTextBox.Refresh();
        }

        private void EnableButtons(bool state)
        {
            btnUp.Enabled = state;
            btnDown.Enabled = state;
        }

        /// <summary>
        /// For debug
        /// Create bad links in directory
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void makeBadLinksToolStripMenuItem_Click(object sender, EventArgs e)
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

            string text = String.Format("Create bad links in directory \"{0}\"", srcPath);

            DialogResult dialogResult = MessageBox.Show(text, "Some Title",
                MessageBoxButtons.YesNoCancel);
            if (dialogResult != DialogResult.Yes)
            {
                return;
            }

            var files = Directory.EnumerateFiles(srcPath);
            foreach (var file in files)
            {
                FileInfo path = new FileInfo(file);
                if (path.Extension == @".lnk")
                {
                    // Change the current file info to the linked target file
                    path = new FileInfo(ShortcutHelper.ResolveShortcut(file));
                    string s = path.FullName;
                    s = 'B' + s.Remove(0, 1);

                    ShortcutHelper.ChangeShortcut(file, s);


                    // Check to see if file is a directory
                    if (path.Extension == String.Empty)
                    {
                        MessageBox.Show(String.Format("File {0} is a linked directory", file));
                        continue;
                    }
                }
            }
        }
    }
}
