using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MergeSort.ViewModel;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;

namespace MergeSort;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public event EventHandler openDbMenu;
    public MainWindow(MainViewModel mainViewModel)
    {
        DataContext = mainViewModel;
        InitializeComponent();
    }

    private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (DataContext is MainViewModel vm)
        {
            vm.OpenDbMenuCommand?.Execute(null);

            if (e.Source is TabControl tabControl &&
            tabControl.SelectedItem == DatabaseTab)
            {
                
             vm.OpenDbMenuCommand?.Execute(null);
            }
            
        }
    }
}