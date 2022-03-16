using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticTest
{
    class Program
    {
        static void Main(string[] args)
        {
            ElasticDataAccess eda = new ElasticDataAccess();
            Console.WriteLine(eda.CheckIfIndexExists("kibana_sample_data_ecommerce"));
            /*
            var person = new Person {
                id = 1,
                firstName = "Mark",
                lastName = "Martynenko"
            };*/

            // Create a new DataTable.
            System.Data.DataTable table = new DataTable("ParentTable");
            // Declare variables for DataColumn and DataRow objects.
            DataColumn column;
            DataRow row;

            // Create new DataColumn, set DataType,
            // ColumnName and add to DataTable.
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Int32");
            column.ColumnName = "id";
            column.ReadOnly = true;
            column.Unique = true;
            // Add the Column to the DataColumnCollection.
            table.Columns.Add(column);

            // Create second column.
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "ParentItem";
            column.AutoIncrement = false;
            column.Caption = "ParentItem";
            column.ReadOnly = false;
            column.Unique = false;
            // Add the column to the table.
            table.Columns.Add(column);

            // Make the ID column the primary key column.
            DataColumn[] PrimaryKeyColumns = new DataColumn[1];
            PrimaryKeyColumns[0] = table.Columns["id"];
            table.PrimaryKey = PrimaryKeyColumns;

            // Create three new DataRow objects and add
            // them to the DataTable
            for (int i = 0; i <= 2; i++)
            {
                row = table.NewRow();
                row["id"] = i;
                row["ParentItem"] = "ParentItem " + i;
                table.Rows.Add(row);
            }

            var model = new ElasticSearchDashboardModel
            {
                Code = 1,
                ResultSet = table,
                Error = ""
            };

            string JSONresult;
            JSONresult = JsonConvert.SerializeObject(model.ResultSet);

            var modelJson = new ElasticSearchDashboardModelJson
            {
                Code = 2,
                ResultSet = JSONresult,
                Error = ""
            };
            
            var indexResponse = eda.IndexDocumentAsync(modelJson);
            indexResponse.Wait();
            var indexResult = indexResponse.Result;
            Console.WriteLine(indexResult.IsValid);
            
            Console.WriteLine(JSONresult);
            //Console.WriteLine(eda.SearchDocs("Mark"));
            Console.WriteLine(eda.SearchDocs(2));
            Console.WriteLine(eda.SearchDocsJson(JSONresult));

            Console.ReadKey();
        }
    }

    public class Person
    {
        public int id;
        public string firstName;
        public string lastName;
    }

    public class ElasticSearchDashboardModel
    {
        public int Code { get; set; }
        public DataTable ResultSet { get; set; }
        public string Error { get; set; }
    }

    public class ElasticSearchDashboardModelJson
    {
        public int Code { get; set; }
        public string ResultSet { get; set; }
        public string Error { get; set; }
    }

}
