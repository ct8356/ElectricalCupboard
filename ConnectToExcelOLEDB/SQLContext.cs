
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.ComponentModel; //this allows INotifyPropertyChanged
using CJT;
using AutoCompleteBox = CJT.AutoCompleteBox;
using System.Data;
using System.Data.SqlClient; //NOTE: should learn a bit about this library.
using System.Configuration;

namespace ConnectToExcelOLEDB {
    class SqlContext : CJT.DbContext {

        public override string GetConnectionString() {
            return ConfigurationManager.ConnectionStrings["ElectricalCupboardDatabase"].ConnectionString;
        }

        public override DataTable GetDataTable(string commandText) {
            using (SqlCommand cmd = new SqlCommand()) {
                cmd.CommandText = commandText;
                using (SqlConnection conn = new SqlConnection(GetConnectionString())) {
                    cmd.Connection = conn;
                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    return table;
                }
            }
        }

        public override DataTable GetDataTable(string commandText, string searchBar) {
            using (SqlCommand cmd = new SqlCommand()) {
                SqlParameter searchBarParam = new SqlParameter("searchBar", "%" + searchBar + "%");//Note; can specify data type if want to.
                cmd.Parameters.Add(searchBarParam);
                cmd.CommandText = commandText; //@ not working!
                using (SqlConnection conn = new SqlConnection(GetConnectionString())) {
                    cmd.Connection = conn;
                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    //NOTE: adapter is for linking DataSet (returned from query?) to DataSource (the DataTable that bind to?)
                    //Not sure about this though.
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    return table;
                }
            }
        }

        public override DataTable GetDataTable(string commandText, string[] columnNames, string[] values) {
            using (SqlCommand cmd = new SqlCommand()) {
                for (int col = 0; col < columnNames.Length; col++) {
                    cmd.Parameters.AddWithValue(columnNames[col], values[col]);
                }
                cmd.CommandText = commandText;
                using (SqlConnection conn = new SqlConnection(GetConnectionString())) {
                    cmd.Connection = conn;
                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    //NOTE: adapter is for linking DataSet (returned from query?) to DataSource (the DataTable that bind to?)
                    //Not sure about this though.
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    return table;
                }
            }
        }

        public override int InsertEntry(string commandText, string[] columnNames, string[] values) {
            using (SqlCommand cmd = new SqlCommand()) {
                for (int col = 0; col < columnNames.Length; col++) {
                    cmd.Parameters.AddWithValue(columnNames[col], values[col]);
                }
                cmd.CommandText = commandText;
                using (SqlConnection conn = new SqlConnection(GetConnectionString())) {
                    cmd.Connection = conn;
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    //TransactionTableChanged(this, new EventArgs());
                    return rowsAffected;
                }
            }
        }

        public override void SaveSettings() {
            //
        }

    }
}
