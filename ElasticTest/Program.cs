using System;
using System.Collections.Generic;
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

            var person = new Person {
                id = 1,
                firstName = "Mark",
                lastName = "Martynenko"
            };
            /*
            var indexResponse = eda.IndexDocumentAsync(person);
            indexResponse.Wait();
            var indexResult = indexResponse.Result;
            Console.WriteLine(indexResult.IsValid);
            */

            Console.WriteLine(eda.SearchDocs("Mark"));

            Console.ReadKey();
        }
    }

    public class Person
    {
        public int id;
        public string firstName;
        public string lastName;
    }
}
