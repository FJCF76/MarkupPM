using System.Windows;

namespace MarkupPM.Views;

public enum UnsavedChangesDialogChoice
{
    Cancel,
    Save,
    ExitWithoutSaving
}

public partial class DiscardChangesDialog : Window
{
    public UnsavedChangesDialogChoice Choice { get; private set; } = UnsavedChangesDialogChoice.Cancel;

    public DiscardChangesDialog()
    {
        InitializeComponent();
    }

    private void ExitWithoutSaving_Click(object sender, RoutedEventArgs e)
    {
        Choice = UnsavedChangesDialogChoice.ExitWithoutSaving;
        DialogResult = true;
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        Choice = UnsavedChangesDialogChoice.Save;
        DialogResult = true;
    }
}
