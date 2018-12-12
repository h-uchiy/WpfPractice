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
                foreach(var line in File.ReadLines(this.FilePath))
                {
                    if(Regex.IsMatch(line, this.SearchString))
                    {
                        yield return line;
                    }
                }
            }

            this.SearchResult = MatchedString().Aggregate((a, b) => a + "\n" + b);
        }
    }

    class MatchedLine
    {
        public MatchedLine(int lineNo, int linePosition, string line)
        {
            LineNo = lineNo;
            LinePosition = linePosition;
            Line = line;
        }

        int LineNo { get; }
        int LinePosition { get; }
        string Line { get; }
    }
}
