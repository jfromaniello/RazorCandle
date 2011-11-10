using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RazorEngine;

namespace RazorCandle
{
    public static class Generator
    {
        private static string aspNetProjectTemplatesFolder;

        public static void Generate(Arguments arguments)
        {
            Razor.SetTemplateBase(typeof(HtmlTemplateBase<>));
            var model = DeserializeModel(arguments);

            if(!string.IsNullOrEmpty(arguments.AspNetProjectFolder))
            {
                aspNetProjectTemplatesFolder = Path.Combine(Path.GetFullPath(arguments.AspNetProjectFolder), "views\\shared");
            }

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
            
            using(new CurrentDirectoryContext(Path.GetDirectoryName(fullPath)))
            {
                if (!File.Exists(fullPath))
                {
                    if (File.Exists(fullPath + ".cshtml"))
                    {
                        fullPath += ".cshtml";    
                    }
                    if(!string.IsNullOrEmpty(aspNetProjectTemplatesFolder))
                    {
                        var candidateTemplateName = Path.Combine(aspNetProjectTemplatesFolder,
                                                                 Path.GetFileName(fullPath) + ".cshtml");
                        if(File.Exists(candidateTemplateName))
                        {
                            fullPath = candidateTemplateName;
                        }
                    }
                }

                var template = File.ReadAllText(fullPath);

                var result = "";
                if (!string.IsNullOrEmpty(template))
                {
                    result = Razor.Parse(template, model);
                }
                return result;
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
}