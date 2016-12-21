using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using System.Windows.Forms;
using System.ComponentModel;
using System.Threading;

namespace FileMagic
{
    public partial class Form1
    {

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            int totalFiles = (int) e.Argument;
            int percentComplete, highestPercentageReached = 0;

            while (true)
            {
                if (backgroundWorker1.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }

                percentComplete = (int)((float)fileCount / (float)totalFiles * 100);
                if (percentComplete >= 100)
                    break;

                if (percentComplete > highestPercentageReached)
                {
                    highestPercentageReached = percentComplete;
                    backgroundWorker1.ReportProgress(percentComplete);
                }
                Thread.Sleep(100); // Wait 100 milliseconds;
            }

        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // Change the value of the ProgressBar to the BackgroundWorker progress.
            progressBar.Value = e.ProgressPercentage;
            // Show percentage complete
            lblPct.Text = e.ProgressPercentage.ToString() + "% complete";
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            string text;

            if ((e.Cancelled == true))
            {
                text = "Canceled!";
            }

            else if (!(e.Error == null))
            {
                text = ("Error: " + e.Error.Message);
            }

            else
            {
                text = "Done!";
            }

            //MessageBox.Show("Background Worker " + text);
            //ProgressBar.Value = 0;
        }

        private void Serialize(string FileName)
        {
            try
            {
                //checking if file exists
                if (File.Exists(FileName))
                    File.Delete(FileName);

                using (FileStream fs = new FileStream(FileName, FileMode.Create))
                {
                    IFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(fs, formData);
                    fs.Close();
                }
            }
            catch
            {
            }
        }

        private void Deserialize(string FileName)
        {
            try
            {
                if (File.Exists(FileName))
                {
                    IFormatter formatter = new BinaryFormatter();
                    Stream stream = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                    formData = (FormData)formatter.Deserialize(stream);
                    stream.Close();
                }
            }
            catch
            {
            }
        }

        private bool checkPathErrors()
        {
            string errMsg;

            if (string.IsNullOrEmpty(srcPath))
            {
                errMsg = "Source path not specified";
                txtSrcInput.Focus();
            }
            else if (!Directory.Exists(srcPath))
            {
                errMsg = "Soruce path is not a vaild directory";
                txtSrcInput.Focus();
            }
            else if (string.IsNullOrEmpty(dstPath))
            {
                errMsg = "Destination path not specified";
                txtDstInput.Focus();
            }
            //else if (!Directory.Exists(dstPath))
            //{
            //    errMsg = "Destination path is not a vaild directory";
            //    txtDstInput.Focus();
            //}
            else if (srcPath == dstPath)
            {
                errMsg = "Source path and destination path are the same";
                txtDstInput.Focus();
            }
            else
            {
                return false; // No errors
            }

            MessageBox.Show(errMsg);
            return true;
        }

        private string GetBytesReadable(long i)
        {
            // Get absolute value
            long absolute_i = (i < 0 ? -i : i);
            // Determine the suffix and readable value
            string suffix;
            double readable;
            if (absolute_i >= 0x1000000000000000) // Exabyte
            {
                suffix = "EB";
                readable = (i >> 50);
            }
            else if (absolute_i >= 0x4000000000000) // Petabyte
            {
                suffix = "PB";
                readable = (i >> 40);
            }
            else if (absolute_i >= 0x10000000000) // Terabyte
            {
                suffix = "TB";
                readable = (i >> 30);
            }
            else if (absolute_i >= 0x40000000) // Gigabyte
            {
                suffix = "GB";
                readable = (i >> 20);
            }
            else if (absolute_i >= 0x100000) // Megabyte
            {
                suffix = "MB";
                readable = (i >> 10);
            }
            else if (absolute_i >= 0x400) // Kilobyte
            {
                suffix = "KB";
                readable = i;
            }
            else
            {
                return i.ToString("0 B"); // Byte
            }
            // Divide by 1024 to get fractional value
            readable = (readable / 1024);
            // Return formatted number with suffix
            return readable.ToString("0.### ") + suffix;
        }
    }
}
