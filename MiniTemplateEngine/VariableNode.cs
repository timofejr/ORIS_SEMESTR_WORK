using System.Collections;
using System.Reflection;

namespace MiniTemplateEngine;

/// <summary>
/// VariableNode - представляет собой узел, который уже будет рендериться с подставленной в него переменной
/// </summary>
public class VariableNode: Node
{
    public string Expression { get; } // Свойство в котором будет храниться выражение, пример: object.Name, user.Location.City

    public VariableNode(string expression)
    {
        Expression = expression.Trim(); // Удаляем все пробелы
    }
    
    public override string Render(object data)
    {
        return GetValueFromExpression(Expression, data).ToString() ?? string.Empty;
    }
    
    /// <summary>
    /// Извлекаем значение переменной из выражения
    /// </summary>
    /// <param name="expression">Выражение из которой извлекаем значение переменной</param>
    /// <param name="data">Данные где берем значение переменной</param>
    /// <returns></returns>
    public static object GetValueFromExpression(string expression, object data)
    {
        var parts = expression.Split("."); // разделяем выражение на названия полей
        var currentObject = data; // текущим объектом делаем весь объект data

        foreach (var part in parts)
        {
            if (currentObject == null) // проверяем текущий объект на null
                return null;

            if (currentObject is IDictionary<string, object> dict) // проверяем не является ли наш объект словарем
            {
                if (dict.TryGetValue(part, out var value)) 
                {
                    currentObject = value;
                    continue;
                }
                return null;
            }

            var propertyInfo = currentObject.GetType()
                .GetProperty(part, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            
            if (propertyInfo == null)
                return null;

            currentObject = propertyInfo.GetValue(currentObject);
        }
        
        return currentObject;
    }
}