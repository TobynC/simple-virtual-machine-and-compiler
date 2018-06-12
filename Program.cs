using System;

namespace CsVm
{
    class Program
    {
        static void Main(string[] args)
        {
            var virtualMahine = new VM(Compiler.Compile());
            virtualMahine.Run();

            Console.ReadLine();
        }
    }
}
