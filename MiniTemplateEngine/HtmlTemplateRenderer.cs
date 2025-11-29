using System.Text;

namespace MiniTemplateEngine;

public class HtmlTemplateRenderer: IHtmlTemplateRenderer
{
    public string RenderFromString(string htmlTemplate, object dataModel)
    {
        var templateParser = new HtmlTemplateParser();
        
        var nodes = templateParser.ParseTemplate(htmlTemplate);

        var renderedTemplate = new StringBuilder();

        foreach (var node in nodes)
        {
            renderedTemplate.Append(node.Render(dataModel));
        }
        
        return renderedTemplate.ToString();
    }

    public string RenderFromFile(string filePath, object dataModel)
    {
        var htmlTemplate = File.ReadAllText(filePath);
        return RenderFromString(htmlTemplate, dataModel);
    }

    public string RenderToFile(string inputFilePath, string outputFilePath, object dataModel)
    {
        var htmlTemplate = File.ReadAllText(inputFilePath);
        var renderedHtmlTemplate = RenderFromString(htmlTemplate, dataModel);
        
        File.WriteAllText(outputFilePath, renderedHtmlTemplate);
        
        return RenderFromString(htmlTemplate, dataModel);
    }
}