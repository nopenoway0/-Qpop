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

            //TODO: Change to JSON, style loading
            contentsS = ASCIIEncoding.ASCII.GetString(contents);
            //contentsS = contentsS.Replace(" ", "");
            contentsS = contentsS.Replace("\n", "");
            sections = contentsS.Split(new char[2] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            x = 0;
            while (x < sections.Length)
            {
                sections[x + 4] = sections[x + 4].Replace(" ", "");
                dimensions = Image_Manipulator.Get_Image_Dimensions(sections[x + 4]);
                Program_Profile profile = new Program_Profile(Int32.Parse(sections[x + 2]), Int32.Parse(sections[x + 3]), dimensions[0], dimensions[1], sections[x + 4]);
                profile.Set_Name(sections[x + 1].Replace("\r", "").Replace("\n", "").Replace(" ", "")); //\r causing problems?
                profile.Set_DisplayName(sections[x].Replace("\r", ""));
                if (profile != null) programs.Add(profile);
                x += 6;
            }
        }
    }
}
