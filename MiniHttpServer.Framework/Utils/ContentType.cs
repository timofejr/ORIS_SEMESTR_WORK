namespace MiniHttpServer.Framework.Utils;

public class ContentType // TODO: сделать синглтоном и добавить в контекст
{
    private static readonly Dictionary<string, string> _headerByExtension = new()
    {
        { ".html", "text/html" },
        { ".css", "text/css" },
        { ".js", "application/javascript" },
        { ".png", "image/png" },
        { ".jpg", "image/jpeg" },
        { ".jpeg", "image/jpeg" },
        { ".webp", "image/webp" },
        { ".ico", "image/x-icon" },
        { ".svg", "image/svg+xml" },
        { ".json", "application/json" }
    };

    public static string GetContentTypeFromFile(string path)
    {
        return _headerByExtension.TryGetValue(new FileInfo(path).Extension, out var ext) ? ext : "text/html";
    }

    public static string GetContentTypeByExtension(string extension)
    {
        return _headerByExtension.TryGetValue(extension, out var ext) ? ext : "text/html";
    }
}