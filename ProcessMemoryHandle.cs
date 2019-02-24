using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace XAYRGA.HeapGlider
{
    class ProcessMemoryHandle
    {
        int permissions = (int)ProcessVirtualMachinePermissions.PROCESS_VM_OPERATION ^
          (int)ProcessVirtualMachinePermissions.PROCESS_VM_WRITE ^
          (int)ProcessVirtualMachinePermissions.PROCESS_WM_READ;

      
        public Process BaseProcess;
        private IntPtr Win32ProcessHandle;

        public ProcessMemoryHandle(Process PData)
        {
            if (PData.HasExited)
            {
                throw new Exception("Initializing ProcessMemoryHandle with an exited process.");
            }

            BaseProcess = PData;
            Win32ProcessHandle = IWindowsProcessMemory.OpenProcess(permissions, false, PData.Id);
 
        }

        public bool ProcessRunning()
        {
      
            return !BaseProcess.HasExited;
        }


        public IntPtr ProcessHandle
        {
            get
            {
                if (!BaseProcess.HasExited)
                {
                    return Win32ProcessHandle;
                }
                return IntPtr.Zero;
            }
            private set
            {
                
                return;
            }
        }


        public byte[] ReadMemory(int offset, int size,out int NumRead,out bool error)
        {
            if (!ProcessRunning())
            {
                NumRead = 0;
                error = true;
                return new byte[0];
            }

    
            byte[] retmem = new byte[size];
            int bytes_read = 0;
            error = IWindowsProcessMemory.ReadProcessMemory((int)Win32ProcessHandle, offset, retmem, size, ref bytes_read); /// aaaand somehow without REF this gets stuffed into retmem?
            NumRead = bytes_read;

            return retmem;

        }

        public int WriteMemory(byte[] data,int offset, int size, out bool error)
        {
            if (!ProcessRunning())
            {
                
                error = true;
                return 0;

            }


            int bytes_written = 0; 

            error = IWindowsProcessMemory.WriteProcessMemory((int)Win32ProcessHandle, offset, data , size, ref bytes_written); 


            return bytes_written;

        }



    }
}
