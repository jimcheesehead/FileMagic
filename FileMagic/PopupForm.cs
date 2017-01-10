using FileShortcutHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileMagic
{
    public partial class PopupForm : Form
    {
        string CurrentShortcut;
        string NewShortcut { get; set; }

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

            CurrentShortcut = ShortcutHelper.ResolveShortcut(file);

            label1.Text = file;
            label3.Text = CurrentShortcut;
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
    }
}
