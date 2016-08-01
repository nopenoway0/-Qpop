using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;

namespace Qpop
{
    class Program_Importer
    {
        public static List<Program_Profile> programs = new List<Program_Profile>();
        public static void Import_Programs()
        {
            Console.WriteLine("Importing programs");
            FileStream read = File.OpenRead("data.txt");
            string contentsS;
            int[] dimensions;
            byte[] contents = new byte[read.Length];
            byte temp;
            bool detected_token = false;
            int x = 0; int EOF;
            string[] sections;
            while (x < read.Length)
            {
                EOF = read.ReadByte();
                temp = (byte)EOF;
                if (EOF == -1) break;
                else if (temp != '"' && detected_token == false)
                {
                    contents[x] = temp;
                    x++;
                }
                else if (temp == '"' && detected_token == true)
                {
                    detected_token = false;
                }
                else if (temp == '"' && detected_token == false)
                {
                    detected_token = true;
                }
            }
            read.Close();
            read.Dispose();

            //TODO: Allow for more than 1 profile, change style of loading
            contentsS = ASCIIEncoding.ASCII.GetString(contents);
            contentsS = contentsS.Replace(" ", "");
            sections = contentsS.Split('\n');
            dimensions = Image_Manipulator.Get_Image_Dimensions(sections[3]);
            Program_Profile profile = new Program_Profile(Int32.Parse(sections[1]), Int32.Parse(sections[2]), dimensions[0], dimensions[1], sections[3]);
            profile.Set_Name(sections[0].Replace("\r", "")); //\r causing problems?
            if (profile != null) programs.Add(profile);
        }
    }
}
