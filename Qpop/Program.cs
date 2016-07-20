using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

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

            //Test Launcher Copy
            SnapshotHelper.Take_Snapshot_Process(cn_process);
            //End Test

            System.Console.ReadLine();
        }
    }
}
