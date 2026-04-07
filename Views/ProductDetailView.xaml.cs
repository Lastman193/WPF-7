using Microsoft.Win32;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using ComicVerse.ViewModels;
using ComicVerse.Models;

namespace ComicVerse.Views
{
    public partial class ProductDetailView : UserControl
    {
        public ProductDetailView()
        {
            InitializeComponent();
        }

        private void ChangePhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog { Filter = "Images|*.jpg;*.png;*.jpeg" };
            if (dlg.ShowDialog() == true)
            {
                if (this.DataContext is ComicItem item)
                {
                    // Обновляем список путей (вызовет OnPropertyChanged в модели)
                    item.ImagePaths = new List<string> { dlg.FileName };
                }
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var mainVM = (MainViewModel)Application.Current.MainWindow.DataContext;
            mainVM.SaveData();
            mainVM.GoBack();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            var mainVM = (MainViewModel)Application.Current.MainWindow.DataContext;
            mainVM.GoBack();
        }
    }
}