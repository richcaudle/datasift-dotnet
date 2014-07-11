using DataSift.Enum;
using DataSift.Rest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSiftTest
{
    class Program
    {
        private static DataSift.DataSift _client;

        static void Main(string[] args)
        {
            // Create a DataSift client
            _client = new DataSift.DataSift("username", "apikey");

            Historics();

            Console.ReadKey(true);
        }

        private static void Core()
        { 
            try
            {
                Console.WriteLine("Compiling CSDL...");
                var compiled = _client.Compile("interaction.content containss \"music\"");
                Console.WriteLine("Compiled to {0}, DPU = {1}", compiled.Data.hash, compiled.Data.dpu);

                Console.WriteLine("Getting usage...");
                var usage = _client.Usage();
                Console.WriteLine("Usage report: " + JsonConvert.SerializeObject(usage.Data));

                Console.WriteLine("Getting balance...");
                var balance = _client.Balance();
                Console.WriteLine("Balance report: " + JsonConvert.SerializeObject(balance.Data));
            }
            catch (RestAPIException e)
            {
                Console.WriteLine("Exception: " + e.StackTrace);
            }
        }

        private static void Historics()
        {
            var get = _client.Historics.Get();
            Console.WriteLine("List of historics: " + JsonConvert.SerializeObject(get.Data));

            var status = _client.Historics.Status(DateTimeOffset.Now.AddDays(-2), DateTimeOffset.Now.AddDays(-1), new string[] { "twitter" });
            Console.WriteLine("Twitter status for period: " + status.Data[0].sources.twitter.status);

            var compiled = _client.Compile("interaction.content contains \"datasift\"");
            Console.WriteLine("Compiled CSDL to {0}, DPU = {1}", compiled.Data.hash, compiled.Data.dpu);

            var prepare = _client.Historics.Prepare(compiled.Data.hash, DateTimeOffset.Now.AddDays(-8), DateTimeOffset.Now.AddDays(-1), "Test .NET Client Library", new string[] { "twitter" }, Sample.TenPercent);
            Console.WriteLine("Prepared historic query, ID = " + prepare.Data.id);

            var update = _client.Historics.Update(prepare.Data.id, "Updated historic query");
            Console.WriteLine("Updated historic (otherwise there would have been an exception!)");

            var getById = _client.Historics.Get(id: prepare.Data.id);
            Console.WriteLine("Details for updated historic: " + JsonConvert.SerializeObject(getById.Data));

            _client.Historics.Delete(prepare.Data.id);
            Console.WriteLine("Deleted historic (otherwise there would have been an exception!)");

            
        }
    }
}
