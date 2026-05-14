using System.IO;
using System.Windows;
using System.Windows.Controls;
using Windows.Storage;

namespace MarkupPM.Views;

public partial class OpenFileDialog : Window
{
    public string? SelectedPath { get; private set; }

    private List<FileInfo> _allFiles = [];

    public OpenFileDialog()
    {
        InitializeComponent();
        // Center on the main window
        var owner = Application.Current.Windows
            .OfType<Window>()
            .FirstOrDefault(w => w.IsActive) ?? Application.Current.MainWindow;
        if (owner is not null)
            Owner = owner;

        LoadFiles();
    }

    private void LoadFiles()
    {
        var folder = ApplicationData.Current.LocalFolder.Path;
        _allFiles = Directory.GetFiles(folder, "*.md")
            .Select(f => new FileInfo(f))
            .OrderByDescending(f => f.LastWriteTime)
            .ToList();

        ApplyFilter(string.Empty);
    }

    private void ApplyFilter(string query)
    {
        var filtered = string.IsNullOrWhiteSpace(query)
            ? _allFiles
            : _allFiles.Where(f => f.Name.Contains(query, StringComparison.OrdinalIgnoreCase)).ToList();

        if (filtered.Count == 0)
        {
            FileList.Visibility = Visibility.Collapsed;
            EmptyText.Visibility = Visibility.Visible;
            EmptyText.Text = _allFiles.Count == 0
                ? "No hay archivos .md en LocalState"
                : "No hay coincidencias con la búsqueda";
        }
        else
        {
            FileList.Visibility = Visibility.Visible;
            EmptyText.Visibility = Visibility.Collapsed;
            FileList.ItemsSource = filtered;
        }
    }

    private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        ApplyFilter(SearchBox.Text);
        OpenBtn.IsEnabled = false;
        PathBox.Text = string.Empty;
    }

    private void FileList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (FileList.SelectedItem is FileInfo fi)
        {
            PathBox.Text = fi.FullName;
            OpenBtn.IsEnabled = true;
        }
    }

    private void FileList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (FileList.SelectedItem is FileInfo)
            Accept();
    }

    private void OpenBtn_Click(object sender, RoutedEventArgs e) => Accept();
    private void CancelBtn_Click(object sender, RoutedEventArgs e) => DialogResult = false;

    private void Accept()
    {
        if (FileList.SelectedItem is FileInfo fi)
        {
            SelectedPath = fi.FullName;
            DialogResult = true;
        }
    }
}
