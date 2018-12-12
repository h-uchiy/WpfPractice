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
        private string _text;

        public ViewModel()
        {
            _searchString = "SearchString";
            _filePath = "FilePath";
            _searchResult = Enumerable.Empty<MatchedLine>();
            SearchCommand = new DelegateCommand(
                () => Search(),
                () => !string.IsNullOrEmpty(SearchString) && !string.IsNullOrEmpty(this.Text))
                .ObservesProperty(() => this.SearchString)
                .ObservesProperty(() => this.Text);
        }

        public string SearchString
        {
            get => _searchString;
            set
            {
                if (_searchString != value)
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

                    if(File.Exists(_filePath))
                    {
                        this.Text = File.ReadAllText(_filePath);
                    }
                }
            }
        }

        public string Text
        {
            get => _text;
            set
            {
                if (_text != value)
                {
                    _text = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public IEnumerable<MatchedLine> SearchResult
        {
            get => _searchResult;
            set
            {
                if (_searchResult != value)
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
            IEnumerable<MatchedLine> MatchedLines()
            {
                int lineNo = 0;
                foreach (var line in this.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.None))
                {
                    lineNo++;
                    var m = Regex.Match(line, this.SearchString);
                    if (m.Success)
                    {
                        yield return new MatchedLine(lineNo, m.Index, line);
                    }
                }
            }

            this.SearchResult = MatchedLines();
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
