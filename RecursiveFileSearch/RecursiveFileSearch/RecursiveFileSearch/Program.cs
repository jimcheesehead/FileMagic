using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecursiveFileSearch
{
    class Program
    {
        static void Main(string[] args)
        {
            args[0] = @"M:\stuff\Femjoy3\2014\01";
            args[1] = @"LAUREN_PatrikRyan_MoveItLikeThat_large.zip";

            args[0] = @"M:\stuff\";
   //         args[1] = @"*.zip";

            if (args.Count() != 2)
            {
                Console.WriteLine("USAGE: <directory> <filename> ");
            }
            else
            {
                GetAllFiles(args[0], args[1]);
                Console.WriteLine("DONE!!");
            }
            Console.ReadLine(); // So we can see output
        }

        private static void GetAllFiles(string sDir, string sFile) {
            foreach (string dir in Directory.GetDirectories(sDir))
            {
                try
                {
                    foreach (string  file in Directory.GetFiles(dir, sFile))
                    {
                        //string fileName = Path.GetFileName(file);
                        //Console.WriteLine(fileName);

                        Console.WriteLine(file);
                    }
                    // Recursive Search
                    GetAllFiles(dir, sFile);
                }

                catch (Exception Error)
                {
                    Console.WriteLine(Error.Message);
                }
            }
        }
    }
}
