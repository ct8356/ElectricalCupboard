using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using CJT;
using CJT.Models;

namespace ConnectToExcelOLEDB.DataTableVMs {
    class ContentsVM : DataTableVM {

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

        public ContentsVM(DbContext dbContext) {
            DbContext = dbContext;
            CurrentSelection = 0;
            CommandText = "SELECT [Part number], Manufacturer, Description, SUM(Quant) AS Quant, Condition, Shelf FROM [Sheet1$] " +
            "WHERE [Part number] LIKE @searchBar OR Description LIKE @searchBar " +
            "GROUP BY [Part number], Manufacturer, Description, Condition, Shelf " +
            "";
            UpdateDataList("");
        }

        public override IList<Entry> GetDataList(string searchString) {
            IList<Entry> list = new ObservableCollection<Entry>();
            foreach (DataRow row in DataTable.Rows) {
                PartClass part = new PartClass();
                part.OrderNumber = row[0].ToString();
                part.Manufacturer = row[1].ToString();
                part.Description = row[2].ToString();
                list.Add(part);
            }
            return list;
        }

        public void ReadCurrentEntry(string searchBar) {
            if (DataTable == null) DataTable = DbContext.GetDataTable(CommandText, searchBar);
            if (DataTable.Rows.Count > 0 && CurrentSelection != -1) {
                PartNumber = DataTable.Rows[CurrentSelection][0].ToString();
                Manufacturer = DataTable.Rows[CurrentSelection][1].ToString();
                Description = DataTable.Rows[CurrentSelection][2].ToString();
                Quantity = 0;
                Condition = DataTable.Rows[CurrentSelection][4].ToString();
                Shelf = DataTable.Rows[CurrentSelection][5].ToString();
            }
        }

    }
}
