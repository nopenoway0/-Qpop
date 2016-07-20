using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Drawing;

namespace Qpop
{
    class Program
    {
        static void Main(string[] args)
        {
            string p_name = "LolClient"; // Set name of program
            // Load Process into a Process_Object
            Console.Write("Searching for Process...\n");
            Process_Object cn_process = Process_Object.GetProcessObject(p_name);
            while(cn_process == null)
            {
                Console.Write("Searching for Process...\n");
                System.Threading.Thread.Sleep(5000);
                cn_process = Process_Object.GetProcessObject(p_name);
            }
            // Print Resulting Process Name and PID (Process ID)
            System.Console.Write(string.Concat("\n", cn_process.GetName(), " ", "PID: ", cn_process.GetPID()));
            // End Load and Print Segment

            //Test Process Close()
            //cn_process.Close();//
            //End Test

            //Diagnostic Window Test
            //SnapshotHelper.Print_Process_Window_Dimensions(cn_process);
            //End Test

            //Test Launcher Copy and Compare
            //Bitmap screen = SnapshotHelper.Take_Snapshot_Process(cn_process, 491, 436, 183, 41);
            //Bitmap refScreen = new Bitmap("reference.PNG");
            //Bitmap queue_img = new Bitmap("join.bmp");
            //Image_Manipulator.Compare_Image_Section(refScreen, queue_img, new Rectangle(491, 436, 183, 41), new Rectangle(0, 0, queue_img.Width, queue_img.Height));
            //End Test

            //Loop for test comparison of live results
            while (true)
            {
                Bitmap screen = SnapshotHelper.Take_Snapshot_Process(cn_process, 491, 436, 183, 41);
                Bitmap queue_img = new Bitmap("join.bmp");
                Console.Write(Image_Manipulator.Compare_Image(screen, queue_img));
                System.Threading.Thread.Sleep(2000);
            }
            //End Test

            System.Console.ReadLine();
        }
    }
}
