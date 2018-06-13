using System;
using System.Collections.Generic;
using System.Text;

namespace CsVm
{
    public static class Utils
    {
        public enum CommandCode
        {
            Halt,
            LoadI,
            Add,
            Subtract,
            Multiply,
            Divide,
            Load
        }

        public static int ToInt(this CommandCode e)
        {
            return (int)e;
        }
    }
}
