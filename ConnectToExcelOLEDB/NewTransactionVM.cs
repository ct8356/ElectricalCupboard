using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using CJT;
using System.ComponentModel;

namespace ConnectToExcelOLEDB {
    class NewTransactionVM : DataTableVM {
        //NOTE: this class should be really simple...
        //It is a VIEWModel! Its specific to the view.
        //It is PURELY to bind to...
        //IT SHOULD ALMOST, have NO METHODS!
        //Any methods, put in other classes. (i.e. neither view nor VM.
        //almost VMModels. Or DALs. you decide.
        //Call it CupboardManager. OR just Cupboard.
        //GOOD, coz may want MANY VMs to use its CheckEnoughParts methods!
        //COOL!
        //SWEET part is, THEN only NEED one set of DataTAbleVMs!
        //JUST call abstract method, let DbContext do all the work!
        //BUT not true???? because I need to pass CheckEnoughParts Something...
        //WAS gonna have a DataTable, and convert it to model when necessary.


        //YO! I think the best way around this is, 
        //TO have a generic NewTransactionVM, that's abstract.
        //THEN have a name space here made for SQLContext,
        //THEN have a namespace for ExcelContext,
        //THEN, use the appropriate class for your needs.
        //I.E. Starting from the MAINVM, if you want ExcelContext, use it.
        //IF want SQL context, use it.
        //IF want both, use both. I GUESS will need to instantiate both VMs.
        //OR just instantiate them when you need them...
        //BUT actually, IF really want to use BOTH,
        //PROBS is easier to use ONE VM, that has same fields,
        //WELL NAH, coz SO different, best to keep them separate.
        //ANYTHING that IS the same, put in the abstract class.
        //OK, the VM is bound to the view....
        //SO if the view is for the EXCEL Connection, instantiate excelVM.
        //IF the view is for the SQL connection, instantiate SQLVM.
        //OK!
        //COZ wont want to access BOTH at same time.
        //BUT view should be similar. VIEW should have option,
        //TO BIND to the excelVM, OR the SQLVM!
        //OK FINE! Have generic VM it binds to... call its abstract methods.
        //OF COURSE! but the one it binds to, DEPENDS on what USER chose in settings.
        //SO once chose, COULD Close down one VM, OPEN another.
        //i,e. once data has been transported from one VM to the other.
        private int jobNumber;
        public int JobNumber {
            get { return jobNumber; }
            set { jobNumber = value; NotifyPropertyChanged("JobNumber"); }
        }
        public ContentsVM ContentsVM;

        public NewTransactionVM(DbContext dbContext, ContentsVM contentsVM) {
            //NOTE: newTransactionsVM is useless without ContentsVM,
            //SO it should be passed a ContentsVM!
            //BETTER THIS WAY, coz it looks after itself!
            DbContext = dbContext;
            ContentsVM = contentsVM;
            CurrentSelection = 0;
            CommandText = "SELECT [Part number], Manufacturer, Description, SUM(Quant) AS Quant, Condition, Shelf FROM [Sheet1$] " +
            "WHERE FALSE " +
            "GROUP BY [Part number], Manufacturer, Description, Condition, Shelf " +
            "";
            DbContext.GetDataTable(CommandText);
            //Subscribe
            ContentsVM.PropertyChanged += ContentsVM_PropertyChanged;
        }

        public void TakeRowFromContentsVM() {
            DataTable.Rows.Clear(); //remove this line if want to add multiple?
            DataTable.ImportRow(ContentsVM.SelectedItem);
            if (DataTable.Rows.Count > 0)
                DataTable.Rows[0]["Quant"] = 0;
        }

        public bool CheckEnoughParts() {
            DataRow row0 = DataTable.Rows[0];
            int quant = Convert.ToInt32(row0["quant"].ToString());
            int sumQuant = 0;
            if (quant != 0) {
                string checkText1 = "SELECT SUM(Quant) AS Quant FROM [Sheet1$] WHERE " +
                    "[Part number] = @partNumber AND " +
                    "[Condition] = @condition " +
                    "GROUP BY [Part number], Condition " +
                    "";
                DataTable dataTable = DbContext.GetDataTable(checkText1,
                    new string[2] { "partNumber", "condition" },
                    new string[2] { row0["Part number"].ToString(), row0["Condition"].ToString() });
                
                if (dataTable.Rows.Count > 0) {
                    sumQuant = Convert.ToInt32(dataTable.Rows[0][0].ToString());
                }
            }
            bool enoughParts = sumQuant + quant >= 0;
            return enoughParts;
        }

        public void ContentsVM_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            if (e.PropertyName == "SelectedItem")
                TakeRowFromContentsVM();
        } //This needs to be here, since ContentsVM does not KNOW about TransactionsVM.

        public void CommitTransactionIfValid() {
            DataRow row0 = DataTable.Rows[0];
            if (CheckEnoughParts()) {
                string commandText =
                "INSERT INTO [Sheet1$] VALUES (@partNumber, @manufacturer, @description, "+
                "@quantity, @condition, @shelf, @dateMade, @jobNumber) " +
                "";
                DbContext.InsertEntry(commandText,
                        new string[8] {
                            "partNumber",
                            "manufacturer",
                            "description",
                            "quantity",
                            "condition",
                            "shelf",
                            "dateMade",
                            "jobNumber"},
                        new string[8] {
                            row0["Part number"].ToString(),
                            row0["manufacturer"].ToString(),
                            row0["description"].ToString(),
                            row0["quant"].ToString(),
                            row0["condition"].ToString(),
                            row0["shelf"].ToString(),
                            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            JobNumber.ToString()}
                    );
                row0["quant"] = 0;
            }
            else {
                ShowMessage("There are not enough parts in the cupboard");
            }
        }

    }
}
