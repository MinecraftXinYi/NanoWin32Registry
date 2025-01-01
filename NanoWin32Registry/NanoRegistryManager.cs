using System;
using System.Text;
using System.Runtime.InteropServices;

namespace NanoWin32Registry;

using Constants;
using Core;

/// <summary>
/// 注册表操作
/// Registry operation
/// </summary>
public static partial class NanoRegistryManager
{
    private static void MarshalAssert(int apiResult)
    {
        if (apiResult != 0)
            throw new InvalidOperationException(apiResult.ToString());
    }

    /// <summary>
    /// 获取注册表根键
    /// Get registry root key
    /// </summary>
    /// <param name="fullKeyPath">完整注册表路径</param>
    /// <param name="subKeyPath">注册表子项路径</param>
    /// <returns>路径包含的注册表根键</returns>
    /// <exception cref="ArgumentException"></exception>
    public static IntPtr GetRegistryRootKeyFromFullPath(string fullKeyPath, out string subKeyPath)
    {
        string root = fullKeyPath.Substring(0, fullKeyPath.IndexOf('\\'));
        IntPtr rootKey = IntPtr.Zero;
        switch (root.ToUpper())
        {
            case "HKEY_CLASSES_ROOT":
                rootKey = RegistryHive.HKEY_CLASSES_ROOT;
                break;
            case "HKEY_CURRENT_USER":
                rootKey = RegistryHive.HKEY_CURRENT_USER;
                break;
            case "HKEY_LOCAL_MACHINE":
                rootKey = RegistryHive.HKEY_LOCAL_MACHINE;
                break;
            case "HKEY_USERS":
                rootKey = RegistryHive.HKEY_USERS;
                break;
            case "HKEY_CURRENT_CONFIG":
                rootKey = RegistryHive.HKEY_CURRENT_CONFIG;
                break;
            default:
                throw new ArgumentException("This registry path includes an invalid registry root.", "fullKeyPath", null);
        }
        subKeyPath = fullKeyPath.Substring(fullKeyPath.IndexOf('\\') + 1);
        return rootKey;
    }

    /// <summary>
    /// 创建注册表键
    /// Create registry key
    /// </summary>
    /// <param name="path">完整注册表路径</param>
    /// <exception cref="Exception"></exception>
    public static void CreateKey(string path)
    {
        IntPtr hKey = GetRegistryRootKeyFromFullPath(path, out string subKey);
        int result = MsWinCoreRegistry.RegCreateKeyExW(hKey, subKey, 0, null, RegOptions.REG_OPTION_NON_VOLATILE, KeyAccess.KEY_WRITE, IntPtr.Zero, out IntPtr phkResult, out _);
        MarshalAssert(result);
        MsWinCoreRegistry.RegCloseKey(phkResult);
    }

    /// <summary>
    /// 删除注册表键
    /// Delete registry key
    /// </summary>
    /// <param name="path">完整注册表路径</param>
    /// <exception cref="Exception"></exception>
    public static void DeleteKey(string path)
    {
        IntPtr hKey = GetRegistryRootKeyFromFullPath(path, out string subKey);
        int result = MsWinCoreRegistry.RegDeleteKeyW(hKey, subKey);
        MarshalAssert(result);
    }

    /// <summary>
    /// 设置注册表 String 类型值
    /// Set registry string value
    /// </summary>
    /// <param name="path">完整注册表路径</param>
    /// <param name="valueName">要设置的项值名称</param>
    /// <param name="value">要设置的值</param>
    /// <exception cref="Exception"></exception>
    public static void SetStringValue(string path, string valueName, string value)
    {
        IntPtr hKey = GetRegistryRootKeyFromFullPath(path, out string subKey);
        int result = MsWinCoreRegistry.RegOpenKeyExW(hKey, subKey, 0, KeyAccess.KEY_WRITE, out IntPtr phkResult);
        MarshalAssert(result);
        byte[] data = Encoding.Unicode.GetBytes(value);
        result = MsWinCoreRegistry.RegSetValueExW(phkResult, valueName, 0, KeyType.REG_SZ, data, data.Length);
        MsWinCoreRegistry.RegCloseKey(phkResult);
        MarshalAssert(result);
    }

    /// <summary>
    /// 设置注册表 Unsigned Int32 类型值
    /// Set registry unsigned int32 value
    /// </summary>
    /// <param name="path">完整注册表路径</param>
    /// <param name="valueName">要设置的项值名称</param>
    /// <param name="value">要设置的值</param>
    /// <exception cref="Exception"></exception>
    public static void SetDwordValue(string path, string valueName, uint value)
    {
        IntPtr hKey = GetRegistryRootKeyFromFullPath(path, out string subKey);
        int result = MsWinCoreRegistry.RegOpenKeyExW(hKey, subKey, 0, KeyAccess.KEY_WRITE, out IntPtr phkResult);
        MarshalAssert(result);
        byte[] data = BitConverter.GetBytes(value);
        result = MsWinCoreRegistry.RegSetValueExW(phkResult, valueName, 0, KeyType.REG_DWORD, data, data.Length);
        MsWinCoreRegistry.RegCloseKey(phkResult);
        MarshalAssert(result);
    }

