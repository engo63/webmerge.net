using System;
using System.Net.Http;
using System.Threading.Tasks;
using WebMerge.Client.Core;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            MainAsync(args).GetAwaiter().GetResult();


            Console.Read();
        }

        static async Task MainAsync(string[] args)
        {
            IApiConfigurator configuration = new WebMergeConfiguration("J8DFJU2K9Q4MRCVDBWIPL3MWZMXY", "HPMSNRXX", new Uri("https://www.webmerge.me"));

            var hClient = new HttpClient();
            using (var client = new WebMergeClient(hClient, configuration))
            {
                var result = await client.GetDocumentListAsync("", "test");

                Console.WriteLine(result);
            }
        }

    }
}
