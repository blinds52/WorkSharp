
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Collections;

namespace WorkSharp.Tasks
{
    public class ForEach : IWorkflowTask
    {
        private Interpolator Interpolator { get; set; }
        private WorkSharp WorkSharp { get; set; }
        public string InExpression { get; set; }  
        public string VarName { get; set; }

        public IDictionary<string, dynamic> Definition { get; private set; }
        public IWorkflowTask LoopTask { get; private set; }
        public ForEach(Interpolator interpolator, WorkSharp workSharp)
        {
            Interpolator = interpolator;
            WorkSharp = workSharp;
        }

        public void InitializeFromJson(object definition)
        {
            Definition = (IDictionary<string, dynamic>)definition;
            VarName = Definition["var"];
            InExpression = Definition["in"];
            LoopTask = WorkSharp.CreateFromJSON(Definition["task"]);
        }

        public async Task<object> Invoke(object context)
        {
            dynamic ctx = context;
            var contextFrame = new ContextFrame { Scope = context, Step = this };
            var items = await Interpolator.InterpolateExpression(InExpression, contextFrame);
            var varName = VarName ?? "Scope.Item";
            if (items is IEnumerable itemsCollection)
            {
                foreach (var item in itemsCollection)
                {
                    await Interpolator.AssignValueOnDynamic(context, varName, item);
                    await LoopTask.Invoke(context);
                }
            }

            return true;
        }
    }
}
