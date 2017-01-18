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

            // Show the bad links. Fixes will be initiated by selecting a bad link.
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
                FixTheShortcut(link);
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

            PopupForm popup = new PopupForm("FIX shortcut");
            popup.PopupForm_Initialize(path, true);
            DialogResult dialogResult = popup.ShowDialog();
            if (dialogResult != DialogResult.OK) 
            {
                return;
            }

            return;






            fixShortcutPanel.Visible = true;

            if (!diskDrives.Contains(pathRoot))
            {
                string format = String.Format("INVALID DISK DRIVE \"{0}\"", pathRoot);
                lblPathError.Text = format;
                txtChangeShortcut.Text = target;
                this.Refresh();

                //MessageBox.Show(String.Format("{0} is not a valid drive", pathRoot));
                return;
            }

            //ShortcutHelper.ChangeShortcut(path, @"B:\foobar.txt");

            ShowBadLinks(info.badLinks, currentTxtBoxLine);
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

            char[] delim = {'\\'};
            string[] levels = DirectoryName.Split(delim);
            return levels.Count();
        }

        public string[] GetDirectoryLevels(string path)
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
