using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Drawing;
    

public class SnapshotHelper
{


    public static Bitmap Take_Snapshot_Process(Process_Object proc)
    {
        int is_success = 0;
        WindowsStructs.RECT window_dimensions;
        WindowsStructs.GetWindowRect(proc.GetMWHandle(), out window_dimensions);
        Bitmap scrnsht_wind = new Bitmap(window_dimensions.Right - window_dimensions.Left + 1, window_dimensions.Bottom - window_dimensions.Top + 1);
        Graphics to_shot = Graphics.FromImage(scrnsht_wind);
        IntPtr bitmap_pointer = to_shot.GetHdc();
        is_success = WindowsStructs.PrintWindow(proc.GetMWHandle(), bitmap_pointer, 0);
        if (is_success == 0)
        {
            System.Console.Write("\nError Taking Screenshot\n");
        }
        else System.Console.Write("\nSnapshot Taken\n");
        to_shot.ReleaseHdc(bitmap_pointer);
        to_shot.Dispose();
        /*try
        {
            scrnsht_wind.Save("League.bmp"); //Only included for debugging reasons
        }
        catch(Exception e)
        {
            System.Console.Write("\nError Saving Bitmap\n");
        }*/
        return scrnsht_wind;
        
    }

    //Allows part of the picture to be cropped out according to offsets
    public static Bitmap Take_Snapshot_Process(Process_Object proc, int offset_x, int offset_y, int offset_x2, int offset_y2)
    {
        int is_success = 0;
        Rectangle new_img_bounds = new Rectangle(0, 0, offset_x2, offset_y2);
        Rectangle crop_bounds = new Rectangle(offset_x, offset_y, offset_x2, offset_y2);

        WindowsStructs.RECT window_dimensions;
        WindowsStructs.GetWindowRect(proc.GetMWHandle(), out window_dimensions);
        Bitmap scrnsht_wind = new Bitmap(window_dimensions.Right - window_dimensions.Left + 1, window_dimensions.Bottom - window_dimensions.Top + 1);
        Graphics to_shot = Graphics.FromImage(scrnsht_wind);
        IntPtr bitmap_pointer = to_shot.GetHdc();
        is_success = WindowsStructs.PrintWindow(proc.GetMWHandle(), bitmap_pointer, 0);

        if (is_success == 0)
        {
            System.Console.Write("\nError Taking Screenshot\n");// can return null which can call for a new mainwindowhandler
        }
        else System.Console.Write("\nSnapshot Taken\n");
        Bitmap cropped_pic = new Bitmap(offset_x2, offset_y2);
        to_shot.ReleaseHdc(bitmap_pointer);
        to_shot.Dispose();
        cropped_pic = (Bitmap)scrnsht_wind.Clone(crop_bounds, scrnsht_wind.PixelFormat); 

        scrnsht_wind.Dispose();

       /* try
        {
            cropped_pic.Save("League.bmp"); //Only included for debugging reasons
        }
        catch (Exception e)
        {
            System.Console.Write("\nError Saving Bitmap\n");
        }*/
        return cropped_pic;

    }

    public static Bitmap Take_Snapshot_Process(Process_Object proc, Program_Profile prof)
    {
        return Take_Snapshot_Process(proc, prof.Get_OffsetX(), prof.Get_OffsetY(), prof.Get_LengthX(), prof.Get_LengthY());
    }

    //For Diagnostics
    public static void Print_Process_Window_Dimensions(Process_Object proc)
    {
        WindowsStructs.RECT window_size;
        if(WindowsStructs.GetWindowRect(proc.GetMWHandle(), out window_size) == 0)
        {
            System.Console.Write("\nERROR getting size");
            return;
        }
        System.Console.Write(string.Concat("\nHeight: ", window_size.Bottom - window_size.Top + 1, "  Width: ", window_size.Right - window_size.Left + 1));
    }
}
