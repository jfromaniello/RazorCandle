using System;
using System.Net.Mime;
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
            try
            {
                Console.WriteLine("Rendering " + arguments.Source);
                Generator.Generate(arguments);
            }catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
