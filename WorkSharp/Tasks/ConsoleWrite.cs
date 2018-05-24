using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WorkSharp.Tasks
{
    public class ConsoleWrite : IWorkflowTask
    {
        public IDictionary<string, dynamic> Definition { get; private set; }
        private Interpolator Interpolator { get; set; }

        public string MessageExpression { get; private set; }

        public ConsoleWrite(Interpolator interpolator)
        {
            Interpolator = interpolator;

        }
        public void InitializeFromJson(object definition)
        {
            Definition = (IDictionary<string, dynamic>)definition;
            MessageExpression = Definition["message"];
        }

        public async Task<object> Invoke(object context)
        {
            var interpolatedMessage = await Interpolator.InterpolateExpression(MessageExpression,
                new ContextFrame { Scope = context, Step = this });
            Console.WriteLine(interpolatedMessage);
            return interpolatedMessage;
        }

    }
}
