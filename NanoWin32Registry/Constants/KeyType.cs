namespace NanoWin32Registry.Constants;

public static class KeyType
{
    public const int REG_NONE = 0;
    public const int REG_SZ = 1;
    public const int REG_EXPAND_SZ = 2;
    public const int REG_MULTI_SZ = 7;
    public const int REG_DWORD = 4;
    public const int REG_DWORD_LITTLE_ENDIAN = 4;
    public const int REG_DWORD_BIG_ENDIAN = 5;
    public const int REG_QWORD = 11;
    public const int REG_QWORD_LITTLE_ENDIAN = 11;
    public const int REG_BINARY = 3;
    public const int REG_RESOURCE_LIST = 8;
    public const int REG_FULL_RESOURCE_DESCRIPTOR = 9;
    public const int REG_RESOURCE_REQUIREMENTS_LIST = 10;
    public const int REG_LINK = 6;
}
