using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using CJT;

namespace ConnectToExcelOLEDB {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            ComboBox.DisplayMemberPath = "Job number";
            UpdateGrids();
            //SUBSCRIBE
            (DataContext as MainVM).DbContext.TransactionTableChanged += TransactionTableChanged;
            //(DataContext as MainVM).PartClassVM.ShowMessageEvent += ShowMessage;//Uncomment if want messagebox
            Closing += Close_Click;
        }

        public void Close_Click(object sender, EventArgs e) {
            (DataContext as MainVM).SaveState();
        }

        public void CommitTransaction_Click(object sender, EventArgs e) {
            (DataContext as MainVM).NewTransactionVM.CommitTransactionIfValid();
            //UpdateGrids();
        }

        public void SearchContents_Click(object sender, EventArgs e) {
            (DataContext as MainVM).Search();
            UpdateGrids();
        }

        public void SearchTransactions_Click(object sender, EventArgs e) {
            (DataContext as MainVM).Search();
            UpdateGrids();
        }

        public void ShowMessage(object sender, MessageEventArgs e) {
            MessageBox.Show(e.Message, "Note", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void TransactionTableChanged(object sender, EventArgs e) {
            (DataContext as MainVM).UpdateDataTables();
            UpdateGrids();
        }

        public void DoTheThing_Click(object sender, EventArgs e) {
            (DataContext as MainVM).DoTheThing();
        }

        public void Save_Click(object sender, EventArgs e) {
            (DataContext as MainVM).DbContext.SaveSettings();
        }

        public void UpdateGrids() {
            //ContentsGrid.ItemsSource = (DataContext as MainVM).PartClassVM.DataTable.DefaultView;
            //TransactionGrid.ItemsSource = (DataContext as MainVM).TransactionVM.DataTable.DefaultView;
            //NewTransactionGrid.ItemsSource = (DataContext as MainVM).NewTransactionVM.DataTable.DefaultView;
            //AHAH! perhaps if I make DefaultView fire a NOTIFYPropChanged event, THEN binding would work.
            //NOTE: this does work, BUT remember, have to GET NEW DATA FROM sql of course!
            ComboBox.ItemsSource = (DataContext as MainVM).JobNumbers.DefaultView;
            ComboBox.Text = (DataContext as MainVM).NewTransactionVM.JobNumber.ToString();
            //DataContext and DataSource might also work in place of ItemsSource?
        }

        private void dataGrid_AutoGeneratingColumn(object sender, 
            DataGridAutoGeneratingColumnEventArgs e) {
            if (e.PropertyType == typeof(string)) {
                DataGridTemplateColumn comboColumn = new DataGridTemplateColumn();
                string propName = e.PropertyName;
                comboColumn.Header = propName;
                comboColumn.CellTemplate = (DataTemplate)Resources["ComboBoxDataTemplate"];
                //NOTE: was the problem, that this cell template gets WIPED out when set visual tree?
                //Yes I think it was. Could check it.
                //ABOVE is almost pointless, since gets overwritten.
                FrameworkElementFactory comboBoxFactory = new FrameworkElementFactory(typeof(EditableComboBox));
                List<string> list = (DataContext as MainVM).ContentsVM.DataTable.AsEnumerable()
                    .Select(row => row[propName].ToString()).Distinct().ToList<string>();
                list = new List<string>() { "pp", "ss", propName};
                comboBoxFactory.SetValue(ComboBox.ItemsSourceProperty, list);
                //PROBLEM! It is setting ALL columns to have same itemsource as last column!
                //HELL I don't know how to fix this quickly. Find a work around! WORK AROUND!
                //var binding = new Binding(propName);
                //comboBoxFactory.SetBinding(ComboBox.TextProperty, binding);
                comboColumn.CellTemplate.VisualTree = comboBoxFactory;
                e.Column = comboColumn;
                //REASON I want to do this dynamically,
                //IS because I want each column to bind to a different PROPERTY of the row.
            }
        } //This won't get called anymore, since turned autogenerate off.

        public List<string> getList(string propertyName) {
            return (DataContext as MainVM).ContentsVM.DataTable.AsEnumerable()
                    .Select(row => row[propertyName].ToString()).Distinct().ToList<string>();
        }

    }
}
