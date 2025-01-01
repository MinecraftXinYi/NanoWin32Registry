using System;
using System.Text;
using System.Runtime.InteropServices;

namespace NanoWin32Registry;

using Constants;
using Core;

public static partial class NanoRegistryManager
{
    /// <summary>
    /// 尝试获取注册表根键
    /// Try to get registry root key
    /// </summary>
    /// <param name="fullKeyPath">完整注册表路径</param>
    /// <param name="rootKey">路径包含的注册表根键</param>
    /// <returns>操作是否成功</returns>
    public static bool TryGetRegistryRootKeyFromFullPath(string fullKeyPath, out IntPtr rootKey)
    {
        string root = fullKeyPath.Substring(0, fullKeyPath.IndexOf('\\'));
        switch (root.ToUpper())
        {
            case "HKEY_CLASSES_ROOT":
                rootKey = RegistryHive.HKEY_CLASSES_ROOT;
                return true;
            case "HKEY_CURRENT_USER":
                rootKey = RegistryHive.HKEY_CURRENT_USER;
                return true;
            case "HKEY_LOCAL_MACHINE":
                rootKey = RegistryHive.HKEY_LOCAL_MACHINE;
                return true;
            case "HKEY_USERS":
                rootKey = RegistryHive.HKEY_USERS;
                return true;
            case "HKEY_CURRENT_CONFIG":
                rootKey = RegistryHive.HKEY_CURRENT_CONFIG;
                return true;
            default:
                rootKey = IntPtr.Zero;
                return false;
        }
    }

    /// <summary>
    /// 尝试创建注册表键
    /// Try to create registry key
    /// </summary>
    /// <param name="path">完整注册表路径</param>
    /// <returns>操作是否成功</returns>
    public static bool TryCreateKey(string path)
    {
        IntPtr hKey = GetRegistryRootKeyFromFullPath(path, out string subKey);
        int result = MsWinCoreRegistry.RegCreateKeyExW(hKey, subKey, 0, null, RegOptions.REG_OPTION_NON_VOLATILE, KeyAccess.KEY_WRITE, IntPtr.Zero, out IntPtr phkResult, out _);
        if (result == 0)
        {
            MsWinCoreRegistry.RegCloseKey(phkResult);
            return true;
        }
        else return false;
    }

    /// <summary>
    /// 尝试删除注册表键
    /// Try to delete registry key
    /// </summary>
    /// <param name="path">完整注册表路径</param>
    /// <returns>操作是否成功</returns>
    public static bool TryDeleteKey(string path)
    {
        IntPtr hKey = GetRegistryRootKeyFromFullPath(path, out string subKey);
        int result = MsWinCoreRegistry.RegDeleteKeyW(hKey, subKey);
        return result == 0;
    }

    /// <summary>
    /// 尝试设置注册表 String 类型值
    /// Try to set registry string value
    /// </summary>
    /// <param name="path">完整注册表路径</param>
    /// <param name="valueName">要设置的项值名称</param>
    /// <param name="value">要设置的值</param>
    /// <returns>操作是否成功</returns>
    public static bool TrySetStringValue(string path, string valueName, string value)
    {
        IntPtr hKey = GetRegistryRootKeyFromFullPath(path, out string subKey);
        int result = MsWinCoreRegistry.RegOpenKeyExW(hKey, subKey, 0, KeyAccess.KEY_WRITE, out IntPtr phkResult);
        if (result != 0) return false;
        byte[] data = Encoding.Unicode.GetBytes(value);
        result = MsWinCoreRegistry.RegSetValueExW(phkResult, valueName, 0, KeyType.REG_SZ, data, data.Length);
        MsWinCoreRegistry.RegCloseKey(phkResult);
        return result == 0;
    }

    /// <summary>
    /// 尝试设置注册表 Unsigned Int32 类型值
    /// Try to set registry unsigned int32 value
    /// </summary>
    /// <param name="path">完整注册表路径</param>
    /// <param name="valueName">要设置的项值名称</param>
    /// <param name="value">要设置的值</param>
    /// <returns>操作是否成功</returns>
    public static bool TrySetDwordValue(string path, string valueName, uint value)
    {
        IntPtr hKey = GetRegistryRootKeyFromFullPath(path, out string subKey);
        int result = MsWinCoreRegistry.RegOpenKeyExW(hKey, subKey, 0, KeyAccess.KEY_WRITE, out IntPtr phkResult);
        if (result != 0) return false;
        byte[] data = BitConverter.GetBytes(value);
        result = MsWinCoreRegistry.RegSetValueExW(phkResult, valueName, 0, KeyType.REG_DWORD, data, data.Length);
        MsWinCoreRegistry.RegCloseKey(phkResult);
        return result == 0;
    }

