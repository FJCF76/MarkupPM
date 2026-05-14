using System.Windows;

namespace MarkupPM.Views;

public partial class OverwriteConfirmDialog : Window
{
    public bool Confirmed { get; private set; }

    public OverwriteConfirmDialog(string fileName)
    {
        InitializeComponent();
        MessageText.Text = $"Ya existe un archivo \"{fileName}\".\n¿Deseas sobreescribirlo?";

        var owner = Application.Current.Windows
            .OfType<Window>()
            .FirstOrDefault(w => w.IsActive) ?? Application.Current.MainWindow;
        if (owner is not null)
            Owner = owner;
    }

    private void Yes_Click(object sender, RoutedEventArgs e)
    {
        Confirmed = true;
        DialogResult = true;
    }

    private void No_Click(object sender, RoutedEventArgs e)
    {
        Confirmed = false;
        DialogResult = false;
    }
}
