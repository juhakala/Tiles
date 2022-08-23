using System;
using System.Collections.Generic;
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
            { ENUM_SpecialFolders.Maps, @"/Resources/Schemas"},
            { ENUM_SpecialFolders.Maps, @"/Resources/Maps"},
        };
        public static string GetSpecialFolderPath(ENUM_SpecialFolders key)
        {
            return SpecialFolderPaths[key];
        }
    }
}
