using System;
using System.Collections.Generic;
using System.Text;

namespace WorkSharp
{
    public class ContextFrame
    {
        public dynamic Scope { get; set; }
        public dynamic Step { get; set; }
    }

    public class Marshal
    {
        public dynamic Result { get; set; }
    }

    public class ContextAssignmentFrame
    {
        public dynamic Scope { get; set; }
        public Marshal Marshal { get; set; }

        public ContextAssignmentFrame() { }

        public ContextAssignmentFrame(dynamic scope, object valueToAssign)
        {
            Scope = scope;
            Marshal = new Marshal { Result = valueToAssign };
        }
    }
}
