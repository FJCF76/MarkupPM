using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using MarkupPM.Services;
using MarkupPM.ViewModels;

namespace MarkupPM.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel(new MdParser(), new MdSerializer(), App.RecentFiles);
    }

    private void ThemeToggle_Click(object sender, RoutedEventArgs e)
    {
        var paletteHelper = new PaletteHelper();
        var theme = paletteHelper.GetTheme();
        var isDark = theme.GetBaseTheme() == BaseTheme.Dark;
        theme.SetBaseTheme(isDark ? BaseTheme.Light : BaseTheme.Dark);
        paletteHelper.SetTheme(theme);
    }

    private void ProjectNameBox_LostFocus(object sender, RoutedEventArgs e)
    {
        if (sender is TextBox tb && DataContext is MainViewModel vm)
            vm.CommitProjectRename(tb.Text);
    }

    private void ProjectNameBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (sender is not TextBox tb || DataContext is not MainViewModel vm)
            return;

        if (e.Key is Key.Return or Key.Enter)
        {
            vm.CommitProjectRename(tb.Text);
            e.Handled = true;
        }
        else if (e.Key == Key.Escape)
        {
            vm.CommitProjectRename(string.Empty);
            e.Handled = true;
        }
    }

    private void PhaseNameBox_LostFocus(object sender, RoutedEventArgs e)
    {
        if (sender is TextBox tb && tb.DataContext is FaseViewModel faseVm)
            faseVm.CommitRename(tb.Text);
    }

    private void PhaseNameBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (sender is not TextBox tb || tb.DataContext is not FaseViewModel faseVm)
            return;

        if (e.Key is Key.Return or Key.Enter)
        {
            faseVm.CommitRename(tb.Text);
            e.Handled = true;
        }
        else if (e.Key == Key.Escape)
        {
            faseVm.CommitRename(string.Empty); // triggers revert in CommitRename
            e.Handled = true;
        }
    }
}
