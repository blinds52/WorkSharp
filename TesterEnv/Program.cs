using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections;
using System.Dynamic;
using System.IO;
using System.Threading.Tasks;
using WorkSharp;
using System.Collections.Generic;

namespace TesterEnv
{
    public class Program
    {
        public static async Task Main(string[] args)
        {

            await RunConfig(@"C:\_tes\service-jobs-scraper\service-jobs-scraper\ScraperConfigs\teach-nsw-edu-au.json");

            String command;
            Boolean quitNow = false;
            while (!quitNow)
            {

                Console.WriteLine("Enter the json config file.");
                command = Console.ReadLine();
                if (command == "exit") quitNow = true;

                await RunConfig(command);

            }
        }

        private static async Task RunConfig(string jsonName)
        {

            var jsonText = "";
            try
            {
                if (!jsonName.Contains(":\\"))
                {
                    jsonName = Directory.GetCurrentDirectory() + "\\" + jsonName;
                }

                jsonText = File.ReadAllText(jsonName);
                ExpandoObject json = JsonConvert.DeserializeObject<ExpandoObject>(jsonText);

                var ws = new WorkSharp.WorkSharp();
                var wf = ws.CreateFromJSON(json);
                var r = await wf.Invoke(new ExpandoObject());
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Invalid json file name!");
            }
            catch (Exception e)
            {
                throw;
            }

        }

    }

}
