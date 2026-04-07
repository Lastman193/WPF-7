using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ComicVerse.Models;
using ComicVerse.ViewModels;

namespace ComicVerse.Views
{
    public partial class CatalogView : UserControl
    {
        public CatalogView()
        {
            InitializeComponent();
        }

        private void Card_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement elem && elem.DataContext is ComicItem item)
            {
                var mainVM = (MainViewModel)Application.Current.MainWindow.DataContext;
                mainVM.Navigate(item);
            }
        }
    }
}