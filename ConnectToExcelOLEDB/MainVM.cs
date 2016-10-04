using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using CJT;

namespace ConnectToExcelOLEDB {
    class MainVM : BaseClass {
        private DbType dbType;
        public DbType DbType {
            get { return dbType; }
            set { dbType = value; NotifyPropertyChanged("DbType"); }
        }
        public ObservableCollection<DbType> DbTypeOptions { get; set; }
        private DbContext dbContext;
        public DbContext DbContext {
            get { return dbContext; }
            set { dbContext = value; NotifyPropertyChanged("DbContext"); }
        }
        public ExcelContext ExcelContext { get; set; }
        public SqlContext SqlContext { get; set; }
        public EFContext EFContext { get; set; }
        public List<DbContext> DbContextOptions { get; set; }
        private string status = "Ready";
        public string Status {
            get { return status; }
            set { status = value; NotifyPropertyChanged("Status"); }
        }

        private string searchBar;
        public string SearchBar {
            get { return searchBar; }
            set { searchBar = value; NotifyPropertyChanged("SearchBar"); }
        }

        private DataTable jobNumbers;
        public DataTable JobNumbers {
            get { return jobNumbers; }
            set { jobNumbers = value; NotifyPropertyChanged("JobNumbers"); }
        }

        private TransactionsVM transactionVM;
        public TransactionsVM TransactionVM {
            get { return transactionVM; }
            set { transactionVM = value; NotifyPropertyChanged("TransactionVM"); }
        }

        private ICollection<ContentsVM> partClassVMs;
        public ICollection<ContentsVM> PartClassVMs {
            get { return partClassVMs; }
            set { partClassVMs = value; NotifyPropertyChanged("PartClassVMs"); }
        }

        private ContentsVM contentsVM;
        public ContentsVM ContentsVM {
            get { return contentsVM; }
            set { contentsVM = value; NotifyPropertyChanged("ContentsVM"); }
        }

        private NewTransactionVM newTransactionVM;
        public NewTransactionVM NewTransactionVM {
            get { return newTransactionVM; }
            set { newTransactionVM = value; NotifyPropertyChanged("NewTransactionVM"); }
        }

        public MainVM() {
            ExcelContext = new ExcelContext();
            SqlContext = new SqlContext();
            EFContext = new EFContext();
            //EFContext.Database.Create();
            //DBContextOptions
            DbContextOptions = new List<DbContext>();
            DbContextOptions.Add(ExcelContext);
            DbContextOptions.Add(SqlContext);
            DbContextOptions.Add(EFContext);
            DbContext = DbContextOptions.First(); //Start with Excel, change when needed.
            //DbTypeOptions
            DbTypeOptions = new ObservableCollection<DbType>();
            DbTypeOptions.Add(DbType.Excel);
            DbTypeOptions.Add(DbType.SQLEntityFramework);
            DbType = DbType.SQLEntityFramework;
            //THE REST
            CreateDataListVMs();
            UpdateDataTables();
            //SUBSCRIBE
            ContentsVM.ShowMessageEvent += MainVM_ShowMessage;

            //ONETIME
            //ExcelContext.AddColumn();
            NewTransactionVM.JobNumber = (int) Properties.Settings.Default["JobNumber"];
        }

        public void CreateDataListVMs() {
            TransactionVM = new DataListVMs.TransactionsVM(DbContext);
            ContentsVM = new DataListVMs.ContentsVM(DbContext);
            NewTransactionVM = new DataListVMs.NewTransactionVM(DbContext, ContentsVM);
        }

        public void CreateDataTableVMs() {
            TransactionVM = new DataTableVMs.TransactionsVM(DbContext);
            ContentsVM = new DataTableVMs.ContentsVM(DbContext);
            NewTransactionVM = new DataTableVMs.NewTransactionVM(DbContext, ContentsVM);
        }

        public void DoTheThing() {
            //Do nought
            string yo = SqlContext.GetConnectionString();
            DataTable dt = SqlContext.GetDataTable("SELECT * FROM [Parts]");
        }

        public void SaveState() {
            Properties.Settings.Default["JobNumber"] = NewTransactionVM.JobNumber;
            Properties.Settings.Default.Save(); // Saves settings in application configuration file
        }

        public void Search() {
            ContentsVM.Search(SearchBar);
            TransactionVM.Search(SearchBar);
        }

        public void UpdateDataTables() {
            ContentsVM.UpdateDataList(SearchBar);
            TransactionVM.UpdateDataList(SearchBar);
            NewTransactionVM.UpdateDataList(SearchBar);
            string commandText = "SELECT DISTINCT [Job number] FROM [Sheet1$] " +
            "";
            JobNumbers = DbContext.GetDataTable(commandText, searchBar);
        }

        public void MainVM_ShowMessage(object sender, MessageEventArgs e) {
            Status = e.Message;
        }

        public void MainVM_DbContextChanged(object sender, EventArgs e) {
            
        }

    }
}
