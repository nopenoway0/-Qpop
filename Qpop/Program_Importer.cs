using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Qpop
{
    class Program_Importer
    {
        string[] program_list;
        string file;
        public static void Import_Programs()
        {
            //TODO: Add support for comments in text files
            Console.WriteLine("Importing programs");
            FileStream read = File.OpenRead("data.txt");
            string contentsS;
            byte[] contents = new byte[read.Length];
            byte temp;
            bool detected_token = false;
            int x = 0;
            while (x < read.Length)
            {
                temp = (byte)read.ReadByte();
                if(temp != '"' && detected_token == false) {
                    contents[x] = temp;
                    x++;
                }
                else if(temp == '"' && detected_token == true)
                {
                    detected_token = false;
                }
                else if(temp == '"' && detected_token == false)
                {
                    detected_token = true;
                }
            }
            read.Close();
            read.Dispose();
            contentsS = ASCIIEncoding.ASCII.GetString(contents);
            Console.WriteLine(contentsS);
        }
    }
}
