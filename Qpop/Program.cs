using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Threading;


//TODO: calculate when the window is minimized
namespace Qpop
{
    class Program
    {
        static int _choice;
        static ManualResetEvent _shutdown = new ManualResetEvent(true);
        //Very Convoluted way of restarting the process of search for processes when the user changes whihc game profile he would like to us. Definitely need to find a more neat approach. Implement shutdown events.
        //TODO: Reimplement once socket is finished
        public static int choice{
            get{ return _choice;
            }

            set{
            _choice = value;
                Console.Clear();
                Program_Selection.main_runner.Abort();
                Program_Selection.main_runner = new Thread(Program.Find_Process);
                Program_Selection.main_runner.Start();
            }
}
        public static void Start(Program_Selection app)
        {
            //Testing importer
            Program_Importer.Import_Programs();
            foreach (Program_Profile p in Program_Importer.programs)
            {
                app.Add_Button(p.Get_DisplayName());
            }
            //app.Add_Button("test");
            app.Invalidate();
            //End Testing

            Find_Process();
        }
        //string p_name = "LolClient"; // Set name of program launcher
        //string m_name = "";        //Set name of main program. Aka the actual game launching passed the launcher.
        // Load Process into a Process_Object. Will load the first process in the data.txt, should be lol
        public static void Find_Process() {
            Console.Write(string.Concat("Searching for Process: ", Program_Importer.programs.ElementAt(choice).Get_Name()));
            Process_Object cn_process = Process_Object.GetProcessObject(Program_Importer.programs.ElementAt(choice).Get_Name());
            while (cn_process == null)
            {
                Console.Write("Searching for Process...\n");
                System.Threading.Thread.Sleep(5000);
                cn_process = Process_Object.GetProcessObject(Program_Importer.programs.ElementAt(choice).Get_Name());
            }
            // Print Resulting Process Name and PID (Process ID)
            System.Console.Write(string.Concat("\n", cn_process.GetName(), " ", "PID: ", cn_process.GetPID()));
            // End Load and Print Segment
            Analyze_Process(cn_process);
        }


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
        //TODO: Add implementation for normal blind pick
        //TODO: Bug starting program before league starts endless Error Loop - workaround repress button
        //Connect to phone
        //TODO: On activation of queue pop, move process window to active, so that click corresponds to the proper progam
        // Server_Socket.StartListening();
        //end connect

        // COMMENTED OUT PROGRAM CODE FOR OTHER TESTS
        public static void Analyze_Process(Process_Object cn_process)
        {
            bool queue_popped = false; //CHANGE TO FALSE OR BROKEN
            bool is_active = true;
            bool main_game_launched = false; //Need to actually start a game to get name of the process
            int counter = 0;
            Bitmap screen = null;
            // Bitmap queue_img = new Bitmap("join.bmp"); // Can Cache image in order to increase perfomance, image is also hardcoded
            while (!queue_popped && is_active)
            {
                is_active = cn_process.IsOpen();
                if (is_active == false)
                {
                    Console.Clear();
                    Console.Write("Process has closed. Closing Program...");
                    break;

                }


                try
                {
                    screen = SnapshotHelper.Take_Snapshot_Process(cn_process, Program_Importer.programs.ElementAt(choice));
                    queue_popped = Image_Manipulator.Compare_Image(screen, Program_Importer.programs.ElementAt(choice).Get_Image());
                }
                catch (Exception e)
                {
                    Console.Write("\nError");
                }
                Console.Write(queue_popped);
                System.Threading.Thread.Sleep(1000);
                if (counter == 100)
                {
                    GC.Collect();
                    counter = 0;
                }
                counter++;
            }

            Process_Object.Click(cn_process, Program_Importer.programs.ElementAt(choice));
            //if (queue_popped == true) Server_Socket.SendSignal("Popped");
            //else Server_Socket.SendSignal("Closed");
            //End Test


            //Socket Tests
            //Server_Socket.StartListening();
            //End Socket Tests

            //Close connection
            //Server_Socket.GoDeaf();
            //End Close


            //Console.Clear();
            Console.Write("Press any key to exit...");
            System.Console.ReadLine();
        }
        }
    }
