namespace MiniTemplateEngine;

/// <summary>
/// TextNode - представляет собок узел, в котором содержится только текст, в который не нужно подставлять никакие данные
/// </summary>
public class TextNode: Node
{
    public string Text { get; } // Свойство представляющее собой просто текст

    public TextNode(string text)
    {
        Text = text; // Передаем текст в поле
    }
    
    public override string Render(object data)
    {
        return Text; // Возвращаем просто текст т.к никакие данные не нужно рендерить
    }
}