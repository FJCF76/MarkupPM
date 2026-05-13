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

        // Semantic surface tokens adapted for dark mode
        if (isDark)
        {
            Application.Current.Resources["SurfaceToolbarBrush"]  = new SolidColorBrush(Color.FromRgb(30, 30, 36));
            Application.Current.Resources["SurfaceSidebarBrush"]  = new SolidColorBrush(Color.FromRgb(24, 24, 30));
            Application.Current.Resources["SurfaceCardBrush"]     = new SolidColorBrush(Color.FromRgb(35, 35, 45));
            Application.Current.Resources["SurfaceRowHoverBrush"] = new SolidColorBrush(Color.FromArgb(40, 103, 58, 183));
            Application.Current.Resources["SurfaceSelectedBrush"] = new SolidColorBrush(Color.FromArgb(60, 103, 58, 183));
            Application.Current.Resources["BorderSubtleBrush"]    = new SolidColorBrush(Color.FromRgb(50, 50, 62));
            Application.Current.Resources["BorderMediumBrush"]    = new SolidColorBrush(Color.FromRgb(70, 70, 85));
            Application.Current.Resources["TextPrimaryBrush"]     = new SolidColorBrush(Color.FromRgb(240, 240, 248));
            Application.Current.Resources["TextSecondaryBrush"]   = new SolidColorBrush(Color.FromRgb(180, 180, 200));
            Application.Current.Resources["TextMutedBrush"]       = new SolidColorBrush(Color.FromRgb(120, 120, 145));
            Application.Current.Resources["AccentLightBrush"]     = new SolidColorBrush(Color.FromArgb(50, 149, 117, 205));
            // Status chips — dark-friendly
            Application.Current.Resources["StatusDoneBg"]         = new SolidColorBrush(Color.FromArgb(45, 76, 175, 80));
            Application.Current.Resources["StatusDoneFg"]         = new SolidColorBrush(Color.FromRgb(129, 199, 132));
            Application.Current.Resources["StatusInProgressBg"]   = new SolidColorBrush(Color.FromArgb(45, 33, 150, 243));
            Application.Current.Resources["StatusInProgressFg"]   = new SolidColorBrush(Color.FromRgb(100, 181, 246));
            Application.Current.Resources["StatusPendingBg"]      = new SolidColorBrush(Color.FromArgb(30, 158, 158, 158));
            Application.Current.Resources["StatusPendingFg"]      = new SolidColorBrush(Color.FromRgb(189, 189, 189));
            // Priority chips
            Application.Current.Resources["PriorityHighBg"]       = new SolidColorBrush(Color.FromArgb(45, 244, 67, 54));
            Application.Current.Resources["PriorityHighFg"]       = new SolidColorBrush(Color.FromRgb(239, 154, 154));
            Application.Current.Resources["PriorityMedBg"]        = new SolidColorBrush(Color.FromArgb(45, 255, 152, 0));
            Application.Current.Resources["PriorityMedFg"]        = new SolidColorBrush(Color.FromRgb(255, 183, 77));
            Application.Current.Resources["PriorityLowBg"]        = new SolidColorBrush(Color.FromArgb(45, 76, 175, 80));
            Application.Current.Resources["PriorityLowFg"]        = new SolidColorBrush(Color.FromRgb(129, 199, 132));
            // Avatar
            Application.Current.Resources["AvatarText"]           = new SolidColorBrush(Color.FromRgb(206, 184, 243));
            // Danger / dialogs
            Application.Current.Resources["DangerBrush"]          = new SolidColorBrush(Color.FromRgb(239, 154, 154));
            Application.Current.Resources["DangerLightBrush"]     = new SolidColorBrush(Color.FromArgb(45, 244, 67, 54));
            Application.Current.Resources["SurfacePageBrush"]     = new SolidColorBrush(Color.FromRgb(30, 30, 36));
        }
        else
        {
            // Restore light-mode defaults (matching Styles.xaml initial values)
            Application.Current.Resources["SurfaceToolbarBrush"]  = new SolidColorBrush(Color.FromRgb(250, 250, 250));
            Application.Current.Resources["SurfaceSidebarBrush"]  = new SolidColorBrush(Color.FromRgb(245, 245, 245));
            Application.Current.Resources["SurfaceCardBrush"]     = new SolidColorBrush(Colors.White);
            Application.Current.Resources["SurfaceRowHoverBrush"] = new SolidColorBrush(Color.FromRgb(243, 238, 249));
            Application.Current.Resources["SurfaceSelectedBrush"] = new SolidColorBrush(Color.FromRgb(237, 231, 246));
            Application.Current.Resources["BorderSubtleBrush"]    = new SolidColorBrush(Color.FromRgb(224, 224, 224));
            Application.Current.Resources["BorderMediumBrush"]    = new SolidColorBrush(Color.FromRgb(189, 189, 189));
            Application.Current.Resources["TextPrimaryBrush"]     = new SolidColorBrush(Color.FromRgb(26, 26, 46));
            Application.Current.Resources["TextSecondaryBrush"]   = new SolidColorBrush(Color.FromRgb(92, 92, 122));
            Application.Current.Resources["TextMutedBrush"]       = new SolidColorBrush(Color.FromRgb(158, 158, 158));
            Application.Current.Resources["AccentLightBrush"]     = new SolidColorBrush(Color.FromRgb(237, 231, 246));
            // Status chips
            Application.Current.Resources["StatusDoneBg"]         = new SolidColorBrush(Color.FromRgb(232, 245, 233));
            Application.Current.Resources["StatusDoneFg"]         = new SolidColorBrush(Color.FromRgb(46, 125, 50));
            Application.Current.Resources["StatusInProgressBg"]   = new SolidColorBrush(Color.FromRgb(227, 242, 253));
            Application.Current.Resources["StatusInProgressFg"]   = new SolidColorBrush(Color.FromRgb(21, 101, 192));
            Application.Current.Resources["StatusPendingBg"]      = new SolidColorBrush(Color.FromRgb(245, 245, 245));
            Application.Current.Resources["StatusPendingFg"]      = new SolidColorBrush(Color.FromRgb(97, 97, 97));
            // Priority chips
            Application.Current.Resources["PriorityHighBg"]       = new SolidColorBrush(Color.FromRgb(255, 235, 238));
            Application.Current.Resources["PriorityHighFg"]       = new SolidColorBrush(Color.FromRgb(198, 40, 40));
            Application.Current.Resources["PriorityMedBg"]        = new SolidColorBrush(Color.FromRgb(255, 243, 224));
            Application.Current.Resources["PriorityMedFg"]        = new SolidColorBrush(Color.FromRgb(230, 81, 0));
            Application.Current.Resources["PriorityLowBg"]        = new SolidColorBrush(Color.FromRgb(232, 245, 233));
            Application.Current.Resources["PriorityLowFg"]        = new SolidColorBrush(Color.FromRgb(46, 125, 50));
            Application.Current.Resources["AvatarText"]           = new SolidColorBrush(Color.FromRgb(103, 58, 183));
            // Danger / dialogs
            Application.Current.Resources["DangerBrush"]          = new SolidColorBrush(Color.FromRgb(198, 40, 40));
            Application.Current.Resources["DangerLightBrush"]     = new SolidColorBrush(Color.FromRgb(255, 235, 238));
            Application.Current.Resources["SurfacePageBrush"]     = new SolidColorBrush(Colors.White);
        }
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
