using System.Collections.Generic;
using System.Threading.Tasks;

namespace WorkSharp
{

    public interface IWorkflowTask
    {
        IDictionary<string, dynamic> Definition { get; }

        Task<object> InvokeAsync(object context);

        void InitializeFromJson(object definition);
    }

}
