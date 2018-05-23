using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WorkSharp.Tasks
{
    public class Assign : IWorkflowTask
    {

        public IDictionary<string, dynamic> Definition { get; private set; }
        private Interpolator Interpolator { get; set; }
        public string Name { get; private set; }
        public string Expression { get; private set; }

        public Assign(Interpolator interpolator)
        {
            Interpolator = interpolator;
        }


        public void InitializeFromJson(object definition)
        {
            Definition = (IDictionary<string, dynamic>)definition;
            Name = Definition["name"];
            Expression = Definition["expression"];
        }

        public async Task<object> Invoke(object context)
        {
            var expressionValue = await interpolate(Expression, new ContextFrame { Scope = context, Step = this });
            await interpolate($"{Name} = Marshal.Result", new ContextAssignmentFrame(context, expressionValue));
            return true;

            Task<object> interpolate<T>(string expression, T contextFrame) => Interpolator.InterpolateExpression(expression, contextFrame);
           
        }
    }
}
