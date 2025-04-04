using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MergeSort.ViewModel;

namespace MergeSort
{
    public class BootStrapper
    {
        public BootStrapper(){}
        public Window Run()
        {
            return new MainWindow(new MainViewModel());
        }
    }
}
