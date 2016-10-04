using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CJT;

namespace ConnectToExcelOLEDB.DataTableVMs {
    class NewTransactionVM : ConnectToExcelOLEDB.NewTransactionVM {

        public NewTransactionVM(DbContext dbContext, ContentsVM contentsVM ) 
            : base (dbContext, contentsVM) {
        }

    }
}
