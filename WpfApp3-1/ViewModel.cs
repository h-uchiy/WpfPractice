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
        private IEnumerable<MatchedLine> _searchResult;

        public ViewModel()
        {
            _searchString = "SearchString";
            _filePath = "FilePath";
            _searchResult = Enumerable.Empty<MatchedLine>();
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

        public IEnumerable<MatchedLine> SearchResult
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
            this.SearchResult = MatchedLines;
        }

        private IEnumerable<MatchedLine> MatchedLines
        {
            get
            {
                int lineNo = 0;
                foreach (var line in File.ReadLines(this.FilePath))
                {
                    lineNo++;
                    var m = Regex.Match(line, this.SearchString);
                    if (m.Success)
                    {
                        yield return new MatchedLine(lineNo, m.Index, line);
                    }
                }
            }
        }
    }

    public class MatchedLine
    {
        public MatchedLine(int lineNo, int linePosition, string line)
        {
            LineNo = lineNo;
            LinePosition = linePosition;
            Line = line;
        }

        public int LineNo { get; }
        public int LinePosition { get; }
        public string Line { get; }
    }
}
