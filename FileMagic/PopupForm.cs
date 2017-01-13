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
    public partial class PopupForm : Form
    {
        public string NewShortcut { get; set; }

        public PopupForm()
        {
            InitializeComponent();
        }

        public void PopupForm_Initialize(string file)
        {
            if (!ShortcutHelper.IsShortcut(file))
            {
                MessageBox.Show(String.Format("\"{0}\"\n is not a shortcut", file));
                this.Close();
            }

            FileInfo target = new FileInfo(ShortcutHelper.ResolveShortcut(file));

            lblFile.Text = file;
            lblTarget.Text = target.FullName;
            txtNewTargetDir.Text = target.DirectoryName;
            NewShortcut = txtNewTargetDir.Text;
        }


        private void btnPopupOK_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void btnPopupSkip_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void btnPopupCancel_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            NewShortcut = txtNewTargetDir.Text;
        }
    }
}
