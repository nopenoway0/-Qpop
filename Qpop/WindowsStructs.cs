using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

    class WindowsStructs
    {
    //Consts
    public const UInt32 FORMAT_MESSAGE_ALLOCATE_BUFFER = 0x00000100;
    public const UInt32 FORMAT_MESSAGE_ARGUMENT_ARRAY = 0x00002000;
    public const UInt32 FORMAT_MESSAGE_FROM_HMODULE = 0x00000800;
    public const UInt32 FORMAT_MESSAGE_FROM_SYSTEM = 0x00001000;
    public const UInt32 WM_LBUTTONDOWN = 0x0201;
    public const UInt32 WM_LBUTTONUP = 0x0202;
    public const UInt32 MK_LBUTTON = 0x0001;
    public const UInt32 FORMAT_MESSAGE_IGNORE_INSERTS = 0x00000200;
    public const UInt32 LANG_NEUT = 0x0C00;
    //Functions
    [DllImport("Kernel32.dll")]
    public static extern IntPtr OpenProcess(IntPtr dwDesiredAccess, bool bInheritHandle, IntPtr dwProcessId);
    [DllImport("Kernel32.dll")]
    public static extern int TerminateProcess(IntPtr hProcess, uint uExitCode);
    [DllImport("Kernel32.dll")]
    public static extern int GetExitCodeProcess(IntPtr hProcess, out IntPtr lpExitCode);
    [DllImport("user32.dll")]
    public static extern int AdjustWindowRectEx(WindowsStructs.RECT lpRect, double dwStyle, bool bMenu, double dwExStyle);
    [DllImport("User32.dll")]
    public static extern int SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
    [DllImport("User32.dll")]
    public static extern int SetForegroundWindow(IntPtr hWnd);
    [DllImport("User32.dll")]
    public static extern int PrintWindow(IntPtr hwnd, IntPtr hdcBlt, uint nFlags);
    [DllImport("user32.dll")]
    public static extern int GetWindowRect(IntPtr hWnd, out RECT lpRect);
    [DllImport("Kernel32.dll")]
    public extern static UInt32 GetLastError();
    //http://www.pinvoke.net/default.aspx/kernel32.formatmessage
    [DllImport("Kernel32.dll")]
    public extern static int FormatMessage(UInt32 dwFlags, ref IntPtr lpSource, UInt32 dwMessageId, UInt32 dwLanguageId, out IntPtr lpBuffer, UInt32 nSize, IntPtr Arguments);
    [DllImport("User32.dll")]
    public extern static int SendInput(UInt32 nInputs, IntPtr pInputs, int cbSize);
    [DllImport("User32.dll")]
    public extern static int PostMessage(IntPtr hWnd, uint Msg, UInt32 wParam, UInt32 lParam);
    
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

    public struct INPUT
    {
        public UInt32 type;
        public MOUSEINPUT mi;
    }
        public struct MOUSEINPUT
    {
        public long dx;
        public long dy;
        public UInt32 mouseData;
        public UInt32 dwFlags;
        public UInt32 time;
        public UIntPtr dwExtraInfo;
    }

        public struct KEYBINPUT
    {
        public UInt16 wVk;
        public UInt16 Scan;
        public UInt32 dwFlags;
        public UInt32 time;
        UIntPtr dwExtraInfo;
    }

        public struct HARDWAREINPUT
    {
        UInt32 uMsg;
        UInt16 wParamL;
        UInt16 wParamH;
    }

    public static void PrintLastError()
    {
        UInt32 message = GetLastError();
        IntPtr buffer;
        int numChars;
        IntPtr source = IntPtr.Zero;
        IntPtr arg = IntPtr.Zero;
        numChars = FormatMessage(FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_IGNORE_INSERTS, ref source, message, 0, out buffer, 0, arg);
        if (numChars == 0) Console.WriteLine("COULDN'T FORMAT MESSAGE");
        else
        {

            String convMessage = Marshal.PtrToStringAnsi(buffer);
            Marshal.FreeHGlobal(buffer);
            Marshal.FreeHGlobal(source);
            Marshal.FreeHGlobal(arg);
            Console.WriteLine("ERROR: " + convMessage);
        }
        //Console.WriteLine(message);
    }
    }
     