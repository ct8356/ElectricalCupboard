using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Microsoft.Office.Interop.Excel; //using Excel = ....  useful
using System.Data.OleDb;
using System.Data;
using System.IO;
using DataTable = System.Data.DataTable;

namespace ConnectToExcelOLEDB {
    class ExcelContext : CJT.ExcelContext {

        public ExcelContext() {
            FilePath = Properties.Settings.Default["FilePath"].ToString();
            if (FilePath == null || FilePath == "") FilePath = 
                    "C:\\Users\\CJT\\OneDrive\\Documents\\ElectricalCupboard\\ElectricalCupboardContents.xlsx";
        }

        public void AddColumn() {
            string commandText =
                "Create Table [Sheet1$] ([Part number] String, Manufacturer String, Description String, "+
                "Quant Int, Condition String, Shelf String, [Date made] DateTime, [Job number] Int)";
            using (OleDbCommand cmd = new OleDbCommand()) {
                cmd.CommandText = commandText;
                using (OleDbConnection conn = new OleDbConnection(GetConnectionString())) {
                    cmd.Connection = conn;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public int ExecuteScalar(string commandText, ContentsVM vm) {
            using (OleDbCommand cmd = new OleDbCommand()) {
                cmd.Parameters.AddWithValue("partNumber", vm.PartNumber);
                cmd.Parameters.AddWithValue("quantity", vm.Quantity);
                cmd.Parameters.AddWithValue("condition", vm.Condition);
                cmd.CommandText = commandText;
                using (OleDbConnection conn = new OleDbConnection(GetConnectionString())) {
                    cmd.Connection = conn;
                    conn.Open();
                    int integer = (int)cmd.ExecuteScalar();
                    return integer;
                }
            }
        }

        public override string GetConnectionString() {
                return string.Format(
                //"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};"+
                //"Extended Properties='Excel 12.0;HDR=YES;IMEX=1;'",
                "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=Excel 12.0;",
                FilePath);
        }

        //public ICollection<T> GetCollection<T>(string commandText, string searchBar) {
        //    ICollection<T> collection;
        //    using (OleDbCommand cmd = new OleDbCommand()) {
        //        OleDbParameter searchBarParam = 
        //new OleDbParameter("searchBar", "%" + searchBar + "%");//Note; can specify data type if want to.
        //        cmd.Parameters.Add(searchBarParam);
        //        cmd.CommandText = commandText; //@ not working!
        //        using (OleDbConnection conn = new OleDbConnection(GetConnectionString())) {
        //            cmd.Connection = conn;
        //            conn.Open();
        //            OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
        //            //NOTE: adapter is for linking DataSet (returned from query?) to DataSource (the DataTable that bind to?)
        //            //Not sure about this though.
        //            DataTable table = new DataTable();
        //        }
        //    }
        //    return collection;
        //}

        //NOTE: when wondering if should use close or dispose on connection:
        //SHOULD use dispose, unless CERTAIN the SAME instance of the connection will be opened again.
        //BECAUSE it is IDisposable, so NEED to call dispose on it...
        //AND it is better to use "using", because it is syntactic sugar for a try and finally block.
        //(WHICH is nec because a connection COULD cause an exception, if SQL is down, and crash your program).
        //NOTE: DOES it actually crash though? Or just not do what you asked it to do?
        //SO THERE! so, for each user ACTION, should be wrapped in a USING block. Fair?
        //DO NOT just leave the connection in RAM, JUST because program still running.
        //BECAUSE it HAS unmanaged resources, that might not be closed for AGES otherwise (don't get this, but ok, accept it.
        //ENOUGH PEOPLE have suggested wrapping it in using. SO i am happy. 
        //Takes NO EFFORT to instantiate a connection. So do it.
        //AND I've seen enough examples, of 3 tier using statements. Conn, command, reader. so do it!

        public DataTable GetDataTable(string commandText, ContentsVM vm) {
            using (OleDbCommand cmd = new OleDbCommand()) {
                cmd.Parameters.AddWithValue("partNumber", vm.PartNumber);
                cmd.Parameters.AddWithValue("condition", vm.Condition);
                cmd.CommandText = commandText; //@ not working!
                using (OleDbConnection conn = new OleDbConnection(GetConnectionString())) {
                    cmd.Connection = conn;
                    conn.Open();
                    OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
                    //NOTE: adapter is for linking DataSet (returned from query?) to DataSource (the DataTable that bind to?)
                    //Not sure about this though.
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    return table;
                }
            }
        }

        public new void CreateTable() {
            OleDbCommand cmd = new OleDbCommand();
            //cmd.Connection = myConn;
            //cmd.CommandText = "CREATE TABLE [table1] (id INT, name VARCHAR, datecol DATE );";
            //cmd.ExecuteNonQuery();
        }

        public override void SaveSettings() {
            Properties.Settings.Default["FilePath"] = FilePath;
            Properties.Settings.Default.Save(); // Saves settings in application configuration file
        }

    }
}
