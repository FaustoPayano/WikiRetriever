using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WikiRetriever.Annotations;

namespace WikiRetriever.Model {
    class SearchTermModel : INotifyPropertyChanged {
        private string _club;
        private string _division;
        private string _nation;
        private string _wikipediaURL;

        public string Club {
            get { return _club; }
            set {
                _club = value;
                OnPropertyChanged(nameof(Club));
            }
        }

        public string Division {
            get { return _division;}
            set {
                _division = value;
                OnPropertyChanged(nameof(Division));
            }
        }

        public string Nation {
            get { return _nation; }
            set {
                _nation = value;
                OnPropertyChanged(nameof(Nation));
            }
        }

        public string WikipediaURL {
            get { return _wikipediaURL; }
            set {
                _wikipediaURL = value;
                OnPropertyChanged(nameof(WikipediaURL));
            }
        }

        public override string ToString() {
            return $"{Club}, {Division} {Nation}";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
