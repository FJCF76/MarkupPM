using System.IO;
using System.Text.Json;
using Windows.Storage;
using Windows.Storage.AccessCache;

namespace MarkupPM.Services;

public class RecentFilesService : IRecentFilesService
{
    // MostRecentlyUsedList has a hard limit of 25 entries — MaxItems must stay well below that
    private const int MaxItems = 5;
    private readonly string _path;
    private List<string> _recent;

    public RecentFilesService()
    {
        // Package-scoped local folder, accessible under AppContainer
        var dir = ApplicationData.Current.LocalFolder.Path;
        _path = Path.Combine(dir, "recent.json");
        _recent = Load();
    }

    public IReadOnlyList<string> GetRecent() =>
        _recent
            .Where(path => StorageApplicationPermissions.MostRecentlyUsedList
                .Entries.Any(e => e.Metadata == path))
            .ToList()
            .AsReadOnly();

    public async Task AddRecent(string filePath)
    {
        _recent.Remove(filePath);
        _recent.Insert(0, filePath);
        if (_recent.Count > MaxItems)
            _recent = _recent.Take(MaxItems).ToList();
        Save();

        // Register persistent access token so AppContainer can reopen the file in future sessions
        try
        {
            var storageFile = await StorageFile.GetFileFromPathAsync(filePath);
            StorageApplicationPermissions.MostRecentlyUsedList.Add(storageFile, filePath);
        }
        catch { /* non-critical: file may not exist yet */ }
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
