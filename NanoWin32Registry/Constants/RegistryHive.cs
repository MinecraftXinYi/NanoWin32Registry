using System;

namespace NanoWin32Registry.Constants;

public static class RegistryHive
{
    public static readonly IntPtr HKEY_CLASSES_ROOT = new(unchecked((int)0x80000000));
    public static readonly IntPtr HKEY_CURRENT_USER = new(unchecked((int)0x80000001));
    public static readonly IntPtr HKEY_LOCAL_MACHINE = new(unchecked((int)0x80000002));
    public static readonly IntPtr HKEY_USERS = new(unchecked((int)0x80000003));
    public static readonly IntPtr HKEY_CURRENT_CONFIG = new(unchecked((int)0x80000005));
}
