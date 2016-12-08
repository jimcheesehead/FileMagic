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
    public partial class Form1 : Form
    {
        [Serializable]
        class FormData
        {
            public BindingList<string> srcInputList = new BindingList<string>();
            public BindingList<string> dstInputList = new BindingList<string>();
        }

        FormData formData = new FormData();

        string srcPath, dstPath;
        string errMsg;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
    }
}
