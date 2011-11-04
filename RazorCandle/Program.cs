using System;
using System.Threading;
using CmdLine;
using RazorEngine.Templating;

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
            }
            catch (TemplateCompilationException ex)
            {
                Console.WriteLine("Template compilation exception: ");
                foreach (var compilerError in ex.Errors)
                {
                    Console.WriteLine("In file: " + compilerError.FileName
                                     + ", line: " + compilerError.Line
                                     + ", error: " + compilerError.ErrorText);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
