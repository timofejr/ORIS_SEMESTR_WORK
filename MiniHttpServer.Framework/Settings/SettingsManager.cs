using System.Text.Json;
using MiniHttpServer.Framework.Context;
using MiniHttpServer.Framework.Utils;

namespace MiniHttpServer.Framework.Settings;

public class SettingsManager
{
    private static SettingsManager _instance;
    private static readonly object _lock = new ();
    
    public SettingsModel Settings { get; private set; }

    private SettingsManager()
    {
        LoadSettings();
    }
    
    public static SettingsManager Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                        _instance = new SettingsManager();
                }
            }
            return _instance;
        }
    }
    
    private void LoadSettings()
    {
        try
        {
            var settingsFile = File.ReadAllText("./settings.json");
            Settings = JsonSerializer.Deserialize<SettingsModel>(settingsFile)
                             ?? throw new InvalidOperationException("Десериализация провалилась");

            var properties = Settings.GetType().GetProperties();

            foreach (var property in properties)
            {
                var propertyValue = property.GetValue(Settings)?.ToString();
                
                if (propertyValue is null || string.IsNullOrWhiteSpace(propertyValue))
                    throw new InvalidOperationException($"Поле '{property.Name}' не было заполнено из settings.json");
            }
        
            GlobalContext.Logger.LogMessage("Настройки упешно загружены");
        }
        catch (FileNotFoundException)
        {
            throw new FileNotFoundException("Файл settings.json не был найден");
        }
        catch (DirectoryNotFoundException)
        {
            throw new DirectoryNotFoundException("Директория с файлом settings.json не была найдена");
        }
    }
}   