using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RazorEngine.Templating;

namespace RazorCandle
{
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

        public IEnumerable<string> GetFiles(string fullPattern)
        {
            var wildcardFolder = Path.GetFullPath(fullPattern.Substring(0, fullPattern.LastIndexOf(Path.DirectorySeparatorChar)));
            var wildcardFilePattern = fullPattern.Substring(fullPattern.LastIndexOf(Path.DirectorySeparatorChar) + 1);
            var filesIn = string.IsNullOrEmpty(wildcardFilePattern) 
                                ? Directory.GetFiles(wildcardFolder)
                                : Directory.GetFiles(wildcardFolder, wildcardFilePattern);
            return filesIn.Select(f => PathUtil.GetRelativePath(Directory.GetCurrentDirectory(), f));
        } 

        public UrlHelper Url
        {
            get
            {
                return new UrlHelper();
            }
        }
    }
}