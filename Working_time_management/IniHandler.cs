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
    public static class IniHandler      // Klasse, die Ini-Dateien verarbeitet, Quelle/Inspiration: https://stackoverflow.com/questions/217902/reading-writing-an-ini-file
    {
        private static readonly string Path = @"..\..\..\data\admin\init\settings.ini";     // Pfad zur ini-Datei

        [DllImport("kernel32", CharSet = CharSet.Unicode)]      // DLL laden
        static extern long WritePrivateProfileString(string Section, string Key, string Value, string FilePath);    // externe Funktion erstellen

        [DllImport("kernel32", CharSet = CharSet.Unicode)]      // DLL laden
        static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);     // externe Funktion erstellen


        public static string Read(string Key, string Section)       // liest gewünschten Value aus 
        {
            var RetVal = new StringBuilder(255);
            GetPrivateProfileString(Section, Key, "", RetVal, 255, Path);
            return RetVal.ToString();
        }

        public static void Write(string Key, string Value, string Section)  // schreibt oder überschreibt Wertepaar in gewünschter Section
        {
            WritePrivateProfileString(Section, Key, Value, Path);
        }

        public static void DeleteKey(string Key, string Section)            // löscht Key und Value Paar einer Section
        {
            Write(Key, null, Section);
        }

        public static void DeleteSection(string Section)                    // löscht komplette Section
        {
            Write(null, null, Section);
        }

        public static bool KeyExists(string Key, string Section)            // checkt, ob Key in einer Section existiert
        {
            return Read(Key, Section).Length > 0;                           // Key auslesen und prüfen, ob Ergebnis-String kein Null-String ist
        }
    }
}