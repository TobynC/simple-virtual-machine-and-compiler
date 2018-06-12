using System;
using System.Collections.Generic;
using System.Text;

namespace CsVm
{
    public static class Utils
    {
        public enum CommandCode:int
        {
            Halt,
            LoadI,
            Add,
            Subtract,
            Multiply,
            Divide
        }

        public static int ToInt(this CommandCode e)
        {
            return (int)e;
        }
    }
}
