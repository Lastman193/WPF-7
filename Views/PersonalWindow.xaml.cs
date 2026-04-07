using System.Windows;
using System.Windows.Controls;
using ComicVerse.ViewModels;

namespace ComicVerse.Views
{
    public partial class PersonalWindow : Window
    {
        private MainViewModel _vm;
        public PersonalWindow(MainViewModel vm)
        {
            InitializeComponent();
            _vm = vm;
            DataContext = _vm;
        }

        private void Theme_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (ThemeBox.SelectedItem is ComboBoxItem item && item.Tag != null)
                App.SelectTheme(item.Tag.ToString());
        }

        private void LangRu_Click(object sender, RoutedEventArgs e) => App.SelectCulture("ru");
        private void LangEn_Click(object sender, RoutedEventArgs e) => App.SelectCulture("en");

        private void SaveAndClose_Click(object sender, RoutedEventArgs e)
        {
            _vm.SaveData(); // Сохраняем имя пользователя и настройки в JSON
            this.Close();
        }
    }
}