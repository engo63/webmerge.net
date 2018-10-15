using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using WebMerge.Client.Core;
using WebMerge.Client.Core.ResponseModels;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            //GetList(args).GetAwaiter().GetResult();
            MergeDocumentWithoutDownload(args).GetAwaiter().GetResult();

            Console.ReadKey();
        }

        public static async Task GetList(string[] args)
        {

            IApiConfigurator configuration = new WebMergeConfiguration("J8DFJU2K9Q4MRCVDBWIPL3MWZMXY", "HPMSNRXX", new Uri("https://www.webmerge.me"));

            using (var client = new WebMergeClient(new HttpClient(), configuration))
            {
                var documents = await client.GetDocumentListAsync();

                Console.WriteLine(documents[0].Id);
            }
        }

        public static async Task MergeDocumentWithoutDownload(string[] args)
        {
            IApiConfigurator configuration = new WebMergeConfiguration("J8DFJU2K9Q4MRCVDBWIPL3MWZMXY", "HPMSNRXX", new Uri("https://www.webmerge.me"));
            var mergeObject = new { customer_name = "John", company_name = "Smith", company_address = "john.smith@example.com" };

            using (var client = new WebMergeClient(new HttpClient(), configuration))
            {
                var documents = await client.MergeDocumentAsync(196717, "daxl3x", mergeObject, true);

                Console.WriteLine(documents.Success);
            }
        }
    }
}
