using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Prism.Commands;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace WpfApp
{
    public class ViewModel : INotifyPropertyChanged
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
                () => !string.IsNullOrEmpty(SearchString) && File.Exists(FilePath))
                .ObservesProperty(() => SearchString)
                .ObservesProperty(() => FilePath);
        }

        public string SearchString
        {
            get => _searchString;
            set
            {
                if(_searchString != value)
                {
                    _searchString = value;
                    NotifyPropertyChanged();
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
                    _filePath = value;
                    NotifyPropertyChanged();
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
                    _searchResult = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public DelegateCommand SearchCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Search()
        {
            IEnumerable<string> MatchedString()
            {
                var content = File.ReadAllText(this.FilePath);
                foreach(Match m in Regex.Matches(content, this.SearchString, RegexOptions.Multiline))
                {
                    yield return m.Value;
                }
            }

            this.SearchResult = MatchedString().Aggregate((a, b) => a + "\n" + b);
        }
    }
}
