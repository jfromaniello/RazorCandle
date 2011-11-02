using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RazorEngine;
using RazorEngine.Templating;

namespace RazorCandle
{
    public static class Generator
    {
        public static void Generate(Arguments arguments)
        {
            Razor.SetTemplateBase(typeof(HtmlTemplateBase<>));
            var template = File.ReadAllText(arguments.Source);

            var model = DeserializeModel(arguments);;
            model.__SourcePath = Path.GetDirectoryName(arguments.Source);
            try
            {
                Console.WriteLine("Saving rendered template to " + arguments.Destination);

                var result = Razor.Parse(template, model);
                File.WriteAllText(arguments.Destination, result);
                
                if(arguments.Verbose)
                {
                    Console.WriteLine(result);
                }
            
            }
            catch (TemplateCompilationException ex)
            {
                Console.WriteLine("Template compilation exception: ");
                foreach (var compilerError in ex.Errors)
                {
                    Console.WriteLine("In file: " + compilerError.FileName 
                                     + ", line: " + compilerError.Line 
                                     + ", error: " +  compilerError.ErrorText);
                }
            }
            
        }

        private static dynamic DeserializeModel(Arguments arguments)
        {
            if (string.IsNullOrEmpty(arguments.Model)) return new ExpandoObject();
            var serializer = JsonSerializer.Create(new JsonSerializerSettings());
            IDictionary<string, JToken> deserializeModel;
            using (var jsonReader = new JsonTextReader(new StringReader(arguments.Model)))
            {
                deserializeModel = (IDictionary<string, JToken>) serializer.Deserialize(jsonReader);
            }
            return deserializeModel.ToExpando();
        }
    }

    public class HtmlTemplateBase<T> : TemplateBase<T>
    {
        private HtmlHelper helper;

        public HtmlHelper Html
        {
            get
            {
                dynamic model = this.Model;
                return helper ?? (helper = new HtmlHelper(model.__SourcePath));
            }
        }
    }

    public class HtmlHelper
    {
        private readonly string sourcePath;

        public HtmlHelper(string sourcePath)
        {
            this.sourcePath = sourcePath;
        }

        public string Partial(string sourceFile)
        {
            var path = Path.Combine(sourcePath, sourceFile);
            var absolutePath = Path.GetFullPath(path);
            var directory = Path.GetDirectoryName(absolutePath);
            return Razor.Parse(File.ReadAllText(path), new { __SourcePath = directory });
        }
    }
}