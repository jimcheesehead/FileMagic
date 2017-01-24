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

        public string NewShortcut { get; set; }

        // Constructor
        public ChangeShortcutForm(string path, bool fixShorcut = false)
        {
            InitializeComponent();
            fileName = path;
            FixShortcut = fixShorcut;
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

            if (FixShortcut)
            {
                this.Text = "Fix Shortcut";
                btnSkip.Visible = false;
            }
            else
            {
                btnSkip.Visible = true;
            }

            // Show the original file (shortcut) and it's current target
            lblFile.Text = fileName;
            target = new FileInfo(ShortcutHelper.ResolveShortcut(fileName));
            lblTarget.Text = target.FullName;

            if (FixShortcut)
            {

            } else
            {
                txtNewTargetDir.Text = target.DirectoryName;
                NewShortcut = txtNewTargetDir.Text;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            
            this.Close();
        }

        private void btnSkip_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtNewTargetDir_TextChanged(object sender, EventArgs e)
        {
            NewShortcut = txtNewTargetDir.Text;
        }


        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!FixShortcut)
            {
                this.Close();
            }

            string newTarget = FindTarget(target);
        }

        private string FindTarget(FileInfo file)
        {
            string[] levels = Form1.GetDirectoryLevels(file.FullName);

            // The most common problem is an invalid disk drive
            string disk = Path.GetPathRoot(file.FullName);
            if (!Form1.diskDrives.Contains(disk))
            {
                string subPath = file.FullName.Substring(3);
                foreach (string drive in Form1.diskDrives)
                {
                    string path = drive + subPath;
                    if (File.Exists(path))
                        return path;
                }
            }


            return null;
        }
    }
}
