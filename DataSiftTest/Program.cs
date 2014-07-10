using DataSift.Enum;
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
            _client = new DataSift.DataSift("flooding", "469c369b35cef6f7f8ccac30e24d6016");

            Core();

            Console.ReadKey(true);
        }

        private static void Core()
        {
            Console.WriteLine("Compiling CSDL...");
            var compiled = _client.Compile("interaction.content contains \"music\"");
            Console.WriteLine("Compiled to {0}, DPU = {1}", compiled.Data.hash, compiled.Data.dpu);

            Console.WriteLine("Getting usage...");
            var usage = _client.Usage();
            Console.WriteLine("Usage report: " + JsonConvert.SerializeObject(usage.Data));

            Console.WriteLine("Getting balance...");
            var balance = _client.Balance();
            Console.WriteLine("Balance report: " + JsonConvert.SerializeObject(balance.Data));
        }

        private static void Historics()
        {
            Console.WriteLine("Get..");
            var get = _client.Historics.Get();
            Console.WriteLine("Get: " + JsonConvert.SerializeObject(get.Data));

            Console.WriteLine("Compiling CSDL...");
            var compiled = _client.Compile("interaction.content contains \"datasift\"");
            Console.WriteLine("Compiled to {0}, DPU = {1}", compiled.Data.hash, compiled.Data.dpu);

            var prepare = _client.Historics.Prepare(compiled.hash, DateTimeOffset.Now.AddDays(-8), DateTimeOffset.Now.AddDays(-1), "Test .NET Client Library", new string[] { "twitter" }, Sample.TenPercent);
            Console.WriteLine("Prepared historic query, ID = " + prepare.Data.id);
        }
    }
}
