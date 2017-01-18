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
        bool fixShortcut;

        public PopupForm(string formText)
        {
            InitializeComponent();
            this.Text = formText;
        }

        private void PopupForm_Load(object sender, EventArgs e)
        {

        }

        public void PopupForm_Initialize(string file, bool FixShortcut = false)
        {
            if (!ShortcutHelper.IsShortcut(file))
            {
                MessageBox.Show(String.Format("\"{0}\"\n is not a shortcut", file));
                this.Close();
            }

            fixShortcut = FixShortcut;
            FileInfo target = new FileInfo(ShortcutHelper.ResolveShortcut(file));

            lblFile.Text = file;
            lblTarget.Text = target.FullName;
            txtNewTargetDir.Text = target.DirectoryName;
            NewShortcut = txtNewTargetDir.Text;
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


        private void btnPopupOK_Click(object sender, EventArgs e)
        {
            if (!fixShortcut)
            {
                this.Close();
            }

            if (!Directory.Exists(NewShortcut))
            {
                string text = String.Format("\"{0}\"\n is an INVALID directory", NewShortcut);
                MessageBox.Show(text,"INVALID DIRECTORY");
            }
        }

    }
}
