namespace MarkupPM.Services;

public interface IRecentFilesService
{
    IReadOnlyList<string> GetRecent();
    void AddRecent(string filePath);
}
