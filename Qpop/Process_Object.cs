using System;
using System.Runtime.InteropServices;
using System.Diagnostics;

public class Process_Object
{

    public static void Click(Process_Object procObj, Program_Profile progProf)
    {
        //Adapted only for 64 bit systems?
        //TODO: Make alternate code for 32bit systems
        WindowsStructs.SetForegroundWindow(procObj.GetMWHandle());
        UInt64 clickPoint = 0x0;
        clickPoint = clickPoint | (UInt32) (progProf.Get_OffsetY() + 5);
        clickPoint = clickPoint << 0x10;
        clickPoint = clickPoint | (UInt32)(progProf.Get_OffsetX() + 5);
        WindowsStructs.SendMessage(procObj.GetMWHandle(), WindowsStructs.WM_LBUTTONDOWN, (IntPtr) WindowsStructs.MK_LBUTTON, (IntPtr) clickPoint);
        WindowsStructs.SendMessage(procObj.GetMWHandle(), WindowsStructs.WM_LBUTTONUP,(IntPtr) WindowsStructs.MK_LBUTTON, (IntPtr)clickPoint);
        //WindowsStructs.SendMessage(procObj.GetMWHandle(), WindowsStructs.WM_LBUTTONDOWN, WindowsStructs.MK_LBUTTON, (UInt32)((progProf.Get_LengthY() << 0x10) | progProf.Get_LengthX()));
        //WindowsStructs.SendMessage(procObj.GetMWHandle(), WindowsStructs.WM_LBUTTONUP, WindowsStructs.MK_LBUTTON, (UInt32)((progProf.Get_LengthY() << 0x10) | progProf.Get_LengthX()));
    }

    private string name;
    private int pid;
    private Process act_process;
    private IntPtr handle;
    private IntPtr mw_handle;
	public Process_Object()
	{
        name = null;
        pid = 0;
	}

    public void SetName(string name)
    {
        this.name = name;
    }

    public void SetPID(int pid)
    {
        this.pid = pid;
    }

    private void SetHandle(IntPtr h_in)
    {
        this.handle = h_in;
    }

    public void SetProcess(Process p_in)
    {
        this.act_process = p_in;
    }

    public string GetName()
    {
        return this.name;
    }

    public int GetPID()
    {
        return pid;
    }

    public IntPtr GetHandle()
    {
        return this.handle;
    }

    private void SetMWHandle(IntPtr hndle)
    {
        this.mw_handle = hndle;
    }

    public IntPtr GetMWHandle()
    {
        return this.mw_handle;
    }

    /**Retrieves a Process_Object to be stored. Stores name, PID, handle to process, and the actual Process object
    */
    public static Process_Object GetProcessObject(string name)
    {
        Process_Object pb1 = new Process_Object();
        Process[] tmp = Process.GetProcessesByName(name);
       // System.Console.Write(tmp.Length);
        if(tmp.Length == 0)
        {
            System.Console.Write("\nCouldn't find process\n");
            return null;
        }
        else if(tmp.Length == 1)
        {
            System.Console.Write("Found Exact Process");
            pb1.SetProcess(tmp[0]);
            pb1.SetName(tmp[0].ProcessName);
            pb1.SetPID(tmp[0].Id);
            pb1.SetMWHandle(tmp[0].MainWindowHandle);
            //Get Handle after opening process, currently only containing Terminate_Process tag
            IntPtr tmp_hndle = WindowsStructs.OpenProcess((IntPtr)0x1F0FFF, false, (IntPtr)tmp[0].Id);
            if(tmp_hndle == null)
            {
                System.Console.Write("Could not open process");
                pb1.SetHandle(tmp[0].Handle);
            }
            else pb1.SetHandle(tmp_hndle);
            //End getting of handle
        }
        else
        {
            System.Console.Write("Multiple Processes Found -- returning null");
            return null;
        }
        return pb1;
    }
    public bool Close()
    {
        uint exit_code = 0;
        int is_success = 0;
        is_success = WindowsStructs.TerminateProcess(this.handle, exit_code);
        if (is_success == 0) return false;
        else return true;
    }
    public bool IsOpen()
    {
        IntPtr ex_code;
        int value;
        unsafe
        {
            int success = WindowsStructs.GetExitCodeProcess(this.GetHandle(), out ex_code);
            if (success == 0) return false;
            value = ex_code.ToInt32();
        }
        if (value == 259)
        {
            return true;
        }
        else {
            return false;
        }
    }
}
