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

    //Code borrowed from pinvoke
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int Left;        // x position of upper-left corner
        public int Top;         // y position of upper-left corner
        public int Right;       // x position of lower-right corner
        public int Bottom;      // y position of lower-right corner
    }
    // End borrowed

    public static Bitmap Take_Snapshot_Process(Process_Object proc)
    {
        int is_success = 0;
        RECT window_dimensions;
        GetWindowRect(proc.GetMWHandle(), out window_dimensions);
        Bitmap scrnsht_wind = new Bitmap(window_dimensions.Right - window_dimensions.Left + 1, window_dimensions.Bottom - window_dimensions.Top + 1);
        Graphics to_shot = Graphics.FromImage(scrnsht_wind);
        IntPtr bitmap_pointer = to_shot.GetHdc();
        is_success = PrintWindow(proc.GetMWHandle(), bitmap_pointer, 0);
        if (is_success == 0)
        {
            System.Console.Write("\nError Taking Screenshot\n");
        }
        else System.Console.Write("\nSnapshot Taken\n");
        to_shot.ReleaseHdc(bitmap_pointer);
        to_shot.Dispose();
        try
        {
            scrnsht_wind.Save("League.bmp"); //Only included for debugging reasons
        }
        catch(Exception e)
        {
            System.Console.Write("\nError Saving Bitmap\n");
        }
        return scrnsht_wind;
        
    }


    //For Diagnostics
    public static void Print_Process_Window_Dimensions(Process_Object proc)
    {
        RECT window_size;
        if(GetWindowRect(proc.GetMWHandle(), out window_size) == 0)
        {
            System.Console.Write("\nERROR getting size");
            return;
        }
        System.Console.Write(string.Concat("\nHeight: ", window_size.Bottom - window_size.Top + 1, "  Width: ", window_size.Right - window_size.Left + 1));
    }
}
