namespace RazorCandle
{
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