using System.Windows;

namespace MergeSort;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private BootStrapper _bootStrapper;
    protected override void OnStartup(StartupEventArgs e)
    {
        _bootStrapper = new BootStrapper();
        MainWindow = _bootStrapper.Run();
        MainWindow.Show();

        base.OnStartup(e);
    }

    protected override void OnExit(ExitEventArgs e)
    {
        base.OnExit(e);
    }
}

