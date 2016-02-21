﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using WikiRetriever.Annotations;
using WikiRetriever.Model;

namespace WikiRetriever.ViewModel {
    class MainWindowViewModel : INotifyPropertyChanged
    {
        private string _filePath;
        private FileInfo _file;

        public MainWindowViewModel(string filePath) {
            SearchTerms = GetSearchTermCollection(filePath);

        }


        /// <summary>
        /// Using file located at filepath, create the Observeable collection for MainWindowViewModel
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private ObservableCollection<SearchTermModel> GetSearchTermCollection(string filePath) {
            var reader = new StreamReader(System.IO.File.OpenRead(filePath));
            ObservableCollection<SearchTermModel> searchTermsObservableCollection = new ObservableCollection<SearchTermModel>();
            while (!reader.EndOfStream) {
                var line = reader.ReadLine();
                if (line != null) {
                    var results = line.Split(',');
                    searchTermsObservableCollection.Add(new SearchTermModel() {
                        Club = results[0],
                        Division = results[1],
                        Nation = results[3]
                    });
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

        private ObservableCollection<SearchTermModel> SearchTerms { get; set; } 
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}