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
        //string targetFileName;

        FileInfo file;
        FileInfo target;

        public string TargetDir { get; private set; }
        public string TargetFileName { get; private set; }

        // Constructor
        public ChangeShortcutForm(string path, bool fixShorcut = false)
        {
            InitializeComponent();
            fileName = path;
            FixShortcut = fixShorcut;

            try
            {
                file = new FileInfo(fileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "INVALID SHORTCUT FILE");
                this.Close();
            }

        }

        private void ChangeShortcutForm_Load(object sender, EventArgs e)
        {
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

            // Get the shortcut target and set properties

            target = new FileInfo(ShortcutHelper.ResolveShortcut(fileName));
            TargetDir = target.DirectoryName;
            TargetFileName = target.Name;


            string targetPathRoot = Path.GetPathRoot(target.FullName);
            if (!Directory.Exists(targetPathRoot))
            {
                char[] a1 = fileName.ToCharArray();
                char[] a2 = TargetDir.ToCharArray();
                a2[0] = a1[0];
                TargetDir = new string(a2);

               
                //MessageBox.Show(String.Format("\"{0}\" has invalid drive", target.FullName), "INVALID SHORTCUT DRIVE");

            }

            // Display the shortcut filename and original target
            lblFile.Text = fileName;
            lblTarget.Text = target.FullName;

            // Set the text box text for changing the shortcut target
            txtNewTargetDir.Text = TargetDir;


            //targetFileName = target.Name;

            //TargetDir = txtNewTargetDir.Text;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // Closes form and returns DialogResult.Cancel 
        }

        private void btnSkip_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtNewTargetDir_TextChanged(object sender, EventArgs e)
        {
            TargetDir = txtNewTargetDir.Text;
        }


        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!FixShortcut)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                // See if the new target is valid ?????????????????????????????????????????????????????????????????
                // If not, don't close the form
                string path = TargetDir + "\\" + TargetFileName;
                if (File.Exists(path))
                {
                    // New shortcut is valid. Close the form
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
        }

        private string FindTarget(FileInfo file)
        {
            string[] levels = Form1.GetDirectoryLevels(file.FullName);

            // The most common problem is an invalid disk drive
            string disk = Path.GetPathRoot(file.FullName);
            // This needs to be fixed
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
