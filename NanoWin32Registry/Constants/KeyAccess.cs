namespace NanoWin32Registry.Constants;

public static class KeyAccess
{
    public const int KEY_QUERY_VALUE = 0x0001;
    public const int KEY_ENUMERATE_SUB_KEYS = 0x0008;
    public const int KEY_SET_VALUE = 0x0002;
    public const int KEY_CREATE_SUB_KEY = 0x0004;
    public const int KEY_CREATE_LINK = 0x0020;
    public const int KEY_NOTIFY = 0x0010;
    public const int KEY_WOW64_32KEY = 0x0200;
    public const int KEY_WOW64_64KEY = 0x0100;
    public const int KEY_EXECUTE = 0x20019;
    public const int KEY_READ = 0x20019;
    public const int KEY_WRITE = 0x20006;
    public const int KEY_ALL_ACCESS = 0xF003F;
}
