using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.InteropServices;




namespace XAYRGA.HeapGlider
{
    public enum ProcessVirtualMachinePermissions
    {
        PROCESS_WM_READ = 0x0010,
        PROCESS_VM_WRITE = 0x0020,
        PROCESS_VM_OPERATION = 0x0008,
    }

    public static class IWindowsProcessMemory
    {
        // Memorybuffer by http://www.codeproject.com/Articles/670373/Csharp-Read-Write-another-Process-Memory
     
            [DllImport("kernel32.dll")]
            public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);
            [DllImport("kernel32.dll")]
            public static extern bool ReadProcessMemory(int hProcess,
              int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);
            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern bool WriteProcessMemory(int hProcess, int lpBaseAddress,
              byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesWritten);
        

    }
}
