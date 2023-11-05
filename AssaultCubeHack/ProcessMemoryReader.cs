using System.Diagnostics;
using System.Runtime.InteropServices;

// ORIGINALLY CREATED BY Pingo, Edited by Fleep
// NOT created by Alex Lee
namespace ProcessMemoryReaderLib
{
    // Class needed to read, write to specific memory locations
    class ProcessMemoryReaderApi
    {
        public const uint PROCESS_VM_READ = (0x0010);
        public const uint PROCESS_VM_WRITE = (0x0020);
        public const uint PROCESS_VM_OPERATION = (0x0008);
        public const uint PAGE_READWRITE = 0x0004;
        public const int WM_SYSCOMMAND = 0x0112;
        public const int WM_ACTIVATE = 0x6;
        public const int WM_HOTKEY = 0x0312;
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(long dwFlags, long dx, long dy, long cButtons, long dwExtraInfo);
        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(UInt32 dwDesiredAccess, Int32 bInheritHandle, UInt32 dwProcessId);

        [DllImport("kernel32.dll")]
        public static extern Int32 CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll")]
        public static extern Int32 ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [In, Out] byte[] buffer, UInt32 size, out IntPtr lpNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        public static extern Int32 WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [In, Out] byte[] buffer, UInt32 size, out IntPtr lpNumberOfBytesWritten);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool VirtualProtectEx(IntPtr hProcess, IntPtr lpAddress, UInt32 dwSize, uint flNewProtect, out uint lpflOldProtect);

        [DllImport("user32.dll")]
        public static extern int GetKeyState(int vKey);

    }
    public class ProcessMemoryReader
    {
        public ProcessMemoryReader()
        {
        }
        public Process ReadProcess
        {
            get
            {
                return m_ReadProcess;
            }
            set
            {
                m_ReadProcess = value;
            }
        }
        private Process m_ReadProcess = null;
        private IntPtr m_hProcess = IntPtr.Zero;
        public void OpenProcess()
        {
            // Literally opens the process, in this case it will be ac_client (AssaultCube main client)
            m_hProcess = ProcessMemoryReaderApi.OpenProcess(ProcessMemoryReaderApi.PROCESS_VM_READ | ProcessMemoryReaderApi.PROCESS_VM_WRITE | ProcessMemoryReaderApi.PROCESS_VM_OPERATION, 1, (uint)m_ReadProcess.Id);
        }

        #region ReadMem
        public int ReadMem(int MemoryAddress, uint bytesToRead, out byte[] buffer)
        {
            // Reads the memory from the given address, very straightforward
            IntPtr procHandle = ProcessMemoryReaderApi.OpenProcess(ProcessMemoryReaderApi.PROCESS_VM_READ | ProcessMemoryReaderApi.PROCESS_VM_WRITE | ProcessMemoryReaderApi.PROCESS_VM_OPERATION, 1, (uint)m_ReadProcess.Id);
            if (procHandle == IntPtr.Zero)
            {
                buffer = new byte[0];
                return 0;
            }

            buffer = new byte[bytesToRead];
            IntPtr ptrBytesReaded;
            ProcessMemoryReaderApi.ReadProcessMemory(procHandle, (IntPtr)MemoryAddress, buffer, bytesToRead, out ptrBytesReaded);
            ProcessMemoryReaderApi.CloseHandle(procHandle);
            return ptrBytesReaded.ToInt32();
        }

        public int ReadMultiLevelPointer(int MemoryAddress, uint bytesToRead, Int32[] offsetList)
        {
            // Reads a 'multi-level pointer' which in short, is a pointer with multiple offsets
            // Literally is a pointer to a pointer, or could be a pointer to a pointer to a pointer
            IntPtr procHandle = ProcessMemoryReaderApi.OpenProcess(ProcessMemoryReaderApi.PROCESS_VM_READ | ProcessMemoryReaderApi.PROCESS_VM_WRITE | ProcessMemoryReaderApi.PROCESS_VM_OPERATION, 1, (uint)m_ReadProcess.Id);
            IntPtr pointer = (IntPtr)0x0;
            // If the process isn't available we return nothing
            if (procHandle == IntPtr.Zero)
            {
                return 0;
            }

            byte[] btBuffer = new byte[bytesToRead];
            IntPtr lpOutStorage = IntPtr.Zero;

            int pointerAddy = MemoryAddress;
            for (int i = 0; i < (offsetList.Length); i++)
            {
                if (i == 0)
                {
                    ProcessMemoryReaderApi.ReadProcessMemory(
                        procHandle,
                        (IntPtr)(pointerAddy),
                        btBuffer,
                        (uint)btBuffer.Length,
                        out lpOutStorage);
                }
                pointerAddy = (BitConverter.ToInt32(btBuffer, 0) + offsetList[i]);

                ProcessMemoryReaderApi.ReadProcessMemory(
                    procHandle,
                    (IntPtr)(pointerAddy),
                    btBuffer,
                    (uint)btBuffer.Length,
                    out lpOutStorage);
            }
            return pointerAddy;
        }
        public int ReadInt(int MemoryAddress)
        {
            // Reads int values from memory
            byte[] buffer;
            int read = ReadMem(MemoryAddress, 4, out buffer);
            if (read == 0)
                return 0;
            else
                return BitConverter.ToInt32(buffer, 0);
        }
        public float ReadFloat(int MemoryAddress)
        {
            // Reads float values from memory
            byte[] buffer;
            int read = ReadMem(MemoryAddress, 4, out buffer);
            if (read == 0)
                return 0;
            else
                return BitConverter.ToSingle(buffer, 0);
        }
        #endregion
        #region WriteMem
        public int WriteMem(int MemoryAddress, byte[] buf)
        {
            // Writes to memory
            IntPtr procHandle = ProcessMemoryReaderApi.OpenProcess(ProcessMemoryReaderApi.PROCESS_VM_READ | ProcessMemoryReaderApi.PROCESS_VM_WRITE | ProcessMemoryReaderApi.PROCESS_VM_OPERATION, 1, (uint)m_ReadProcess.Id);
            if (procHandle == IntPtr.Zero)
                return 0;

            uint oldProtect;
            ProcessMemoryReaderApi.VirtualProtectEx(procHandle, (IntPtr)MemoryAddress, (uint)buf.Length, ProcessMemoryReaderApi.PAGE_READWRITE, out oldProtect);
            IntPtr ptrBytesWritten;
            ProcessMemoryReaderApi.WriteProcessMemory(procHandle, (IntPtr)MemoryAddress, buf, (uint)buf.Length, out ptrBytesWritten);
            ProcessMemoryReaderApi.CloseHandle(procHandle);
            return ptrBytesWritten.ToInt32();
        }
        public void WriteInt(int MemoryAddress, int w)
        {
            // Writes int values to given memory address
            byte[] buf = BitConverter.GetBytes(w);
            WriteMem(MemoryAddress, buf);
        }
        public void WriteFloat(int MemoryAddress, float f)
        {
            // Writes float values to given memory address
            byte[] buf = BitConverter.GetBytes(f);
            WriteMem(MemoryAddress, buf);
        }
        #endregion
        #region Keys
        [DllImport("user32.dll")]
        public static extern short GetKeyState(Keys nVirtKey);
        // Checks if the given key has been pressed, in this case we check the right mouse
        // has been clicked or not
        public enum VirtualKeyStates : int
        {
            VK_LBUTTON = 0x01,
            VK_RBUTTON = 0x02,
        }
        public bool Keystate(Keys key)
        {
            int state = GetKeyState(key);
            if (state == -127 || state == -128)
            {
                return true;
            }
            return false;
        }
        #endregion
    }
}