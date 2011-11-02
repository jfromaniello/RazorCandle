using CmdLine;

namespace RazorCandle
{
    [CommandLineArguments(Program = "Razor Candle", 
        Title = "Execute the razor engine against a razor file and save the result in a file", 
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
        public string Source { get; set; }

        private string destination;

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
            set { destination = value; }
        }
        [CommandLineParameter(
            Command = "M",
            Required = false,
            Description = "Json model as string to pass to the model.")]
        public string Model { get; set; }

        [CommandLineParameter(
            Command = "V", 
            Required = false, 
            Description = "Verbose mode. Show result in the output.")]
        public bool Verbose { get; set; }

    }
}