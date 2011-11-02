using System;
using CmdLine;

namespace RazorCandle
{
    class Program
    {
        static void Main(string[] args)
        {
            Arguments arguments;
            try
            {
                arguments = CommandLine.Parse<Arguments>();
            }
            catch (CommandLineException exception)
            {
                Console.WriteLine(exception.ArgumentHelp.Message);
                Console.WriteLine(exception.ArgumentHelp.GetHelpText(Console.BufferWidth));
                return;
            }
            Generator.Generate(arguments);
        }
    }
}
