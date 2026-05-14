using System.IO;
using System.Windows;
using Windows.Storage;

namespace MarkupPM.Views;

public partial class SaveFileDialog : Window
{
    public string? SelectedPath { get; private set; }

    public SaveFileDialog(string suggestedName = "proyecto")
    {
        InitializeComponent();
        var owner = Application.Current.Windows
            .OfType<Window>()
            .FirstOrDefault(w => w.IsActive) ?? Application.Current.MainWindow;
        if (owner is not null)
            Owner = owner;

        var folder = ApplicationData.Current.LocalFolder.Path;
        FileNameBox.Text = suggestedName;
        LocationText.Text = folder;
        FileNameBox.SelectAll();
        FileNameBox.Focus();
    }

    private void FileNameBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
    {
        SaveBtn.IsEnabled = !string.IsNullOrWhiteSpace(FileNameBox.Text);
    }

    private void SaveBtn_Click(object sender, RoutedEventArgs e) => Accept();
    private void CancelBtn_Click(object sender, RoutedEventArgs e) => DialogResult = false;

    private void Accept()
    {
        var name = FileNameBox.Text.Trim();
        if (string.IsNullOrEmpty(name)) return;

        if (!name.EndsWith(".md", StringComparison.OrdinalIgnoreCase))
            name += ".md";

        var folder = ApplicationData.Current.LocalFolder.Path;
        var path = Path.Combine(folder, name);

        if (File.Exists(path))
        {
            var dlg = new OverwriteConfirmDialog(name);
            if (dlg.ShowDialog() != true) return;
        }

        SelectedPath = path;
        DialogResult = true;
    }
}
