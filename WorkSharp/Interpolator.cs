using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WorkSharp
{


    public class Interpolator
    {
        public Dictionary<string, Script> Cache { get; } = new Dictionary<string, Script>();

        public async Task<object> InterpolateExpression<TContextType>(string script, TContextType executionContextFrame)
        {
            var refs = new List<MetadataReference>{
                MetadataReference.CreateFromFile(typeof(Microsoft.CSharp.RuntimeBinder.RuntimeBinderException).GetTypeInfo().Assembly.Location),
                MetadataReference.CreateFromFile(typeof(System.Runtime.CompilerServices.DynamicAttribute).GetTypeInfo().Assembly.Location),
                MetadataReference.CreateFromFile(typeof(WorkSharp).GetTypeInfo().Assembly.Location)
            };
            var options = ScriptOptions
              .Default
              .AddReferences(refs)
              .AddReferences(typeof(System.Linq.Enumerable).Assembly)
              .AddImports("System", "System.Collections.Generic", "System.Linq", "WorkSharp.Functions");

            //var s = CSharpScript.Create<object>("$\"" + script     + "\"", options, typeof(TContextType));
            var s = CSharpScript.Create<object>(script, options, typeof(TContextType));
            s.Compile();
            var r = await s.RunAsync(executionContextFrame);
            return r.ReturnValue;
        }

        public Task<object> AssignValueOnDynamic(object context, string variableName, object value)
        {
            return InterpolateExpression($"{variableName} = Marshal.Result",
                new ContextAssignmentFrame { Scope = context, Marshal = new Marshal { Result = value } });
        }

        public Task<object> AppendValueOnDynamic(object context, string listName, object value)
        {
            return InterpolateExpression($"{listName}.Add(Marshal.Result); {listName}",
                new ContextAssignmentFrame { Scope = context, Marshal = new Marshal { Result = value } });
        }

    }
}
