using CommunityToolkit.Mvvm.ComponentModel;
using MergeSort.Service;

namespace MergeSort.ViewModel
{
    public partial class AboutViewModel : ObservableObject
    {
        // Свойство, которое хранит путь к HTML файлу с информацией "О программе"
        [ObservableProperty]
        private string sourceString = PathService.GetCurentFolderPath("Resources\\About.html");
        public AboutViewModel() { }
    }
}
