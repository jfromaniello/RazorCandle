using CmdLine;

namespace RazorCandle
{
    [CommandLineArguments(Program = "Razor Candle", 
        Title = "Execute the razor engine to a template and save the result in a file", 
        Description = "razorcandel ")]
    public class Arguments
    {
        [CommandLineParameter(Command = "?", Default = false, Description = "Show Help", Name = "Help", IsHelp = true)]
        public bool Help { get; set; }

        [CommandLineParameter(
            Name = "source", 
            ParameterIndex = 1, 
            Required = true, 
            Description = "Specifies the source razor file.")]
        public string Source 
        { 
            get
            {
                return source;
            } 
            set
            {
                source = RemoveLastDoubleQuote(value);
            } 
        }

        private string destination;
        private string source;
        private string aspNetProjectFolder;

        [CommandLineParameter(
            Name = "destination", 
            ParameterIndex = 2, 
            Description = "Specify the output file. By default is the same name as the source with the html extension.")]
        public string Destination
        {
            get
            {   
                if(string.IsNullOrEmpty(destination))
                {
                    return Source.Substring(0, Source.LastIndexOf(".")) + ".html";
                }
                return destination;
            }
            set { destination = RemoveLastDoubleQuote(value); }
        }

        [CommandLineParameter(
            Command = "M",
            Required = false,
            Description = "The model for rendering the template in JSON format.")]
        public string Model { get; set; }

        [CommandLineParameter(
            Command = "V", 
            Required = false, 
            Description = "Verbose mode. Show result in the output.")]
        public bool Verbose { get; set; }

        [CommandLineParameter(
            Command = "AspNetProjectFolder",
            Required = false,
            Description = "Path to an Asp.Net project folder. This is useful if you need to resolve things from view\\shared")]
        public string AspNetProjectFolder 
        { 
            get 
            {
                return aspNetProjectFolder;
            } 
            set 
            {
                aspNetProjectFolder = RemoveLastDoubleQuote(value);
            } 
        }

        private string RemoveLastDoubleQuote(string value)
        {
            return value.EndsWith("\"") ? value.Substring(0, value.Length - 1) : value;
        }
    }
}