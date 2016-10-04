using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConnectToExcelOLEDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectToExcelOLEDB.Tests {

    [TestClass()]
    public class MainWindowTests {

        [TestMethod()]
        public void getListTest() {
            MainWindow mainWindow = new MainWindow();
            List<string> list = mainWindow.getList("Quant");
            foreach (string element in list) {
                Console.WriteLine(element);
            }
        }

    }
}