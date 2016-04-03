using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using Jitbit.Utils;
using WikipediaNET;
using WikipediaNET.Objects;
using WikiRetriever.Annotations;
using WikiRetriever.Model;

namespace WikiRetriever.ViewModel {
    class MainWindowViewModel : INotifyPropertyChanged {
        private string _filePath;
        private string _saveFilePath;
        private FileInfo _file;
        private int _numberOfSearchTerms;
        private int _termsLeftToAnalyze;
        private int _numberOfErrors;

        public MainWindowViewModel(string filePath,string savePath) {
            SearchTerms = GetSearchTermCollection(filePath);
            this.FilePath = filePath;
            this.SaveFilePath = savePath;

        }


        /// <summary>
        /// Using file located at filepath, create the Observeable collection for MainWindowViewModel
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private ObservableCollection<SearchTermModel> GetSearchTermCollection(string filePath) {
            var reader = new StreamReader(System.IO.File.OpenRead(filePath));
            ObservableCollection<SearchTermModel> searchTermsObservableCollection =
                new ObservableCollection<SearchTermModel>();
            while (!reader.EndOfStream) {
                var line = reader.ReadLine();
                if (line != null && !line.Contains("club")) {
                    var results = line.Split(',');
                    searchTermsObservableCollection.Add(new SearchTermModel() {
                        Club = results[0],
                        Division = results[1],
                        Nation = results[2]
                    });

                    NumberOfSearchTerms = searchTermsObservableCollection.Count;
                    TermsLeftToAnalyze = searchTermsObservableCollection.Count;
                }
            }
            return searchTermsObservableCollection;
        }

        public string FilePath {
            get { return _filePath; }
            set {
                _filePath = value;
                OnPropertyChanged(nameof(FilePath));
            }
        }

        public FileInfo File {
            get { return _file; }
            set {
                _file = value;
                OnPropertyChanged(nameof(File));
            }
        }

        public int NumberOfSearchTerms {
            get { return _numberOfSearchTerms; }
            set {
                _numberOfSearchTerms = value;
                OnPropertyChanged(nameof(NumberOfSearchTerms));
            }
        }

        public int TermsLeftToAnalyze {
            get { return _termsLeftToAnalyze; }
            set {
                _termsLeftToAnalyze = value;
                OnPropertyChanged(nameof(TermsLeftToAnalyze));
            }
        }

        public string SaveFilePath {
            get { return _saveFilePath; }
            set {
                _saveFilePath = value;
                OnPropertyChanged(nameof(SaveFilePath));
            }
        }

        public int NumberOfErrors {
            get { return _numberOfErrors; }
            set {
                _numberOfErrors = value;
                OnPropertyChanged(nameof(NumberOfErrors));
            }
        }

        public ObservableCollection<SearchTermModel> SearchTerms { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


        }

        public void GetArticle() {
            var wiki = new Wikipedia();
            foreach (var term in SearchTerms) {
                QueryResult results = wiki.Search(term.Club);
                foreach (var possibleUrl in results.Search) {
                    if (possibleUrl.Title != null) {
                        if (possibleUrl.Title.ToLower() == (term.Club.ToLower())) {
                            term.WikipediaURL = possibleUrl.Url.AbsoluteUri;
                        }
                    }
                    else {
                        term.WikipediaURL = "No Valid URLS";
                    }
                    if (string.IsNullOrEmpty(term.WikipediaURL)) {
                        term.WikipediaURL = "No Valid  URLS";
                    }
                }
                TermsLeftToAnalyze--;
            }   

           SaveToCsv();
        }

        public void SaveToCsv() {
            var exportResults = new CsvExport();

            foreach (var entry in SearchTerms) {

                exportResults.AddRow();
                exportResults["Club"] = entry.Club;
                exportResults["Division"] = entry.Division;
                exportResults["Nation"] = entry.Nation;
                exportResults["Wikipedia URL"] = entry.WikipediaURL;
                


            }
            exportResults.ExportToFile(SaveFilePath);
        }
    }
}