    /// <summary>
    /// 设置注册表 Unsigned Int64 类型值
    /// Set registry unsigned int64 value
    /// </summary>
    /// <param name="path">完整注册表路径</param>
    /// <param name="valueName">要设置的项值名称</param>
    /// <param name="value">要设置的值</param>
    /// <exception cref="Exception"></exception>
    public static void SetQwordValue(string path, string valueName, ulong value)
    {
        IntPtr hKey = GetRegistryRootKeyFromFullPath(path, out string subKey);
        int result = MsWinCoreRegistry.RegOpenKeyExW(hKey, subKey, 0, KeyAccess.KEY_WRITE, out IntPtr phkResult);
        MarshalAssert(result);
        byte[] data = BitConverter.GetBytes(value);
        result = MsWinCoreRegistry.RegSetValueExW(phkResult, valueName, 0, KeyType.REG_QWORD, data, data.Length);
        MsWinCoreRegistry.RegCloseKey(phkResult);
        MarshalAssert(result);
    }

    /// <summary>
    /// 获取注册表 String 类型值
    /// Get registry string value
    /// </summary>
    /// <param name="path">完整注册表路径</param>
    /// <param name="valueName">要读取的项值名称</param>
    /// <returns>获取的值</returns>
    /// <exception cref="Exception"></exception>
    public static string GetStringValue(string path, string valueName)
    {
        IntPtr hKey = GetRegistryRootKeyFromFullPath(path, out string subKey);
        int result = MsWinCoreRegistry.RegOpenKeyExW(hKey, subKey, 0, KeyAccess.KEY_READ, out IntPtr phkResult);
        MarshalAssert(result);
        int size = 1024;
        IntPtr rawData = WinMemory.FastLocalAlloc(size);
        result = MsWinCoreRegistry.RegGetValueW(phkResult, null, valueName, KeyRrf.RRF_RT_REG_SZ, out int _, rawData, ref size);
        MsWinCoreRegistry.RegCloseKey(phkResult);
        MarshalAssert(result);
        string data = Marshal.PtrToStringUni(rawData);
        WinMemory.FastLocalFree(rawData);
        return data;
    }

    /// <summary>
    /// 获取注册表 Unsigned Int32 类型值
    /// Get registry unsigned int32 value
    /// </summary>
    /// <param name="path">完整注册表路径</param>
    /// <param name="valueName">要读取的项值名称</param>
    /// <returns>获取的值</returns>
    /// <exception cref="Exception"></exception>
    public static uint GetDwordValue(string path, string valueName)
    {
        IntPtr hKey = GetRegistryRootKeyFromFullPath(path, out string subKey);
        int result = MsWinCoreRegistry.RegOpenKeyExW(hKey, subKey, 0, KeyAccess.KEY_READ, out IntPtr phkResult);
        MarshalAssert(result);
        int size = 1024;
        IntPtr rawData = WinMemory.FastLocalAlloc(size);
        result = MsWinCoreRegistry.RegGetValueW(phkResult, null, valueName, KeyRrf.RRF_RT_DWORD, out int _, rawData, ref size);
        MsWinCoreRegistry.RegCloseKey(phkResult);
        MarshalAssert(result);
        uint data = (uint)Marshal.ReadInt64(rawData);
        WinMemory.FastLocalFree(rawData);
        return data;
    }

    /// <summary>
    /// 获取注册表 Unsigned Int64 类型值
    /// Get registry unsigned int64 value
    /// </summary>
    /// <param name="path">完整注册表路径</param>
    /// <param name="valueName">要读取的项值名称</param>
    /// <returns>获取的值</returns>
    /// <exception cref="Exception"></exception>
    public static ulong GetQwordValue(string path, string valueName)
    {
        IntPtr hKey = GetRegistryRootKeyFromFullPath(path, out string subKey);
        int result = MsWinCoreRegistry.RegOpenKeyExW(hKey, subKey, 0, KeyAccess.KEY_READ, out IntPtr phkResult);
        MarshalAssert(result);
        int size = 1024;
        IntPtr rawData = WinMemory.FastLocalAlloc(size);
        result = MsWinCoreRegistry.RegGetValueW(phkResult, null, valueName, KeyRrf.RRF_RT_QWORD, out int _, rawData, ref size);
        MsWinCoreRegistry.RegCloseKey(phkResult);
        MarshalAssert(result);
        ulong data = (ulong)Marshal.ReadInt64(rawData);
        WinMemory.FastLocalFree(rawData);
        return data;
    }

    /// <summary>
    /// 删除注册表值
    /// Delete registry value
    /// </summary>
    /// <param name="path">完整注册表路径</param>
    /// <param name="valueName">要删除的项值名称</param>
    /// <exception cref="Exception"></exception>
    public static void DeleteValue(string path, string valueName)
    {
        IntPtr hKey = GetRegistryRootKeyFromFullPath(path, out string subKey);
        int result = MsWinCoreRegistry.RegOpenKeyExW(hKey, subKey, 0, KeyAccess.KEY_WRITE, out IntPtr phkResult);
        MarshalAssert(result);
        result = MsWinCoreRegistry.RegDeleteValueW(phkResult, valueName);
        MsWinCoreRegistry.RegCloseKey(phkResult);
        MarshalAssert(result);
    }
}
