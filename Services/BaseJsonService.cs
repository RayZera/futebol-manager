// Services/BaseJsonService.cs
using System.Text.Json;

namespace FutebolManager.Services;

public abstract class BaseJsonService<T>
{
    protected readonly string FilePath;
    protected readonly List<T> Items;

    protected BaseJsonService(string fileName)
    {
        FilePath = Path.Combine(AppContext.BaseDirectory, "Database", fileName);
        Items = File.Exists(FilePath)
            ? JsonSerializer.Deserialize<List<T>>(File.ReadAllText(FilePath)) ?? new()
            : new();
    }

    protected void Save()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(FilePath)!);
        File.WriteAllText(FilePath,
            JsonSerializer.Serialize(Items, new JsonSerializerOptions { WriteIndented = true }));
    }
}
