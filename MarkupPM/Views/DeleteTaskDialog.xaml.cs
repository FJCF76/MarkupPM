using System.Windows;

namespace MarkupPM.Views;

public partial class DeleteTaskDialog : Window
{
    public DeleteTaskDialog(string taskName)
    {
        InitializeComponent();
        MessageText.Text = $"¿Eliminar la tarea \"{taskName}\"?";
    }

    private void Cancel_Click(object sender, RoutedEventArgs e) => DialogResult = false;

    private void Delete_Click(object sender, RoutedEventArgs e) => DialogResult = true;
}
