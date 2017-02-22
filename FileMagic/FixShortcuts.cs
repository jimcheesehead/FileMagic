using FileShortcutHelper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

using System.Windows.Forms;

namespace FileMagic
{
    public partial class Form1
    {
        DirOps.DirInfo info;


        int currentTxtBoxLine;
        int totalTxtBoxLines;
        const string PointsTo = " -> ";

        private void fixShortcutsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string selectedLink;

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

            // Show the bad links. Then allow fix on the first (selected) link.
            selectedLink = ShowBadLinks(info.badLinks, 0);
            FixTheShortcut(selectedLink);
        }

        private string getLinkFromText()
        {
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
            return link;
        }

        private string ShowBadLinks(List<string> badLinks, int line)
        {
            filesTextBox.Clear();
            currentTxtBoxLine = 0;
            string badLink = null;

            // Nothing more to do if there ar no bad links!
            if (badLinks.Count == 0)
            {
                ShowSatus("Ready", "");
                return badLink;
            }

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

            badLink = getLinkFromText();
            return badLink;
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

                // Allow fixing of of the selected shortcut
                FixTheShortcut(getLinkFromText());
            }
            else if (e.KeyCode == Keys.Down)
            {
                // What is the line no.?
            }
        }

        private void filesTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
            }
        }

        private void txtChangeShortcut_TextChanged(object sender, EventArgs e)
        {
            System.Drawing.SizeF mySize = new System.Drawing.SizeF();

            // Use the textbox font
            System.Drawing.Font myFont = txtChangeShortcut.Font;

            using (Graphics g = this.CreateGraphics())
            {
                // Get the size given the string and the font
                mySize = g.MeasureString(txtChangeShortcut.Text, myFont);
            }

            // Resize the textbox 
            this.txtChangeShortcut.Width = (int)Math.Round(mySize.Width, 0);
        }

        /// <summary>
        /// This is the method that does the work of fixing a bad shortcut (link)
        /// </summary>
        /// <param name="path"></param>
        private void FixTheShortcut(string path)
        {
            string target = ShortcutHelper.ResolveShortcut(path);
            string pathRoot = Path.GetPathRoot(target);

            //ChangeShortcutForm ChgShortcut = new ChangeShortcutForm(path, true);
            ChangeShortcutForm ChgShortcut = new ChangeShortcutForm(path, false);
            DialogResult dialogResult = ChgShortcut.ShowDialog();
            if (dialogResult == DialogResult.Ignore)
            {
                return;
            }
            else if (dialogResult == DialogResult.Cancel)
            {
                return;
            }

            // Get the new shortcut directory and see if we want to fix all bad links
            string targetDir = ChgShortcut.TargetDir;


            text = String.Format("Change all shortcuts directories to \"{0}\"", targetDir);
            dialogResult = MessageBox.Show(text, "CHANGE SHORTCUT",
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.None, MessageBoxDefaultButton.Button2);
            if (dialogResult == DialogResult.Cancel)
            {
                return;
            }
            if (dialogResult == DialogResult.No)
            {
                // Fix only the selected bad link
                string targetFile = RecursiveFindFile(targetDir, ChgShortcut.TargetFileName);
                if (!String.IsNullOrEmpty(targetFile))
                {
                    // Target file has been found. Fix the shortcut
                    ShortcutHelper.ChangeShortcut(path, targetFile);
                    //// Remove the bad shortcut from the list
                    info.badLinks.Remove(path);
                }
            }
            else
            {

                // Change all shortcuts to the new target
                // WARNING!! Make sure only the bad ones are fixed

                FixAllShortcuts(targetDir);
            }

            /////// TODO!!!!! //////////
            // Display the NEW directory and bad link count 

            ShowBadLinks(info.badLinks, currentTxtBoxLine);
        }

        private void FixAllShortcuts(string newTargetDir)
        {
            // Make an array copy of the badLinks items so they can be removed
            string[] badLinks = new string[info.badLinks.Count];
            info.badLinks.CopyTo(badLinks);

            // Fix the shortcut targets and remove them from the list
            // That's why we use an array copy of the bad links

            foreach (string path in badLinks)
            {
                // Find the shortcut target file
                string target = ShortcutHelper.ResolveShortcut(path);
                string targetFilename = Path.GetFileName(target);

                string targetFile = RecursiveFindFile(newTargetDir, targetFilename);
                if (!String.IsNullOrEmpty(targetFile))
                {
                    // Target file has been found. Fix the shortcut
                    ShortcutHelper.ChangeShortcut(path, targetFile);
                    //// Remove the bad shortcut from the list
                    info.badLinks.Remove(path);
                }
            }
        }

        private string RecursiveFindFile(string sDir, string sFile)
        {
            // Continues to search for ALL occurences of the file.
            // We need to stop the recursion after the first file is found!
            foreach (string dir in Directory.GetDirectories(sDir))
            {
                try
                {
                    foreach (string file in Directory.GetFiles(dir, sFile))
                    {
                        //string fileName = Path.GetFileName(file);
                        //Console.WriteLine(fileName);

                        // FILE FOUND! STOP the search
                        return file;
                    }
                    // Recursive Search
                    string path = RecursiveFindFile(dir, sFile);
                    if (!String.IsNullOrEmpty(path))
                    {
                        return path;
                    }
                }
                catch (Exception Error)
                {
                    // Probably access denied on directory like $RECYCLE.BIN
                    // Ignore the directory

                    //Console.WriteLine(Error.Message);
                }

            }

            // File not found in this tree
            return null;
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
            tableLayoutPanel2.Visible = true;
            btnUp.Enabled = state;
            btnDown.Enabled = state;
            lblPathError.Text = null;
        }

        public static int CountDirectoryLevels(string path)
        {
            FileInfo f = new FileInfo(path);
            string DirectoryName = f.DirectoryName;

            char[] delim = { '\\' };
            string[] levels = DirectoryName.Split(delim);
            return levels.Count();
        }

        public static string[] GetDirectoryLevels(string path)
        {
            FileInfo f = new FileInfo(path);
            string DirectoryName = f.DirectoryName;

            char delim = '\\';
            string s = null;
            string[] levels = DirectoryName.Split(delim);

            for (int i = 0; i < levels.Count(); i++)
            {
                if (i == 0)
                {
                    s = levels[i];
                }
                else
                {
                    s += "\\" + levels[i];
                }

                levels[i] = s;
                if (i == 0 && levels[i].Contains(":"))
                    levels[i] += "\\";

                // MessageBox.Show(levels[i]);
            }

            return levels;
        }

        public static DialogResult InputBox(string title, string promptText, ref string value)
        {
            Form form = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = title;
            label.Text = promptText;
            textBox.Text = value;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            //form.ClientSize = new Size(396, 107);
            Size clientSize = TextRenderer.MeasureText(title, form.Font);
            form.ClientSize = new Size(clientSize.Width + 100, 107);



            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            //form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            //form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();
            value = textBox.Text;
            return dialogResult;
        }
    }
}
