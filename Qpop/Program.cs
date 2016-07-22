using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Net.Sockets;

namespace Qpop
{
    class Program
    {
        static void Main(string[] args)
        {

            // COMMENTED OUT PROGRAM CODE FOR OTHER TESTS
            string p_name = "LolClient"; // Set name of program launcher
            string m_name = "";        //Set name of main program. Aka the actual game launching passed the launcher.
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
            //TODO: Add listener for Lol.exe to know when it closes and resume scanning. Assuming user will want to queue for another game
            //TODO: Add listener to LolClient.exe to when the user closes it, shutdown the app. Currently working prototype

            //Connect to phone
            Server_Socket.StartListening();
            //end connect

            // COMMENTED OUT PROGRAM CODE FOR OTHER TESTS
            bool queue_popped = false; //CHANGE TO FALSE OR BROKEN
            bool is_active = true;
            bool main_game_launched = false; //Need to actually start a game to get name of the process
            int counter = 0;
            Bitmap screen = null;
            Bitmap queue_img = new Bitmap("join.bmp"); // Can Cache image in order to increase perfomance, image is also hardcoded
            while (!queue_popped && is_active)
            {
                is_active = cn_process.IsOpen();
                if(is_active == false)
                {
                    Console.Clear();
                    Console.Write("Process has closed. Closing Program...");
                    break;

                }
                try {
                    screen = SnapshotHelper.Take_Snapshot_Process(cn_process, 491, 436, 183, 41);
                    queue_popped = Image_Manipulator.Compare_Image(screen, queue_img);
                }catch(Exception e)
                {
                    Console.Write("\nError");
                }
                Console.Write(queue_popped);
                System.Threading.Thread.Sleep(1000);
                if(counter == 100)
                {
                    GC.Collect();
                    counter = 0;
                }
                counter++;
            }
            if (queue_popped == true) Server_Socket.SendSignal("Queue Popped");
            else Server_Socket.SendSignal("Terminated before queue popped");
            //End Test


            //Socket Tests
            //Server_Socket.StartListening();
            //End Socket Tests
            
            //Close connection
            Server_Socket.GoDeaf();
            //End Close

            Console.Clear();
            Console.Write("Press any key to exit...");
            System.Console.ReadLine();
        }
    }
}
