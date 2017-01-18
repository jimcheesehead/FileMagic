using FileShortcutHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileMagic
{
    public partial class ChangeShortcutForm : Form
    {
        bool FixShortcut;
        string fileName;

        FileInfo file;
        FileInfo target;

        // Constructor
        public ChangeShortcutForm(string path, bool fixShorcut = false)
        {
            InitializeComponent();
            fileName = path;
            FixShortcut = fixShorcut;
            if (FixShortcut)
            {
                this.Text = "Fix Shortcut";
            }
        }

        private void ChangeShortcutForm_Load(object sender, EventArgs e)
        {
            try
            {
                file = new FileInfo(fileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "INVALID SHORTCUT FILE");
                this.Close();
            }

            if (!ShortcutHelper.IsShortcut(fileName))
            {
                MessageBox.Show(String.Format("\"{0}\" is not a shortcut", fileName), "INVALID SHORTCUT FILE");
                this.Close();
            }

            target = new FileInfo(ShortcutHelper.ResolveShortcut(fileName));
            lblFile.Text = fileName;
            lblTarget.Text = target.FullName;
            txtNewTargetDir.Text = target.DirectoryName;

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!FixShortcut)
            {
                this.Close();
            }
        }
    }
}
