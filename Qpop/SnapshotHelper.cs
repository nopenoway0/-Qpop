using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Drawing;
    

public class SnapshotHelper
{
    [DllImport("User32.dll")]
    static extern int PrintWindow(IntPtr hwnd, IntPtr hdcBlt, uint nFlags);
    [DllImport("user32.dll")]
    static extern int GetWindowRect(IntPtr hWnd, out RECT lpRect);

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int Left;        // x position of upper-left corner
        public int Top;         // y position of upper-left corner
        public int Right;       // x position of lower-right corner
        public int Bottom;      // y position of lower-right corner
    }

    public static bool Take_Snapshot_Process(Process_Object proc)
    {
        RECT window_dimensions;
        GetWindowRect(proc.GetHandle(), out window_dimensions);
        //Bitmap scrnsht_wind = new Bitmap(window_dimensions.Left, window_dimensions.Height);
        //Graphics to_shot = Graphics.FromImage(scrnsht_wind);

        return true;

       /* int is_success = PrintWindow(proc.GetHandle(), scrnsht_wind, 0); // Doesn't Work. Need device ptr
        if (is_success == 0)
        {
            return false;
        }
        else return true;*/
    }

    //For Diagnostics
    public static void Print_Process_Window_Dimensions(Process_Object proc)
    {
        RECT window_size;
        if(GetWindowRect(proc.GetHandle(), out window_size) == 0)
        {
            System.Console.Write("\nERROR getting size");
            return;
        }
        System.Console.Write(window_size.ToString());
        System.Console.Write(string.Concat("\nHeight: ", window_size.Bottom - window_size.Top + 1, "  Width: ", window_size.Right - window_size.Left + 1));
    }
}
