using System.Windows;
using MergeSort.ViewModel;

namespace MergeSort
{
    public class BootStrapper
    {
        public BootStrapper() { }
        public Window Run()
        {
            return new MainWindow(new MainViewModel());
        }
    }
}
