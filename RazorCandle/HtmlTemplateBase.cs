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
        public UrlHelper Url
        {
            get
            {
                return new UrlHelper();
            }
        }
    }
}