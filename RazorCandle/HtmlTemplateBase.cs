using System.Collections;
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
                dynamic model = Model;
                return helper ?? (helper = new HtmlHelper(model));
            }
        }

        public EnumerableFileWrapper GetFiles(string fullPattern)
        {
            var wildcardFolder = Path.GetFullPath(fullPattern.Substring(0, fullPattern.LastIndexOf(Path.DirectorySeparatorChar)));
            var wildcardFilePattern = fullPattern.Substring(fullPattern.LastIndexOf(Path.DirectorySeparatorChar) + 1);
            var filesIn = string.IsNullOrEmpty(wildcardFilePattern) 
                                ? Directory.GetFiles(wildcardFolder)
                                : Directory.GetFiles(wildcardFolder, wildcardFilePattern);
            return new EnumerableFileWrapper(filesIn.Select(f => PathUtil.GetRelativePath(Directory.GetCurrentDirectory(), f)));
        } 

        public UrlHelper Url
        {
            get
            {
                return new UrlHelper();
            }
        }

        public class EnumerableFileWrapper : IEnumerable<string>
        {
            private readonly IEnumerable<string> wrapped;


            public EnumerableFileWrapper(IEnumerable<string> wrapped)
            {
                this.wrapped = wrapped;
            }

            public IEnumerable<string> ToUrl()
            {
                return wrapped.Select(v => v.Replace(Path.DirectorySeparatorChar.ToString(), "/"));
            }

            public IEnumerator<string> GetEnumerator()
            {
                return wrapped.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }


}