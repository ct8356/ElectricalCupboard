using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using CJT;

namespace ConnectToExcelOLEDB {
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
        }

    }
}
