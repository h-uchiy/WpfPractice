using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Prism.Commands;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfApp1
{
    internal class ViewModel : INotifyPropertyChanged
    {
        private string _searchString;
        private string _filePath;
        private string _searchResult;

        public ViewModel()
        {
            _searchString = "SearchString";
            _filePath = "FilePath";
            _searchResult = string.Empty;
            SearchCommand = new DelegateCommand(
                () => Search(),
                () => string.IsNullOrEmpty(SearchString) && File.Exists(FilePath));
        }

        public string SearchString
        {
            get => _searchString;
            set
            {
                if(_searchString != value)
                {
                    NotifyPropertyChanged();
                    _searchString = value;
                }
            }
        }

        public string FilePath
        {
            get => _filePath;
            set
            {
                if (_filePath != value)
                {
                    NotifyPropertyChanged();
                    _filePath = value;
                }
            }
        }

        public string SearchResult
        {
            get => _searchResult;
            set
            {
                if(_searchResult != value)
                {
                    NotifyPropertyChanged();
                    _searchResult = value;
                }
            }
        }

        public DelegateCommand SearchCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null && propertyName != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void Search()
        {
        }
    }
}
