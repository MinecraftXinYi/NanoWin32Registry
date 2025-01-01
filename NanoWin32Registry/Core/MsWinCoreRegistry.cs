using System;
using System.Runtime.InteropServices;

namespace NanoWin32Registry.Core;

public static class MsWinCoreRegistry
{
    private const string NativeRegApiDllL1 = "downlevel\\api-ms-win-core-registry-l1-1-0.dll";

    [DllImport(NativeRegApiDllL1, CharSet = CharSet.Unicode, SetLastError = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern int RegCreateKeyExW(
        IntPtr hKey,
        string lpSubKey,
        int Reserved,
        string? lpClass,
        int dwOptions,
        int samDesired,
        IntPtr lpSecurityAttributes,
        out IntPtr phkResult,
        out int lpdwDisposition);

    [DllImport(NativeRegApiDllL1, CharSet = CharSet.Unicode, SetLastError = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern int RegOpenKeyExW(
        IntPtr hKey,
        string lpSubKey,
        int ulOptions,
        int samDesired,
        out IntPtr phkResult);

    [DllImport(NativeRegApiDllL1, CharSet = CharSet.Unicode, SetLastError = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern int RegCloseKey(IntPtr hKey);

    [DllImport(NativeRegApiDllL1, CharSet = CharSet.Unicode, SetLastError = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern int RegGetValueW(
        IntPtr hKey,
        string? lpSubKey,
        string lpValue,
        int dwFlags,
        out int pdwType,
        IntPtr pvData,
        ref int pcbData);

    [DllImport(NativeRegApiDllL1, CharSet = CharSet.Unicode, SetLastError = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern int RegSetValueExW(
        IntPtr hKey,
        string lpValueName,
        int Reserved,
        int dwType,
        byte[] lpData,
        int cbData);

    [DllImport(NativeRegApiDllL1, CharSet = CharSet.Unicode, SetLastError = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern int RegDeleteKeyW(IntPtr hKey, string lpSubKey);

    [DllImport(NativeRegApiDllL1, CharSet = CharSet.Unicode, SetLastError = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern int RegDeleteValueW(IntPtr hKey, string lpValueName);
}
