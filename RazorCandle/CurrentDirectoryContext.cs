using System;
using System.IO;

namespace RazorCandle
{
    public class CurrentDirectoryContext :  IDisposable
    {
        private readonly string previousDirectory;
        public CurrentDirectoryContext(string newCurrentDirectory)
        {
            previousDirectory = Directory.GetCurrentDirectory();
            Directory.SetCurrentDirectory(newCurrentDirectory);
        }

        public void Dispose()
        {
            Directory.SetCurrentDirectory(previousDirectory);
        }
    }
}