using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using MongoDB.Driver;
using FunctionApp1.Models;
using FunctionApp1.Mongo;
using System.Net;

namespace FunctionApp1
{
    public static class Function1
    {
        private static readonly IMongoLayer mongoLayer;

        static Function1()
        {
            //TODO: DI
            var connectionString = Environment.GetEnvironmentVariable("ConnectionString");
            var databaseName = Environment.GetEnvironmentVariable("Database");
            var database = GetMongoDatabase(connectionString, databaseName);

            mongoLayer = new MongoLayer(database);
        }

        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "post")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Function1 called");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<Test>(requestBody);

            try
            {
                mongoLayer.InsertDocument("test", data);
            }
            catch (Exception exc)
            {
                log.LogInformation($"{exc.Message}: {exc.StackTrace}");
                return new StatusCodeResult((int)HttpStatusCode.BadRequest);
            }

            return new OkResult();
        }


        private static IMongoDatabase GetMongoDatabase(string connection, string databaseName)
        {
            return new MongoClient(connection).GetDatabase(databaseName);
        }
    }
}
