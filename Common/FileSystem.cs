using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTiles.Common
{
    public enum ENUM_SpecialFolders
    {
        Schemas,
        Maps,
    }
    public static class FileSystem
    {
        static Dictionary<ENUM_SpecialFolders, string> SpecialFolderPaths = new Dictionary<ENUM_SpecialFolders, string>()
        {
            { ENUM_SpecialFolders.Schemas, @"./Resources/Schemas"},
            { ENUM_SpecialFolders.Maps, @"./Resources/Maps"},
        };
        public static string GetSpecialFolderPath(ENUM_SpecialFolders key)
        {
            return SpecialFolderPaths[key];
        }
        public static string[] GetFiles(ENUM_SpecialFolders key)
        {
            return Directory.GetFiles(SpecialFolderPaths[key], "*.*", SearchOption.AllDirectories);
        }
    }
}
