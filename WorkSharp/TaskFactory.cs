using System;
using System.Collections.Generic;
using System.Text;

namespace WorkSharp
{
    public class TaskFactory<T> : ITaskFactory where T : IWorkflowTask
    {
        private Interpolator Interpolator { get; }

        public TaskFactory(Interpolator interpolator)
        {
            Interpolator = interpolator;
        }

        public IWorkflowTask CreateInstance(object definition)
        {
            return (T)Activator.CreateInstance(typeof(T), definition, Interpolator);
        }
    }

    public interface ITaskFactory
    {

        IWorkflowTask CreateInstance(dynamic definition);

    }
}
