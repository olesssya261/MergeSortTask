using MergeSort.Model.ObservableModels;
using MergeSort.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MergeSort
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Конструктор окна, инициализирует данные для взаимодействия с ViewModel
        /// </summary>
        /// <param name="mainViewModel">Экземпляр MainViewModel для привязки данных</param>
        public MainWindow(MainViewModel mainViewModel)
        {
            // Подписываемся на событие открытия массива в ViewModel
            mainViewModel.openArray += OpenArrayHendler;

            // Устанавливаем DataContext для привязки данных
            DataContext = mainViewModel;

            // Инициализация компонентов окна
            InitializeComponent();
        }

        /// <summary>
        /// Обработчик события открытия массива для сортировки.
        /// При его активации переключаем вкладку на вкладку сортировки.
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргументы события</param>
        private void OpenArrayHendler(object? sender, EventArgs e)
        {
            // Переключаем на вкладку сортировки
            MenuTab.SelectedItem = SortTab;
        }

        /// <summary>
        /// Обработчик двойного клика по элементу в ListView.
        /// При двойном клике выполняется открытие массива для редактирования.
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргументы события</param>
        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Проверяем, является ли sender элементом ListViewItem
            if (sender is ListViewItem item && item.DataContext is SortArrayObservableModel model)
            {
                // Получаем текущий экземпляр ViewModel из DataContext
                if (DataContext is MainViewModel vm)
                {
                    // Выполняем команду открытия массива для сортировки
                    vm.OpenArrayCommand.Execute(model);

                    // Останавливаем дальнейшую обработку события
                    e.Handled = true;
                }
            }
        }

        /// <summary>
        /// Обработчик изменения выбранной вкладки в TabControl.
        /// При выборе вкладки "База данных" вызывается команда для загрузки данных из базы.
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргументы события</param>
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Получаем экземпляр MainViewModel из DataContext
            if (DataContext is MainViewModel vm)
            {
                // Проверяем, что событие вызвано именно из TabControl и выбрана вкладка "DatabaseTab"
                if (e.Source is TabControl tabControl &&
                tabControl.SelectedItem == DatabaseTab)
                {
                    // Выполняем команду для загрузки данных из базы данных
                    vm.OpenDbMenuCommand?.Execute(null);
                }
            }
        }
    }
}
