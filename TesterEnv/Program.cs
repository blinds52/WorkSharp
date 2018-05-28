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
            var jsonText = File.ReadAllText(Directory.GetCurrentDirectory() + "\\for-each.json");
            ExpandoObject json = JsonConvert.DeserializeObject<ExpandoObject>(jsonText);
            var ws = new WorkSharp.WorkSharp();
            var wf = ws.CreateFromJSON(json);
            var r = await wf.Invoke(new ExpandoObject());
            Console.ReadLine();
        }
    }
}
