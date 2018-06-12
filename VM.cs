using System;
using static CsVm.Utils.CommandCode;

namespace CsVm
{
    public class VM
    {
        private const int NumberOfRegisters = 4;
        private readonly int[] _program;
        private readonly int[] _registers;
        private int _pc;
        private int _register1, _register2, _register3, _immediateValue;
        private Utils.CommandCode _instruction;
        private bool _isRunning;

        public VM(int[] programStream)
        {
            /*
             * 1.) operation type
             * 2.) register
             * 3-4.) immediate value or target registers
             */
            _program = programStream;
            _registers = new int[NumberOfRegisters];
            _pc = 0;
            _register1 = _register2 = _register3 = _immediateValue = 0;
            _instruction = Halt;
            _isRunning = true;
        }

        protected int Fetch()
        {
            return _program[_pc++];
        }

        protected void Decode(int instruction)
        {
            _instruction = (Utils.CommandCode)((instruction & 0xF000) >> 12);
            _register1 = (instruction & 0xF00) >> 8;
            _register2 = (instruction & 0xF0) >> 4;
            _register3 = instruction & 0xF;
            _immediateValue = instruction & 0xFF;
        }

        protected void Eval()
        {
            switch (_instruction)
            {
                case Halt:
                    //halt
                    Console.Write(Environment.NewLine);
                    Console.WriteLine("Program Finished Executing.");
                    Console.WriteLine("Press Any Key To Exit...");
                    _isRunning = false;
                    break;
                case LoadI:
                    //load i
                    Console.WriteLine($"loadi r{_register1} #{_immediateValue}");
                    _registers[_register1] = _immediateValue;
                    break;
                case Add:
                    //add
                    Console.WriteLine($"add r{_register1} r{_register2} r{_register3}");
                    _registers[_register3] = _registers[_register1] + _registers[_register2];
                    break;
                case Subtract:
                    //subtract
                    Console.WriteLine($"subtract r{_register1} r{_register2} r{_register3}");
                    _registers[_register3] = _registers[_register1] - _registers[_register2];
                    break;
                case Multiply:
                    //subtract
                    Console.WriteLine($"subtract r{_register1} r{_register2} r{_register3}");
                    _registers[_register3] = _registers[_register1] * _registers[_register2];
                    break;
                case Divide:
                    //subtract
                    Console.WriteLine($"subtract r{_register1} r{_register2} r{_register3}");
                    _registers[_register3] = _registers[_register1] / _registers[_register2];
                    break;
            }
        }

        private void DisplayRegisters()
        {
            Console.Write("regs = ");
            for (byte i = 0; i < NumberOfRegisters; i++)
            {
                Console.Write($"{_registers[i]:X4} ");
            }
            Console.Write(Environment.NewLine);
        }

        public void Run()
        {
            while (_isRunning)
            {
                DisplayRegisters();
                Decode(Fetch());
                Eval();
            }
        }
    }
}
