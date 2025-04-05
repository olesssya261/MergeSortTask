using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using MergeSort.Service;

namespace MergeSort.ViewModel
{
    public partial class AboutViewModel : ObservableObject
    {
        [ObservableProperty]
        private string sourceString = PathService.GetCurentFolderPath("Resources\\About.html");
        public AboutViewModel() { }
    }
}
