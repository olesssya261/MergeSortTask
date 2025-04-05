using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MergeSort.Model.ObservableModels;
using MergeSort.ViewModel;

namespace MergeSort;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow(MainViewModel mainViewModel)
    {
        mainViewModel.openArray += OpenArrayHendler;
        DataContext = mainViewModel;
        InitializeComponent();
    }

    private void OpenArrayHendler(object? sender, EventArgs e)
    {
        MenuTab.SelectedItem = SortTab;
    }
    private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (sender is ListViewItem item && item.DataContext is SortArrayObservableModel model)
        {
            if (DataContext is MainViewModel vm)
            {
                vm.OpenArrayCommand.Execute(model);
                e.Handled = true; // Останавливаем дальнейшую обработку события
            }
        }
    }
    private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (DataContext is MainViewModel vm)
        {
            if (e.Source is TabControl tabControl &&
            tabControl.SelectedItem == DatabaseTab)
            {
                vm.OpenDbMenuCommand?.Execute(null);
            }

        }
    }
}