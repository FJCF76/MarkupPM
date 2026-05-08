using System.Windows;

namespace MarkupPM.Views;

public partial class PhaseDeleteDialog : Window
{
    public PhaseDeleteDialog(string phaseName, int taskCount)
    {
        InitializeComponent();

        if (taskCount > 0)
        {
            Title = "No se puede eliminar fase";
            HeaderText.Text = "Esta fase contiene tareas";
            MessageText.Text = $"La fase \"{phaseName}\" tiene {taskCount} tarea{(taskCount == 1 ? string.Empty : "s")}. Mueve o elimina sus tareas antes de borrar la fase.";

            DeleteButton.Visibility = Visibility.Collapsed;
            CancelButton.Content = "Aceptar";
            CancelButton.Margin = new Thickness(0);
            CancelButton.IsDefault = true;
        }
        else
        {
            Title = "Confirmar eliminación";
            HeaderText.Text = "¿Eliminar fase?";
            MessageText.Text = $"¿Eliminar la fase \"{phaseName}\"?";
        }
    }

    private void Cancel_Click(object sender, RoutedEventArgs e) => DialogResult = false;

    private void Delete_Click(object sender, RoutedEventArgs e) => DialogResult = true;
}
