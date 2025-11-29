namespace MiniTemplateEngine;

/// <summary>
/// Абстрактный класс, который представляет узел синтаксического абстрактного дерева
/// </summary>
public abstract class Node
{
    public abstract string Render(object data); // Метод который рендерит, подставляя в него наши данные/перменные и т.д
}