    /// <summary>
    /// 尝试设置注册表 Unsigned Int64 类型值
    /// Try to set registry unsigned int64 value
    /// </summary>
    /// <param name="path">完整注册表路径</param>
    /// <param name="valueName">要设置的项值名称</param>
    /// <param name="value">要设置的值</param>
    /// <returns>操作是否成功</returns>
    public static bool TrySetQwordValue(string path, string valueName, ulong value)
    {
        IntPtr hKey = GetRegistryRootKeyFromFullPath(path, out string subKey);
        int result = MsWinCoreRegistry.RegOpenKeyExW(hKey, subKey, 0, KeyAccess.KEY_WRITE, out IntPtr phkResult);
        if (result != 0) return false;
        byte[] data = BitConverter.GetBytes(value);
        result = MsWinCoreRegistry.RegSetValueExW(phkResult, valueName, 0, KeyType.REG_QWORD, data, data.Length);
        MsWinCoreRegistry.RegCloseKey(phkResult);
        return result == 0;
    }

    /// <summary>
    /// 尝试获取注册表 String 类型值
    /// Try to get registry string value
    /// </summary>
    /// <param name="path">完整注册表路径</param>
    /// <param name="valueName">要读取的项值名称</param>
    /// <param name="value">获取的值</param>
    /// <returns>操作是否成功</returns>
    public static bool TryGetStringValue(string path, string valueName, out string? value)
    {
        IntPtr hKey = GetRegistryRootKeyFromFullPath(path, out string subKey);
        int result = MsWinCoreRegistry.RegOpenKeyExW(hKey, subKey, 0, KeyAccess.KEY_READ, out IntPtr phkResult);
        if (result != 0)
        {
            value = null;
            return false;
        }
        int size = 1024;
        IntPtr rawData = WinMemory.FastLocalAlloc(size);
        result = MsWinCoreRegistry.RegGetValueW(phkResult, null, valueName, KeyRrf.RRF_RT_REG_SZ, out int _, rawData, ref size);
        MsWinCoreRegistry.RegCloseKey(phkResult);
        if (result != 0)
        {
            value = null;
            return false;
        }
        value = Marshal.PtrToStringUni(rawData);
        WinMemory.FastLocalFree(rawData);
        return true;
    }

    /// <summary>
    /// 尝试获取注册表 Unsigned Int32 类型值
    /// Try to get registry unsigned int32 value
    /// </summary>
    /// <param name="path">完整注册表路径</param>
    /// <param name="valueName">要读取的项值名称</param>
    /// <param name="value">获取的值</param>
    /// <returns>操作是否成功</returns>
    public static bool TryGetDwordValue(string path, string valueName, out uint? value)
    {
        IntPtr hKey = GetRegistryRootKeyFromFullPath(path, out string subKey);
        int result = MsWinCoreRegistry.RegOpenKeyExW(hKey, subKey, 0, KeyAccess.KEY_READ, out IntPtr phkResult);
        if (result != 0)
        {
            value = null;
            return false;
        }
        int size = 1024;
        IntPtr rawData = WinMemory.FastLocalAlloc(size);
        result = MsWinCoreRegistry.RegGetValueW(phkResult, null, valueName, KeyRrf.RRF_RT_DWORD, out int _, rawData, ref size);
        MsWinCoreRegistry.RegCloseKey(phkResult);
        if (result != 0)
        {
            value = null;
            return false;
        }
        value = (uint)Marshal.ReadInt64(rawData);
        WinMemory.FastLocalFree(rawData);
        return true;
    }

    /// <summary>
    /// 尝试获取注册表 Unsigned Int64 类型值
    /// Try to get registry unsigned int64 value
    /// </summary>
    /// <param name="path">完整注册表路径</param>
    /// <param name="valueName">要读取的项值名称</param>
    /// <param name="value">获取的值</param>
    /// <returns>操作是否成功</returns>
    public static bool TryGetQwordValue(string path, string valueName, out ulong? value)
    {
        IntPtr hKey = GetRegistryRootKeyFromFullPath(path, out string subKey);
        int result = MsWinCoreRegistry.RegOpenKeyExW(hKey, subKey, 0, KeyAccess.KEY_READ, out IntPtr phkResult);
        if (result != 0)
        {
            value = null;
            return false;
        }
        int size = 1024;
        IntPtr rawData = WinMemory.FastLocalAlloc(size);
        result = MsWinCoreRegistry.RegGetValueW(phkResult, null, valueName, KeyRrf.RRF_RT_QWORD, out int _, rawData, ref size);
        MsWinCoreRegistry.RegCloseKey(phkResult);
        if(result != 0)
        {
            value = null;
            return false;
        }
        value = (ulong)Marshal.ReadInt64(rawData);
        WinMemory.FastLocalFree(rawData);
        return true;
    }

    /// <summary>
    /// 尝试删除注册表值
    /// Try to delete registry value
    /// </summary>
    /// <param name="path">完整注册表路径</param>
    /// <param name="valueName">要删除的项值名称</param>
    /// <returns>操作是否成功</returns>
    public static bool TryDeleteValue(string path, string valueName)
    {
        IntPtr hKey = GetRegistryRootKeyFromFullPath(path, out string subKey);
        int result = MsWinCoreRegistry.RegOpenKeyExW(hKey, subKey, 0, KeyAccess.KEY_WRITE, out IntPtr phkResult);
        if (result != 0) return false;
        result = MsWinCoreRegistry.RegDeleteValueW(phkResult, valueName);
        MsWinCoreRegistry.RegCloseKey(phkResult);
        return result == 0;
    }
}
