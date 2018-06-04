using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Web;

namespace WorkSharp.Tasks
{
    public class HttpGet : IWorkflowTask
    {

        public IDictionary<string, dynamic> Definition { get; private set; }
        private Interpolator Interpolator { get; set; }
        public string UrlExpression { get; private set; }
        public string IsJSONExpression { get; private set; }
        public string ConsoleWrite { get; private set; }

        public HttpGet(Interpolator interpolator)
        {
            Interpolator = interpolator;
        }


        public void InitializeFromJson(object definition)
        {
            Definition = (IDictionary<string, dynamic>)definition;
            UrlExpression = Definition["url"];
            IsJSONExpression = Definition.ContainsKey("isJSON") ? Definition["isJSON"] : "true";
            ConsoleWrite = Definition.ContainsKey("consoleWrite") ? Definition["consoleWrite"] : "true";
        }

        public async Task<object> Invoke(object context)
        {
            var contextFrame = new ContextFrame { Scope = context, Step = this };
            var url = (string)await interpolate(UrlExpression);
            var console = (bool)await interpolate(ConsoleWrite);
            url = HttpUtility.HtmlDecode(url);

            var isJSON = (bool)await interpolate(IsJSONExpression);
            var client = new HttpClient();
            if (console)
            {
                Console.WriteLine($"Request with HTTP GET from: ${url}... hold your horses.");
            }
            var response = await client.GetAsync(url);
            var resultString = await response.Content.ReadAsStringAsync();
            if (!isJSON)
            {
                return resultString;
            }
            return JsonConvert.DeserializeObject<dynamic>(resultString);

            Task<object> interpolate(string expression) => Interpolator.InterpolateExpression(expression, contextFrame);
           
        }
    }
}
