using System.Windows;
using ComicVerse.ViewModels;

namespace ComicVerse.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }

        private void GoToCatalog_Click(object sender, object e)
        {
            if (DataContext is MainViewModel vm)
                vm.Navigate(new CatalogViewModel(vm));
        }

        private void BackNav_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainViewModel vm) vm.GoBack();
        }

        private void Redo_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainViewModel vm) vm.Redo();
        }

        private void OpenPersonal_Click(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is MainViewModel vm)
            {
                PersonalWindow win = new PersonalWindow(vm);
                win.Owner = this;
                win.ShowDialog();
            }
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainViewModel vm)
            {
                var newItem = new ComicVerse.Models.ComicItem { ShortName = "New Item", Price = 0 };
                vm.AddItem(newItem);
                vm.Navigate(newItem);
            }
        }
    }
}