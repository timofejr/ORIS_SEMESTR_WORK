using System.Collections;
using System.Dynamic;
using System.Reflection;
using System.Text;

namespace MiniTemplateEngine;

public class ForNode: Node
{
    public string VariableName { get; }
    public string CollectionName { get; }
    public List<Node> Body { get; }

    public ForNode(string variableName, string collectionName, List<Node> body)
    {
        VariableName = variableName;
        CollectionName = collectionName;
        Body = body;
    }
    
    public override string Render(object data)
    {
        var collection = VariableNode.GetValueFromExpression(CollectionName, data);

        if (collection is not IEnumerable list)
            return "";

        var sb = new StringBuilder();

        foreach (var item in list)
        { 
            dynamic localData = new ExpandoObject();
            ((IDictionary<string, object>)localData)[VariableName] = item;
            
            foreach (var node in Body)
            {
                var renderedNode = node.Render(localData);
                sb.Append(renderedNode);
            }
        }

        return sb.ToString();
    }
}