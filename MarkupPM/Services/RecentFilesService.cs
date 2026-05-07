using System.IO;
using System.Text.Json;

namespace MarkupPM.Services;

public class RecentFilesService : IRecentFilesService
{
    private const int MaxItems = 5;
    private readonly string _path;
    private List<string> _recent;

    public RecentFilesService()
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var dir = Path.Combine(appData, "MarkupPM");
        Directory.CreateDirectory(dir);
        _path = Path.Combine(dir, "recent.json");
        _recent = Load();
    }

    public IReadOnlyList<string> GetRecent() => _recent.Where(File.Exists).ToList().AsReadOnly();

    public void AddRecent(string filePath)
    {
        _recent.Remove(filePath);
        _recent.Insert(0, filePath);
        if (_recent.Count > MaxItems)
            _recent = _recent.Take(MaxItems).ToList();
        Save();
    }

    private List<string> Load()
    {
        try
        {
            if (!File.Exists(_path)) return [];
            var json = File.ReadAllText(_path);
            return JsonSerializer.Deserialize<List<string>>(json) ?? [];
        }
        catch { return []; }
    }

    private void Save()
    {
        try { File.WriteAllText(_path, JsonSerializer.Serialize(_recent)); }
        catch { /* non-critical */ }
    }
}
