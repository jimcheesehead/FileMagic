using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;


namespace FileMagic
{
    partial class Form1
    {
        void Serialize(string FileName)
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

        void Deserialize(string FileName)
        {
            try
            {
                if (File.Exists(FileName))
                {
                    IFormatter formatter = new BinaryFormatter();
                    Stream stream = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                    formData = (FormData)formatter.Deserialize(stream);
                    stream.Close();

                    //Bind the comboboxes data source
                    txtSrcInput.DataSource = formData.srcInputList;
                    txtDstInput.DataSource = formData.dstInputList;
                }
            }
            catch
            {
            }
        }
    }
}
