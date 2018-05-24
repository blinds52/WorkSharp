using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace WorkSharp.Tasks
{
    public class Delay : IWorkflowTask
    {

        public IDictionary<string, dynamic> Definition { get; private set; }
        private Interpolator Interpolator { get; set; }

        public string DurationExpression { get; private set; }

        public Delay(Interpolator interpolator)
        {
            Interpolator = interpolator;
        }

        public void InitializeFromJson(object definition)
        {
            Definition = (IDictionary<string, dynamic>)definition;
            DurationExpression = ((object)Definition["duration"]).ToString();
        }

        public async Task<object> Invoke(object context)
        {
            var durationValue = await interpolate(DurationExpression, new ContextFrame { Scope = context, Step = this });
            await Task.Delay((int)durationValue);
            return (int)durationValue;
            Task<object> interpolate<T>(string expression, T contextFrame) => Interpolator.InterpolateExpression(expression, contextFrame);
           
        }
    }
}
