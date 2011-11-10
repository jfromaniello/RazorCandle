using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
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
            var model = DeserializeModel(arguments);;

            var result = Render(arguments.Source, model);
            File.WriteAllText(arguments.Destination, result);
            if(arguments.Verbose)
            {
                Console.WriteLine(result);
            }
        }

        public static string Render(
            string templatePath, 
            dynamic model)
        {
            var fullPath = Path.GetFullPath(templatePath);

            var previous = Directory.GetCurrentDirectory();
            Directory.SetCurrentDirectory(Path.GetDirectoryName(fullPath));

            if(!File.Exists(fullPath) && File.Exists(fullPath + ".cshtml"))
            {
                fullPath += ".cshtml";
            }

            var template = File.ReadAllText(fullPath);
            
            var result = "";
            if(!string.IsNullOrEmpty(template))
            {
                result = Razor.Parse(template, model);
            }

            Directory.SetCurrentDirectory(previous);
            return result;
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
                return helper ?? (helper = new HtmlHelper(model));
            }
        }
        public UrlHelper Url
        {
            get
            {
                return new UrlHelper();
            }
        }
    }

    public class UrlHelper
    {
        public string Content(string uri)
        {
            //this doesn't work yet, but i need to keep backward compatibility
            return uri;
        }
    }

    public class HtmlHelper
    {
        private readonly dynamic model;

        public HtmlHelper(dynamic model)
        {
            this.model = model;
        }

        public string Partial(string sourceFile)
        {
            return Generator.Render(sourceFile, model);
        }
    }
}