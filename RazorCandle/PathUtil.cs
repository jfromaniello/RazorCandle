using System;
using System.IO;

namespace RazorCandle
{
    public class PathUtil
    {
        public static string GetRelativePath(string rootPath, string fullPath)
        {
            if(!rootPath.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                rootPath += Path.DirectorySeparatorChar;
            }
            var value = new Uri(rootPath);
            var value2 = new Uri(fullPath);
            var result = value.MakeRelativeUri(value2)
                            .OriginalString.Replace("/", Path.DirectorySeparatorChar.ToString());
            return result;
        }
    }
}