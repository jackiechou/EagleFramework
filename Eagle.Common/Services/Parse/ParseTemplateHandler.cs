using System.Collections;

namespace Eagle.Common.Services.Parse
{
    public class ParseTemplateHandler
    {
        public static string ParseTemplate(Hashtable templateVariables, string templateContent)
        {
            ParseHtmlContents parser = new ParseHtmlContents(templateContent, templateVariables);
            return parser.Parse();
        }
    }
}
