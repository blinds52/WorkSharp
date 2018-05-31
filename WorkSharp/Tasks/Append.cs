using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WorkSharp.Tasks
{
    public class Append : IWorkflowTask
    {

        public IDictionary<string, dynamic> Definition { get; private set; }
        private Interpolator Interpolator { get; set; }

        public string Name { get; private set; }
        public dynamic Expression { get; private set; }

        public Append(Interpolator interpolator)
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

            var evaluationContext = new ContextFrame { Scope = context, Step = this, };

            // check the existence of the object
             var check = false;
            try
            {
                await Interpolate(Name);
                check = true;
            }
            catch (Exception)
            {
            }

            if (!check)
            {
                // create list if not exist
                var List = new List<dynamic>();
                await Interpolator.AssignValueOnDynamic(context, Name, List);
            }

            var expressionValue = await Interpolate(Expression);
            var result = await AppendValueOnDynamic(Name, expressionValue);
            
            Task<object> Interpolate(string expression) => Interpolator.InterpolateExpression(expression, evaluationContext);
            Task<object> AppendValueOnDynamic(string name, dynamic value) => Interpolator.AppendValueOnDynamic(context, name, value);

            return result;
           
        }
    }
}
