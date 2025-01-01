using System;
using System.Runtime.InteropServices;

namespace NanoWin32Registry.Core;

public static class WinMemory
{
    [DllImport("KernelBase.dll", SetLastError = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern IntPtr LocalAlloc(uint uFlags, UIntPtr sizetdwBytes);

    [DllImport("KernelBase.dll", SetLastError = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern IntPtr LocalFree(IntPtr handle);

    public static IntPtr FastLocalAlloc(IntPtr cb, bool win32 = true)
    {
        UIntPtr numBytes;
        if (win32) numBytes = (UIntPtr)unchecked((uint)cb.ToInt32());
        else numBytes = (UIntPtr)unchecked((uint)cb.ToInt64());
        IntPtr pNewMem = LocalAlloc(0, unchecked(numBytes));
        if (pNewMem == IntPtr.Zero)
            Marshal.ThrowExceptionForHR(Marshal.GetLastWin32Error());
        return pNewMem;
    }

    public static IntPtr FastLocalAlloc(int cb, bool win32 = true)
        => FastLocalAlloc((IntPtr)cb, win32);

    public static void FastLocalFree(IntPtr handle)
    {
        if (LocalFree(handle) != IntPtr.Zero)
            Marshal.ThrowExceptionForHR(Marshal.GetLastWin32Error());
    }
}
