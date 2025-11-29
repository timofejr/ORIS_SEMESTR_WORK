using System.Text;
using MiniHttpServer.Framework.Settings;
using MiniHttpServer.Utils;

namespace MiniHttpServer.Framework.Utils;

public static class BufferManager // TODO: сделать синглтоном и добавить в контекст
{
    public static byte[] GetBytesFromFile(string path)
    {
        if (Path.HasExtension(path))
            return File.ReadAllBytes(TryGetFile(path));
        
        return File.ReadAllBytes(TryGetFile(path + "/index.html"));            
    }

    public static byte[] GetBytesFromJson(string jsonString)
    {
        return Encoding.UTF8.GetBytes(jsonString);
    }

    public static byte[] GetBytesFromString(string templateString)
    {
        return Encoding.UTF8.GetBytes(templateString);
    }

    private static string TryGetFile(string path)
    {
        try
        {
            var targetPath = Path.Combine(path.Split("/"));

            string? found = Directory.EnumerateFiles(SettingsManager.Instance.Settings.StaticFilesPath,
                    $"{Path.GetFileName(path)}", SearchOption.AllDirectories)
                .FirstOrDefault(f => f.EndsWith(targetPath, StringComparison.OrdinalIgnoreCase));

            return found ?? throw new FileNotFoundException(path);
        }
        catch (DirectoryNotFoundException)
        {
            Logger.Instance.LogMessage("Директория не найдена");
        }
        catch (FileNotFoundException)
        {
            Logger.Instance.LogMessage("Файл не найден");
        }
        catch (Exception)
        {
            Logger.Instance.LogMessage("Ошибка при извлечении текста");
        }

        return SettingsManager.Instance.Settings.StaticFilesPath + "404.html";
    }
}