using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace Working_time_management
{
    public static class IniHandler   // revision 11
    {
        static readonly string Path = @"..\..\..\data\admin\init\settings.ini";

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern long WritePrivateProfileString(string Section, string Key, string Value, string FilePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);


        public static string  Read(string Key, string Section)
        {
            var RetVal = new StringBuilder(255);
            GetPrivateProfileString(Section, Key, "", RetVal, 255, Path);
            return RetVal.ToString();
        }

        public static void Write(string Key, string Value, string Section)
        {
            WritePrivateProfileString(Section, Key, Value, Path);
        }

        public static void DeleteKey(string Key, string Section)
        {
            Write(Key, null, Section);
        }

        public static void DeleteSection(string Section)
        {
            Write(null, null, Section);
        }

        public static bool KeyExists(string Key, string Section)
        {
            return Read(Key, Section).Length > 0;
        }
    }
}