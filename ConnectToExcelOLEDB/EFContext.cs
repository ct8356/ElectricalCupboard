
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.ComponentModel; //this allows INotifyPropertyChanged
using CJT.Models;
using AutoCompleteBox = CJT.AutoCompleteBox;
using System.Data;
using System.Data.SqlClient; //NOTE: should learn a bit about this library.
using System.Configuration;

namespace ConnectToExcelOLEDB {
    //NOTE: what you cannot see here,
    //is that when this class is instantiated, a connection is made.
    //The "namespace + class name" are used to identify the database.
    //If database does not exist, is it created? yes! (Not sure about that though).
    //NOTE, it also has a Database property.
    //Apparently, if call database.Create, creates a database that matches this schema.
    //SO, don't even need InitialCreate file??? No, you don't.
    public class EFContext : CJT.DbContext {
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Entry> Entries { get; set; }
        public DbSet<PartClass> PartClasses { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public delegate PropertyChangedEventHandler Handler(PropertyChangedEventArgs args);

        public EFContext() : base() {
            //Problem is, I want it to handle prop changed every time ANY student changes.
            //which means, subscribing to EVERY student.
            //AND don't just want to save database,
            //have to UPDATE a row as well.
            //MUST be easier way to do this?
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public override string GetConnectionString() {
            return string.Format("yo");
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

        //HANDLERS
        //Now, I want this Class to respond to an event.
        public void handlePropertyChanged(PropertyChangedEventArgs args) {
        }
    }
}
