using FileShortcutHelper;
using System;
using System.IO;
using System.Linq;

using System.Windows.Forms; // Only for MessageBox

namespace FileMagic
{
    public partial class Form1
    {
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
            filesTextBox.Clear();
            currentTxtBoxLine = 0;

            DirOps.DirInfo info;
            DirOps.Options options = GetOptions();
            info = DirOps.GetDirInfo(srcPath, options);
            if (info.badLinks.Count() == 0)
            {
                MessageBox.Show(String.Format("\"{0}\"\n Has no bad shortcuts!", srcPath));
                return;
            }

            foreach (var file in info.badLinks)
            {
                FileInfo shortcut = new FileInfo(file);
                FileInfo target = new FileInfo(ShortcutHelper.ResolveShortcut(file));

                string text = String.Format("{0}{1}{2}\n", shortcut.FullName, PointsTo, target.FullName);
                //string text = String.Format("{0}\n", "TEXT123");
                filesTextBox.AppendText(text);
            }

            string[] lines = filesTextBox.Lines;
            totalTxtBoxLines = lines.Count() - 1; // Last line is single \n

            SelectTextBoxLine(0); ////// Works but needs to be fixed
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
                string text, link, target, shortcut;

                index = filesTextBox.SelectionStart;
                currentTxtBoxLine = line = filesTextBox.GetLineFromCharIndex(index);
                SelectTextBoxLine(line);

                text = filesTextBox.SelectedText;
                first = text.IndexOf(PointsTo);
                last = first + PointsTo.Length;
                link = text.Substring(0, first);

                shortcut = ShortcutHelper.ResolveShortcut(link);

                ShortcutHelper.ChangeShortcut(link, @"X:\foobar.txt");

                //target = text.Substring(last);
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

        private void SelectTextBoxLine(int line)
        {
            if ((line < 0) || (line >= totalTxtBoxLines))
                return;

            filesTextBox.SelectionBackColor = filesTextBox.BackColor;

            int startPos = 0;
            for (int i = 0; i < line; i++)
            {
                startPos += filesTextBox.Lines[i].Length +1; // add \n
            }

            filesTextBox.Select(startPos, filesTextBox.Lines[line].Length);
            filesTextBox.Refresh();
        }

        private void EnableButtons(bool state)
        {
            btnUp.Enabled = state;
            btnDown.Enabled = state;
        }

    }
}
