using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using WikiRetriever.ViewModel;

namespace WikiRetriever {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private MainWindowViewModel mainModel;
        private string listFilepath;
        private string _outputSaveFilePath;
        public MainWindow() {
            InitializeComponent();    
        }

        /// <summary>
        /// When user clicks on file input textbox, starts open file dialog to select input file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e) {
            //TODO: Add openfile dialog so user can select file to use as input.
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true) {
                listFilepath = openFileDialog.FileName;
                InputListBox.DataContext = mainModel;
                CreateBackgroundWorker();
            }
        }

        private void CreateBackgroundWorker() {
            var backGroundWorker = new BackgroundWorker();
            backGroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ViewModelEstablished);
            backGroundWorker.DoWork += new DoWorkEventHandler(EstablishViewModel);
            backGroundWorker.RunWorkerAsync(_outputSaveFilePath);
        }
        /// <summary>
        /// Creates the view model with information from the respective controls.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EstablishViewModel(object sender, DoWorkEventArgs e) {
            string tempSavePath = string.Empty;
            if (string.IsNullOrEmpty(sender as string)) {
                tempSavePath = AppDomain.CurrentDomain.BaseDirectory;
                mainModel = new MainWindowViewModel(listFilepath, tempSavePath);
            }
            else {
                mainModel = new MainWindowViewModel(listFilepath, _outputSaveFilePath);
            }
        }

        /// <summary>
        /// Background worker completed event for establishing DataContexts for the different controls.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewModelEstablished(object sender, RunWorkerCompletedEventArgs e) {
            InputListBox.DataContext = mainModel;
            SearchTermsListBox.ItemsSource = mainModel.SearchTerms;
            CountBlock.DataContext = mainModel;
            LeftToCountBlock.DataContext = mainModel;
            SaveOutputBox.DataContext = mainModel;
        }

        /// <summary>
        /// An alternative event for establishing the viewmodel off of the load file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e) {
            UIElement_OnMouseDown(sender, e as MouseButtonEventArgs);
        }

        /// <summary>
        /// Initiates analysis of search terms.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchAllTerms_OnClick(object sender, RoutedEventArgs e) {
            var analysisWorker = new BackgroundWorker();
            analysisWorker.DoWork += new DoWorkEventHandler(SearchTerms);
            analysisWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(completedSearchTerms);
            analysisWorker.RunWorkerAsync();
        }

        private void completedSearchTerms(object sender, RunWorkerCompletedEventArgs e) {
        }

        /// <summary>
        /// Begins retrieval of wikipedia URLS for each search term.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchTerms(object sender, DoWorkEventArgs e) {
            mainModel.GetArticle();
        }

        /// <summary>
        /// Establishes the save/output file path.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveLocation_OnClick(object sender, RoutedEventArgs e) {
            var saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true) {
                mainModel.SaveFilePath = saveFileDialog.FileName;
            }
        }
    }
}
