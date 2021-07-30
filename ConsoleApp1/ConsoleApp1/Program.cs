using Epic.OnlineServices;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Timers;

namespace ConsoleApp1
{
    class Program
    {
        static Eos _eos = new Eos();
        static Timer _timer;

        static void Main()
        {
            Console.WriteLine("EOS Test Connect Login");

            _eos.Initialize();
            StartTicks(_eos.Tick);

            System.Threading.Thread.Sleep(3000);
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("- Press ENTER to login...              -");
            Console.WriteLine("----------------------------------------");
            Console.ReadKey();

            var oauthClient = new ItchioOAuthClient(ConfigurationManager.AppSettings["itchio_client_id"], ConfigurationManager.AppSettings["itchio_redirect_uri"]);
            var accessToken = oauthClient.RequestAuthorization();

            Console.WriteLine($"Getting Itch.io user info with accessToken [{accessToken}]...");
            var result = HttpGet("https://itch.io/api/1/key/me", accessToken);

            Console.WriteLine("Itch.io user info received:");
            Console.WriteLine(result);

            Console.WriteLine($"Attempting to login using EOS Connect with Itch.io accessToken [{accessToken}]...");
            _eos.Login(accessToken, ExternalCredentialType.ItchioKey, (ProductUserId productUserId) =>
            {
                Console.WriteLine($"ProductUserId [{productUserId}]");
            });

            Console.WriteLine("Press ENTER to quit...");
            Console.ReadKey();
            _eos.Shutdown();
        }

        private static string HttpGet(string uri, string accessToken)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Headers["Authorization"] = $"Bearer {accessToken}";
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return JObject.Parse(reader.ReadToEnd()).ToString(Newtonsoft.Json.Formatting.Indented);
            }
        }

        public static void StartTicks(Action tick)
        {
            _timer = new Timer(100);
            _timer.Elapsed += new ElapsedEventHandler((object sender, ElapsedEventArgs e) => _eos.Tick());
            _timer.Enabled = true;
        }
    }
}
