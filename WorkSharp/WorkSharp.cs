using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using WorkSharp.Tasks;

namespace WorkSharp
{
    public class WorkSharp 
    {
        public Dictionary<string, Type> TaskTypes = new Dictionary<string, Type>();

        public ServiceCollection Services { get; } = new ServiceCollection();

        public IServiceProvider Provider { get; set; }
        public WorkSharp()
        {

            Services.AddSingleton(new Interpolator());
            Services.AddSingleton(this);

            RegisterTaskType<Tasks.ConsoleWrite>();
            RegisterTaskType<Tasks.Assign>();
            RegisterTaskType<Tasks.Delay>();
            RegisterTaskType<Tasks.HttpGet>();
            RegisterTaskType<Tasks.Sequence>();
        }

        public void RegisterTaskType<T>() where T : class
        {
            TaskTypes[typeof(T).Name] = typeof(T);
            Services.AddTransient<T>();
            Provider = Services.BuildServiceProvider();
        }

        public IWorkflowTask CreateFromJSON(object definition)
        {
            dynamic def = definition;
            string typeName = def._type;
            var taskType = TaskTypes[typeName];
            var taskInstance = (IWorkflowTask)Provider.GetRequiredService(taskType);
            taskInstance.InitializeFromJson(definition);
            return taskInstance;
        }
    }
}
