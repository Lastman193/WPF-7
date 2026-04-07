using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ComicVerse.Models
{
    public class ComicItem : INotifyPropertyChanged
    {
        private string _shortName;
        private string _category;
        private double _price;
        private string _description;
        private List<string> _imagePaths = new List<string>();

        public string ShortName
        {
            get => _shortName;
            set { _shortName = value; OnPropertyChanged(); }
        }

        public string Category
        {
            get => _category;
            set { _category = value; OnPropertyChanged(); }
        }

        public double Price
        {
            get => _price;
            set { _price = value; OnPropertyChanged(); }
        }

        public string Description
        {
            get => _description;
            set { _description = value; OnPropertyChanged(); }
        }

        public List<string> ImagePaths
        {
            get => _imagePaths;
            set { _imagePaths = value; OnPropertyChanged(); }
        }

        public bool IsExpensive => Price > 100;


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}