namespace MarkupPM.Services;

public interface IRecentFilesService
{
    IReadOnlyList<string> GetRecent();
    Task AddRecent(string filePath);
}
