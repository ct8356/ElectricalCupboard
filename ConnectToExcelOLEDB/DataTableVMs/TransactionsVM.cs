﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CJT;

namespace ConnectToExcelOLEDB.DataTableVMs {
    class TransactionsVM : ConnectToExcelOLEDB.TransactionsVM {

        public TransactionsVM(DbContext dbContext) : base(dbContext) {
        }

    }
}
