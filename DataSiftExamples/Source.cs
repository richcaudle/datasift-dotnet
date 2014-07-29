using DataSift;
using DataSift.Rest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataSiftExamples
{
    static class Source
    {
        internal static void Run(string username, string apikey)
        {
            // TODO: Insert your auth token here from https://datasift.com/source/managed/new/facebook_page
            var authtoken = "";

            if(String.IsNullOrEmpty(authtoken))
                throw new ArgumentException("Set the authtoken variable to a valid token");

            var client = new DataSiftClient(username, apikey);

            Console.WriteLine("Running 'Source' example...");

            var get = client.Source.Get(page: 1, perPage: 5);
            Console.WriteLine("\nList of sources: " + JsonConvert.SerializeObject(get.Data));

            var prms = new
            {
                likes = true,
                posts_by_others = true,
                comments = true,
                page_likes = false
            };

            var resources = new[] {
                    new { 
                        parameters = new {
                            url = "http://www.facebook.com/theguardian",
                            title = "The Guardian",
                            id = 10513336322
                        }
                    }
                };

            var auth = new[] {
                    new { 
                        parameters = new {
                            value = authtoken
                        }
                    }
                };

            var create = client.Source.Create("facebook_page", "Example source", prms, resources, auth);
            Console.WriteLine("\nCreated source: {0}", create.Data.id);

            client.Source.Start(create.Data.id);
            Console.WriteLine("\nStarted source.");

            var update = client.Source.Update("facebook_page", "Updated example source", create.Data.id, create.Data.parameters, create.Data.resources, create.Data.auth);
            Console.WriteLine("\nUpdated source: {0}", update.Data.id);

            var getSource = client.Source.Get(id: create.Data.id);
            Console.WriteLine("\nSource details: " + JsonConvert.SerializeObject(getSource.Data));

            client.Source.Stop(create.Data.id);
            Console.WriteLine("\nStopped source.");

            var log = client.Source.Log(create.Data.id, 1, 5);
            Console.WriteLine("\nSource log: " + JsonConvert.SerializeObject(log.Data.log_entries));

            client.Source.Delete(create.Data.id);
            Console.WriteLine("\nDeleted source.");
        }
    }
}
