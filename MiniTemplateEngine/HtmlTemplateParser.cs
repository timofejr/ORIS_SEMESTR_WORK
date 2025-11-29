using System.Text.RegularExpressions;

namespace MiniTemplateEngine;

public class HtmlTemplateParser
{
    private const string VarTokenStart = "$(";
    private const string VarTokenEnd = ")";

    private const string BlockTokenStart = "$";
    private const string BlockTokenEnd = "\n";
    
    private const string ForeachTagStart = "foreach";
    private const string ForeachTagEnd = "endfor";
    
    private const string IfTagStart = "if";
    private const string IfTagElse = "else";
    private const string IfTagEnd = "endif";

    private static readonly Regex Rg = new (
        $@"({Regex.Escape(VarTokenStart)}.*?{Regex.Escape(VarTokenEnd)}|{Regex.Escape(BlockTokenStart)}.*?{Regex.Escape(BlockTokenEnd)})",
        RegexOptions.Singleline
    );
    
    public List<Node> ParseTemplate(string template)
    {
        var tokens = Rg.Split(template).ToList();

        var (nodes, _, _) = Parse(tokens, 0, new());
        
        return nodes;
    }

    private (List<Node> nodes, string endTag, int pos) Parse(List<string> tokens, int start, HashSet<string> endTags)
    {
        var nodes = new List<Node>();
        int i = start;
        string token;

        while (i < tokens.Count)
        {
            token = tokens[i];

            if (token.StartsWith(VarTokenStart))
            {
                var expression = token[VarTokenStart.Length .. ^VarTokenEnd.Length];
                nodes.Add(new VariableNode(expression));
            }
            else if (token.StartsWith(BlockTokenStart))
            {
                var parts = token[BlockTokenStart.Length .. ^BlockTokenEnd.Length].Trim().Split('(', StringSplitOptions.RemoveEmptyEntries);
                var tag = parts[0];
                
                if (endTags.Contains(tag))
                    return (nodes, tag, i);

                if (tag == ForeachTagStart)
                {
                    var variableName = parts[1].Split(" ", StringSplitOptions.RemoveEmptyEntries)[1];
                    var collectionName = parts[1].Split(" ", StringSplitOptions.RemoveEmptyEntries)[3][.. ^1];
                    var (body, _, pos) = Parse(tokens, i + 1, new HashSet<string> { ForeachTagEnd });
                    
                    nodes.Add(new ForNode(variableName, collectionName, body));
                    i = pos;
                }
                else if (tag == IfTagStart)
                {
                    var condition = parts[1][.. ^1];
                    var (trueBranch, endTag, pos2) = Parse(tokens, i + 1, new HashSet<string> { IfTagEnd, IfTagElse });
                    List<Node> falseBranch = new();
                
                    if (endTag == IfTagElse)
                        (falseBranch, _, pos2) = Parse(tokens, pos2 + 1, new HashSet<string> { IfTagEnd });
                
                    nodes.Add(new IfNode(condition, trueBranch, falseBranch));
                    i = pos2;
                }
            }
            else if (!string.IsNullOrEmpty(token))
            {
                nodes.Add(new TextNode(token));
            }

            i++;
        }

        return (nodes, null, i);
    }
}