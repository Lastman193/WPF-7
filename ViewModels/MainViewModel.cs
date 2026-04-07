using ComicVerse.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Windows;
using System.Windows.Input;

namespace ComicVerse.ViewModels
{
    public class UserSettings : INotifyPropertyChanged
    {
        private string _name = "Гость";
        private string _selectedTheme = "Light";

        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(nameof(Name)); }
        }
        public string SelectedTheme
        {
            get => _selectedTheme;
            set { _selectedTheme = value; OnPropertyChanged(nameof(SelectedTheme)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public class MainViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<ComicItem> _allItems = new ObservableCollection<ComicItem>();
        private object _currentView;
        private bool _isAdminMode;
        private UserSettings _currentUser = new UserSettings();
        private double _minPrice = 0;

        // Стек для реализации Undo/Redo (навигации)
        private Stack<object> _backStack = new Stack<object>();
        private Stack<object> _forwardStack = new Stack<object>();
        private bool _isNavigating = false;

        public ICommand RemoveCommand { get; }

        public MainViewModel()
        {
            RemoveCommand = new RelayCommand(obj => {
                if (obj is ComicItem item)
                {
                    _allItems.Remove(item);
                    OnPropertyChanged(nameof(ItemsView));
                    SaveData();
                }
            });

            LoadData();
            // Инициализируем начальную страницу
            _currentView = new CatalogViewModel(this);
        }

        // --- СВОЙСТВА ДЛЯ ФИЛЬТРАЦИИ ---
        public double MinPrice
        {
            get => _minPrice;
            set
            {
                _minPrice = value;
                OnPropertyChanged(nameof(MinPrice));
                OnPropertyChanged(nameof(ItemsView)); // Оповещаем View, что список изменился
            }
        }

        // Именно к этому свойству должен быть привязан ItemsSource в CatalogView.xaml
        public IEnumerable<ComicItem> ItemsView
        {
            get
            {
                if (_allItems == null) return new List<ComicItem>();
                return _allItems.Where(x => x.Price >= _minPrice).ToList();
            }
        }

        public ObservableCollection<ComicItem> AllItems
        {
            get => _allItems;
            set
            {
                _allItems = value;
                OnPropertyChanged(nameof(AllItems));
                OnPropertyChanged(nameof(ItemsView));
            }
        }

        // --- НАВИГАЦИЯ И СОСТОЯНИЕ ---
        public object CurrentView
        {
            get => _currentView;
            set { _currentView = value; OnPropertyChanged(nameof(CurrentView)); }
        }

        public bool IsAdminMode
        {
            get => _isAdminMode;
            set { _isAdminMode = value; OnPropertyChanged(nameof(IsAdminMode)); }
        }

        public UserSettings CurrentUser
        {
            get => _currentUser;
            set { _currentUser = value; OnPropertyChanged(nameof(CurrentUser)); }
        }

        public bool CanGoBack => _backStack.Count > 0;
        public bool CanRedo => _forwardStack.Count > 0;

        public void Navigate(object view)
        {
            if (_isNavigating) return;
            if (_currentView != null) _backStack.Push(_currentView);
            _forwardStack.Clear();
            CurrentView = view;
            UpdateNavStatus();
        }

        public void GoBack()
        {
            if (_backStack.Count > 0)
            {
                _isNavigating = true;
                _forwardStack.Push(_currentView);
                CurrentView = _backStack.Pop();
                _isNavigating = false;
                UpdateNavStatus();
            }
        }

        public void Redo()
        {
            if (_forwardStack.Count > 0)
            {
                _isNavigating = true;
                _backStack.Push(_currentView);
                CurrentView = _forwardStack.Pop();
                _isNavigating = false;
                UpdateNavStatus();
            }
        }

        private void UpdateNavStatus()
        {
            OnPropertyChanged(nameof(CanGoBack));
            OnPropertyChanged(nameof(CanRedo));
        }

        public void AddItem(ComicItem item)
        {
            _allItems.Add(item);
            OnPropertyChanged(nameof(ItemsView));
            SaveData();
        }

        // --- РАБОТА С ФАЙЛАМИ ---
        public void SaveData()
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                };
                File.WriteAllText("items.json", JsonSerializer.Serialize(_allItems, options));
                File.WriteAllText("settings.json", JsonSerializer.Serialize(_currentUser, options));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка сохранения: " + ex.Message);
            }
        }

        private void LoadData()
        {
            try
            {
                if (File.Exists("items.json"))
                {
                    var json = File.ReadAllText("items.json");
                    var list = JsonSerializer.Deserialize<ObservableCollection<ComicItem>>(json);
                    if (list != null) _allItems = list;
                }

                if (File.Exists("settings.json"))
                {
                    var json = File.ReadAllText("settings.json");
                    var settings = JsonSerializer.Deserialize<UserSettings>(json);
                    if (settings != null) _currentUser = settings;
                }
            }
            catch
            {
                _allItems = new ObservableCollection<ComicItem>();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public class CatalogViewModel
    {
        public MainViewModel Main { get; }
        public CatalogViewModel(MainViewModel main) { Main = main; }
    }
}