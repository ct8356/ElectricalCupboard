using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using CJT;

namespace ConnectToExcelOLEDB {
    class TransactionsVM : DataTableVM {
        //NOTE: transaction is the only VM that will actually WRITE to the database!
        //ACTUALLY! NO! PartClass can submit a transaction... Can do this itself via excelContext!

        //NOTE: WON'T BOTHER with ORDERS! JUST GROUP all transactions by job.
        //SO just have transaction, for each partClass. No need for GROUPTransactions class.
        //JUST GROUP by job. OR supplier, or both... EASY! (That is all order is really!).
        private string partNumber;
        public string PartNumber {
            get { return partNumber; }
            set { partNumber = value; NotifyPropertyChanged("PartNumber"); }
        }

        private string manufacturer;
        public string Manufacturer {
            get { return manufacturer; }
            set { manufacturer = value; NotifyPropertyChanged("Manufacturer"); }
        }

        private string description;
        public string Description {
            get { return description; }
            set { description = value; NotifyPropertyChanged("Description"); }
        }

        private int quantity;
        public int Quantity {
            get { return quantity; }
            set { quantity = value; NotifyPropertyChanged("Quantity"); }
        }

        private string condition;
        public string Condition {
            get { return condition; }
            set { condition = value; NotifyPropertyChanged("Condition"); }
        }

        private string shelf;
        public string Shelf {
            get { return shelf; }
            set { shelf = value; NotifyPropertyChanged("Shelf"); }
        }

        private DateTime dateMade;
        public DateTime DateMade {
            get { return dateMade; }
            set { dateMade = value; NotifyPropertyChanged("DateMade"); }
        }

        public TransactionsVM(DbContext dbContext) {
            DbContext = dbContext;
            CurrentSelection = 0;
            CommandText = "SELECT * FROM [Sheet1$] WHERE [Part number] LIKE @searchBar";
            ReadCurrentEntry("");
        }

        public void ReadCurrentEntry(string searchBar) {
            if (DataTable == null) DataTable = DbContext.GetDataTable(CommandText, searchBar);
            if (DataTable.Rows.Count > 0) {
                PartNumber = DataTable.Rows[CurrentSelection][0].ToString();
                Manufacturer = DataTable.Rows[CurrentSelection][1].ToString();
                Description = DataTable.Rows[CurrentSelection][2].ToString();
                Quantity = Convert.ToInt32(DataTable.Rows[CurrentSelection][3].ToString());
                Condition = DataTable.Rows[CurrentSelection][4].ToString();
                Shelf = DataTable.Rows[CurrentSelection][5].ToString();
                DateMade = Convert.ToDateTime(DataTable.Rows[CurrentSelection][6].ToString());
            }
        }

    }
}
