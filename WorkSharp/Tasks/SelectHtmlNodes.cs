using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WorkSharp.Tasks
{
    public class SelectHtmlNodes : IWorkflowTask
    {
        public IDictionary<string, dynamic> Definition { get; private set; }
        private Interpolator Interpolator { get; set; }

        public string DocumentNode { get; private set; }
        public string Selector { get; private set; }
        public string NodeExpression { get; private set; }

        public SelectHtmlNodes(Interpolator interpolator)
        {
            Interpolator = interpolator;
        }

        public void InitializeFromJson(object definition)
        {
            Definition = (IDictionary<string, dynamic>)definition;
            DocumentNode = Definition["from"];
            Selector = Definition["selector"];
            //NodeExpression = Definition["expression"];
        }

        public async Task<object> Invoke(object context)
        {

            // get html text
            var contextFrame = new ContextFrame { Scope = context, Step = this };
            var htmlDoc = (HtmlDocument)await interpolate(DocumentNode);
            //var selector = (string)await interpolate(Selector);

            // run selector 
            HtmlNodeCollection nodes = htmlDoc.DocumentNode
                .SelectNodes(Selector);

            // run ndoe expression
            //var nodeContextFrame = new NodeContextFrame { NodeList = nodes };
            //return await Interpolator.InterpolateExpression(NodeExpression, nodeContextFrame);
            return nodes;

            Task<object> interpolate(string expression) => Interpolator.InterpolateExpression(expression, contextFrame);

        }
    }

    public class NodeContextFrame
    {
        public dynamic NodeList { get; set; }
    }
}
