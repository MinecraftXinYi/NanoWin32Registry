using NanoWin32Registry;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.ReadKey();
            NanoRegistryManager.SetStringValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Test", "test3", "HiWorld");
            NanoRegistryManager.SetDwordValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Test", "test4", 26100);
            NanoRegistryManager.SetQwordValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Test", "test5", 99999999);
            Console.WriteLine(NanoRegistryManager.TrySetDwordValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Test", "test6", 10).ToString());
            Console.WriteLine(NanoRegistryManager.TrySetQwordValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Test", "test7", 100000000).ToString());
            Console.WriteLine(NanoRegistryManager.TrySetStringValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Test", "test8", "Windows").ToString());
            Console.WriteLine(NanoRegistryManager.GetDwordValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Test", "test1").ToString());
            Console.WriteLine(NanoRegistryManager.TryGetQwordValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Test", "test2", out ulong? c).ToString());
            Console.WriteLine(c);
            Console.WriteLine(NanoRegistryManager.GetStringValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Test", "test8"));
            Console.WriteLine(NanoRegistryManager.TryGetStringValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Test", "test3", out string? a).ToString());
            Console.WriteLine(a);
            Console.WriteLine(NanoRegistryManager.TryGetDwordValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Test", "test6", out uint? b).ToString());
            Console.WriteLine(b);
            Console.WriteLine(NanoRegistryManager.GetQwordValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Test", "test2").ToString());
            Console.ReadKey();
        }
    }
}
