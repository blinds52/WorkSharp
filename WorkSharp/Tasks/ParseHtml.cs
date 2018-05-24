using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WorkSharp.Tasks
{
    class ParseHtml : IWorkflowTask
    {
        public IDictionary<string, dynamic> Definition { get; private set; }
        private Interpolator Interpolator { get; set; }

        public string HtmlExpression { get; private set; }

        public ParseHtml(Interpolator interpolator)
        {
            Interpolator = interpolator;
        }

        public void InitializeFromJson(object definition)
        {
            Definition = (IDictionary<string, dynamic>)definition;
            HtmlExpression = Definition["html"];
        }

        public async Task<object> Invoke(object context)
        {

            // get html text
            var contextFrame = new ContextFrame { Scope = context, Step = this };
            var htmlString = (string)await interpolate(HtmlExpression);


            // parse html to document object
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlString);
            return htmlDoc;

            Task<object> interpolate(string expression) => Interpolator.InterpolateExpression(expression, contextFrame);

        }
    }
}
