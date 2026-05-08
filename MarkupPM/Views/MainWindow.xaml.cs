using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
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
        Closing += MainWindow_Closing;
        Loaded += (_, _) =>
        {
            ApplyThemeDependentBrushes();
            App.OpenPendingFile();
        };
    }

    private void ThemeToggle_Click(object sender, RoutedEventArgs e)
    {
        var paletteHelper = new PaletteHelper();
        var theme = paletteHelper.GetTheme();
        var isDark = theme.GetBaseTheme() == BaseTheme.Dark;
        theme.SetBaseTheme(isDark ? BaseTheme.Light : BaseTheme.Dark);
        paletteHelper.SetTheme(theme);
        ApplyThemeDependentBrushes();
    }

    private void ApplyThemeDependentBrushes()
    {
        var paletteHelper = new PaletteHelper();
        var theme = paletteHelper.GetTheme();
        var isDark = theme.GetBaseTheme() == BaseTheme.Dark;
        var brushKey = isDark ? "PrimaryHueLightBrush" : "PrimaryHueMidBrush";

        if (TryFindResource(brushKey) is Brush brush)
            Resources["ActionAccentBrush"] = brush;
    }

    private void MainWindow_Closing(object? sender, CancelEventArgs e)
    {
        if (DataContext is MainViewModel vm && !vm.ConfirmarDescartarCambios())
            e.Cancel = true;
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

    private void RenameBox_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (sender is TextBox tb && tb.Visibility == Visibility.Visible)
        {
            tb.Dispatcher.InvokeAsync(() =>
            {
                tb.Focus();
                tb.SelectAll();
            }, System.Windows.Threading.DispatcherPriority.Input);
        }
    }

    private void PhaseNameBox_LostFocus(object sender, RoutedEventArgs e)
    {
        if (sender is TextBox tb && tb.DataContext is FaseViewModel faseVm)
        {
            faseVm.CommitRename(tb.Text);
            Keyboard.ClearFocus();
        }
    }

    private void PhaseNameBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (sender is not TextBox tb || tb.DataContext is not FaseViewModel faseVm)
            return;

        if (e.Key is Key.Return or Key.Enter)
        {
            faseVm.CommitRename(tb.Text);
            Keyboard.ClearFocus();
            e.Handled = true;
        }
        else if (e.Key == Key.Escape)
        {
            faseVm.CommitRename(string.Empty); // triggers revert in CommitRename
            Keyboard.ClearFocus();
            e.Handled = true;
        }
    }
}
