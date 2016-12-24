using FileShortcutHelper;
using System;
using System.IO;
using System.Linq;

using System.Windows.Forms; // Only for MessageBox

namespace FileMagic
{
    public partial class Form1
    {
        private void fixShortcutsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (checkPathErrors(false))
            {
                return;
            }

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

                string text = String.Format("{0} -> {1}\n", shortcut.FullName, target.FullName);
                filesTextBox.AppendText(text);
            }
        }
    }
}
