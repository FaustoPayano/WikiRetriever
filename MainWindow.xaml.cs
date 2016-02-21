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
        private string ouputSaveFilePath;
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
                var backGroundWorker = new BackgroundWorker();
                backGroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ViewModelEstablished);
                backGroundWorker.DoWork += new DoWorkEventHandler(EstablishViewModel);
                backGroundWorker.RunWorkerAsync();
            }
        }

        private void EstablishViewModel(object sender, DoWorkEventArgs e) {
            mainModel = new MainWindowViewModel(listFilepath);
        }

        private void ViewModelEstablished(object sender, RunWorkerCompletedEventArgs e) {
            InputListBox.DataContext = mainModel;
            SearchTermsListBox.ItemsSource = mainModel.SearchTerms;
            CountBlock.DataContext = mainModel;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e) {
            UIElement_OnMouseDown(sender, e as MouseButtonEventArgs);
        }
    }
}
