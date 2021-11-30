using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace redis_keyspace
{
    public interface ITaskServices
    {
        void SubscribeToDo(string keyPrefix);

        Task DoTaskAsync();
    }

    public class TaskServices : ITaskServices
    {
        private readonly IRedisCacheClient _redisCacheClient;

        public TaskServices(IRedisCacheClient redisCacheClient)
        {
            _redisCacheClient = redisCacheClient;

        }

        public async Task DoTaskAsync()
        {
            // do something here  
            // ...  
            using (ConnectionMultiplexer connection = ConnectionMultiplexer.Connect("localhost:6379"))
            {

                //// this operation should be done after some min or sec  
                //var taskId = new Random().Next(1, 10000);
                //int sec = new Random().Next(1, 5);

                //var subscriber = connection.GetSubscriber();

                //// publish a message to the 'chat' channel
                //subscriber.Publish("chat", "This is a message");

                IDatabase db = connection.GetDatabase();

                string key = "key:676.787.878.78:676766";
                
                int stage = (int?)(await db.StringGetAsync(key)) ?? 0;
                if (stage > 5)
                {
                    //add to blacklist database
                }
                stage++;
                await db.StringSetAsync(key, stage);

                var timeremaining = await db.KeyTimeToLiveAsync(key) ?? TimeSpan.Zero;
                await db.KeyExpireAsync(key, TimeSpan.FromSeconds(10) + timeremaining);
            }
        }

        public void SubscribeToDo(string keyPrefix)
        {
            System.Diagnostics.Debug.WriteLine("Starting...");
            ConnectionMultiplexer connection = ConnectionMultiplexer.Connect("localhost:6379");
            
                var subscriber = connection.GetSubscriber();

                // subscribe to a messages over the 'chat' channel
                subscriber.Subscribe("__keyevent@0__:expired", (channel, message) =>
                {
                    string messageString = (string)message;
                    // do something with the message
                    System.Diagnostics.Debug.WriteLine((string)channel);
                    System.Diagnostics.Debug.WriteLine(messageString);

                    IDatabase db = connection.GetDatabase();

                    string[] messages = messageString.Split(':');

                    System.Diagnostics.Debug.WriteLine(messages[0]);
                    System.Diagnostics.Debug.WriteLine(messages[1]);

                    //Check if messages[1] is in the blacklist database

                    System.Diagnostics.Debug.WriteLine(messages[2]);
                });
            

            //_redisCacheClient.Db0.SubscribeAsync(
            //    ("__keyevent@0__:expired", arg =>
            //    {
            //        var msg = arg.Body;
            //        Console.WriteLine($"recive {msg}");
            //        if (msg.StartsWith(keyPrefix))
            //        {
            //        // read the task id from expired key  
            //        var val = msg.Substring(keyPrefix.Length);
            //            Console.WriteLine($"Redis + Subscribe {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} begin to do task {val}");
            //        }
            //    }
            //)
            //);
        }
    }

}
