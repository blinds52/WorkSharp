using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace WorkSharp.Tasks
{
    public class Sequence : IWorkflowTask
    {
        private Interpolator Interpolator { get; set; }
        public List<IWorkflowTask> Items { get; private set; }
        public IDictionary<string, dynamic> Definition { get; private set; }
        public WorkSharp WorkSharp { get; set; }
        public Sequence(Interpolator interpolator, WorkSharp workSharp)
        {
            Interpolator = interpolator;
            WorkSharp = workSharp;
        }

        public void InitializeFromJson(object definition)
        {
            Definition = (IDictionary<string, dynamic>)definition;

            IEnumerable<object> items = (IEnumerable<dynamic>)Definition["items"];
            Items = items.Select(item => WorkSharp.CreateFromJSON(item)).ToList();
        }

        public async Task<object> InvokeAsync(object context)
        {
            dynamic ctx = context;

            foreach (var item in Items)
            {
                var itemResult = await item.InvokeAsync(context);
                if (item.Definition.ContainsKey("_resultTo"))
                {
                    var contextFrame = new ContextFrame { Scope = context, Step = this };
                    var key = item.Definition["_resultTo"];
                    await Interpolator.AssignValueOnDynamic(context, key, itemResult);
                }
            }
            return await Task.FromResult((object)true);
        }
    }
}
