using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectToExcelOLEDB {
    interface IContentsVM {
        string PartNumber { get; set; }

        string Manufacturer { get; set; }

        string Description { get; set; }

        int Quantity { get; set; }
    
        string Condition { get; set; }

        string Shelf { get; set; }

    }
}
