using DataSift.Enum;
using DataSift.Rest;
using DataSift.Streaming;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataSiftTest
{
    class Program
    {
        private static DataSift.DataSift _client;
        private static DataSift.Streaming.DataSiftStream _stream;

        static void Main(string[] args)
        {
            // Create a DataSift client
            _client = new DataSift.DataSift("rcaudle", "b09a645fe2f1fed748c12268fd473662");

            Stream();

            Console.ReadKey(true);
        }

        private static void Stream()
        {
            Console.WriteLine("Connecting...");
            _stream = _client.Connect();
            _stream.OnConnect += stream_OnConnect;
            _stream.OnMessage += _stream_OnMessage;
            _stream.OnDataSiftMessage += _stream_OnDataSiftMessage;
            _stream.OnSubscribed += _stream_OnSubscribed;
            _stream.OnError +=_stream_OnError;
            _stream.OnClosed += _stream_OnClosed;
        }

        static void _stream_OnClosed(object sender, EventArgs e)
        {
            Console.WriteLine("CLOSED!");
        }

        static void _stream_OnError(StreamAPIException e)
        {
            Console.WriteLine("Error: " + e.StackTrace);
        }

        static void _stream_OnSubscribed(string hash)
        {
            Console.WriteLine("GLOBAL Subscribed (" + hash + ")");
        }

        static void _stream_OnDataSiftMessage(DataSiftMessageStatus status, string message)
        {
            
            Console.WriteLine("DATASIFT MESSAGE (" + status + "): " + message);
        }

        static void stream_OnConnect()
        {
            Console.WriteLine("Connected...");

            //var arsenalStream = _client.Compile("interaction.content contains_any \"arsenal,afc\"");
            //Console.WriteLine("Compiled to {0}, DPU = {1}", arsenalStream.Data.hash, arsenalStream.Data.dpu);

            //var spursStream = _client.Compile("interaction.content contains_any \"spurs, tottenham, thfc\"");
            //Console.WriteLine("Compiled to {0}, DPU = {1}", spursStream.Data.hash, spursStream.Data.dpu);

            //DataSift.Streaming.DataSiftStream.OnMessageHandler onMessage = (hash, message) =>
            //{
            //    Console.WriteLine("Message (" + hash + "): " + message.interaction.content);
            //};

            //DataSift.Streaming.DataSiftStream.OnSubscribedHandler onSubscribed = (hash) =>
            //{
            //    Console.WriteLine("Subscribed (" + hash + ")");
            //};

            ////_stream.Subscribe(arsenalStream.Data.hash, onMessage);
            //_stream.Subscribe(spursStream.Data.hash, onMessage, onSubscribed);
            ////_stream.Subscribe(spursStream.Data.hash);
            

            //Console.WriteLine("Subscribed...");
        }

        static void _stream_OnMessage(string hash, dynamic message)
        {
            Console.WriteLine("GLOBAL Message (" + hash + "): " + message.interaction.content);
        }

        private static void Core()
        { 
            try
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

            var prepare = _client.Historics.Prepare(compiled.Data.hash, DateTimeOffset.Now.AddDays(-8), DateTimeOffset.Now.AddDays(-1), "Example historic", new string[] { "twitter" }, Sample.TenPercent);
            Console.WriteLine("Prepared historic query, ID = " + prepare.Data.id);

            var subscription = _client.Push.Create("Example historic subscription", "pull", historicsId: prepare.Data.id);
            Console.WriteLine("Created subscription, ID = " + subscription.Data.id);

            _client.Historics.Start(prepare.Data.id);
            Console.WriteLine("Started historic (otherwise there would have been an exception!)");

            var update = _client.Historics.Update(prepare.Data.id, "Updated historic query");
            Console.WriteLine("Updated historic (otherwise there would have been an exception!)");

            var getById = _client.Historics.Get(id: prepare.Data.id);
            Console.WriteLine("Details for updated historic: " + JsonConvert.SerializeObject(getById.Data));

            _client.Historics.Pause(prepare.Data.id);
            Console.WriteLine("Paused historic (otherwise there would have been an exception!)");

            _client.Historics.Resume(prepare.Data.id);
            Console.WriteLine("Resumed historic (otherwise there would have been an exception!)");

            _client.Historics.Stop(prepare.Data.id);
            Console.WriteLine("Stopped historic (otherwise there would have been an exception!)");

            _client.Historics.Delete(prepare.Data.id);
            Console.WriteLine("Deleted historic (otherwise there would have been an exception!)");

            
        }

        private static void Push()
        {
            var get = _client.Push.Get(page:1, perPage: 5, orderBy: OrderBy.UpdatedAt, includeFinished: true);
            Console.WriteLine("List of push subscriptions: " + JsonConvert.SerializeObject(get.Data));

            var compiled = _client.Compile("interaction.content contains \"music\"");
            Console.WriteLine("Compiled to {0}", compiled.Data.hash);

            var create = _client.Push.Create(".NET example pull", "pull", hash: compiled.Data.hash);
            Console.WriteLine("Created pull subscription: {0}", create.Data.id);

            var update = _client.Push.Update(create.Data.id, name: "Updated example pull");
            Console.WriteLine("Updated subscription name.");

            var getById = _client.Push.Get(id: create.Data.id);
            Console.WriteLine("Subscription details: " + JsonConvert.SerializeObject(getById.Data));

            var log = _client.Push.Log(create.Data.id);
            Console.WriteLine("Log for new subscription: " + JsonConvert.SerializeObject(log.Data.log_entries));

            Console.WriteLine("Pausing for data.");
            Thread.Sleep(5000);

            var pull = _client.Pull(create.Data.id, size: 500000);
            Console.WriteLine("Got data, first interaction: " + JsonConvert.SerializeObject(pull.Data[0]));

            _client.Push.Stop(create.Data.id);
            Console.WriteLine("Stopped subscription (otherwise there would have been an exception!)");

            _client.Push.Delete(create.Data.id);
            Console.WriteLine("Deleted subscription (otherwise there would have been an exception!)");
        }

        private static void HistoricsPreview()
        {
            var compiled = _client.Compile("interaction.content contains \"datasift\"");
            Console.WriteLine("Compiled to {0}", compiled.Data.hash);

            var prms = new List<HistoricsPreviewParameter>();
            prms.Add(new HistoricsPreviewParameter() { Target = "interaction.author.link", Analysis = "targetVol", Argument = "hour" });
            prms.Add(new HistoricsPreviewParameter() { Target = "twitter.user.lang", Analysis = "freqDist", Argument = "10" });
            prms.Add(new HistoricsPreviewParameter() { Target = "twitter.user.followers_count", Analysis = "numStats", Argument = "hour" });
            prms.Add(new HistoricsPreviewParameter() { Target = "interaction.content", Analysis = "wordCount", Argument = "10" });

            var create = _client.HistoricsPreview.Create(compiled.Data.hash, new string[] { "twitter" }, prms, DateTimeOffset.Now.AddDays(-2), DateTimeOffset.Now.AddDays(-1));
            Console.WriteLine("Created preview: {0}", create.Data.id);

            Console.WriteLine("Pausing for preview status update.");
            Thread.Sleep(10000);

            var get1 = _client.HistoricsPreview.Get(create.Data.id);
            Console.WriteLine("Preview status: " + JsonConvert.SerializeObject(get1.Data));

        }

        private static void Sources()
        {
            var get = _client.Source.Get(page: 1, perPage: 5);
            Console.WriteLine("List of sources: " + JsonConvert.SerializeObject(get.Data));

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
                            value = "authtoken"
                        }
                    }
                };

            var create = _client.Source.Create("facebook_page", "Example source", prms, resources, auth);
            Console.WriteLine("Created source: {0}", create.Data.id);

            _client.Source.Start(create.Data.id);
            Console.WriteLine("Started source (otherwise there would have been an exception!)");

            var update = _client.Source.Update("facebook_page", "Updated example source", create.Data.id, create.Data.parameters, create.Data.resources, create.Data.auth);
            Console.WriteLine("Updated source: {0}", update.Data.id);

            var getSource = _client.Source.Get(id: create.Data.id);
            Console.WriteLine("Source details: " + JsonConvert.SerializeObject(getSource.Data));
            
            _client.Source.Stop(create.Data.id);
            Console.WriteLine("Stopped source (otherwise there would have been an exception!)");

            var log = _client.Source.Log(create.Data.id, 1, 5);
            Console.WriteLine("Source log: " + JsonConvert.SerializeObject(log.Data.log_entries));

            _client.Source.Delete(create.Data.id);
            Console.WriteLine("Deleted source (otherwise there would have been an exception!)");

        }

        private static void DynamicLists()
        {
            var get = _client.List.Get();
            Console.WriteLine("Current of dynamic lists: " + JsonConvert.SerializeObject(get.Data));

            var create = _client.List.Create(ListType.Integer, "Example integer list");
            Console.WriteLine("Created list: {0}", create.Data.id);

            _client.List.Add(create.Data.id, new int[] { 1, 2, 3, 4, 5 });
            Console.WriteLine("Added items to list");

            _client.List.Remove(create.Data.id, new int[] { 1, 2 });
            Console.WriteLine("Removed items from list");

            var exists = _client.List.Exists(create.Data.id, new int[] { 2, 4 });
            Console.WriteLine("Existance in list: " + JsonConvert.SerializeObject(exists.Data));

            var replace = _client.List.ReplaceStart(create.Data.id);
            Console.WriteLine("Replace started with ID: " + replace.Data.id);

            _client.List.ReplaceAdd(replace.Data.id, new int[] { 6, 7, 8, 9, 10 });
            Console.WriteLine("Added new items in replace.");

            _client.List.ReplaceCommit(replace.Data.id);
            Console.WriteLine("Committed bulk replace");

            _client.List.Delete(create.Data.id);
            Console.WriteLine("Deleted list (otherwise there would have been an exception!)");

        }
    }
}
