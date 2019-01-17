using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using static CsVm.Utils.CommandCode;

namespace CsVm
{
    public static class Compiler
    {
        private const string FileLocation = @"C:\Users\Tobyn\Documents\simple-virtual-machine-and-compiler\program.tasm";
        private static readonly List<int> FileStream = new List<int>();

        private static readonly Dictionary<Utils.CommandCode, string> CommandLookup = new Dictionary<Utils.CommandCode, string>
        {
            {Add, "add"},
            {LoadI, "loadi"},
            {Subtract, "sub"},
            {Multiply, "mul"},
            {Divide, "div"},
            {Load, "load"}
        };

        private static readonly Dictionary<string, int> RegisterLookup = new Dictionary<string, int>
        {
            { "r0", 0 },
            { "r1", 1 },
            { "r2", 2 },
            { "r3", 3 }
        };
        
        private static void ReadFileAndCompile()
        {
            foreach (var line in File.ReadLines(FileLocation))
                CompileLine(line);
            
            FileStream.Add(0x0000);
        }

        private static void CompileLine(string line)
        {
            var command = line.Split(' ').First();
            var values = line.Substring(line.IndexOf(" ", StringComparison.Ordinal) + 1).Split(',').Select(x => x.Trim()).ToArray();

            if (command == CommandLookup[LoadI]) CompileLoadI(ref values);
            else if (command == CommandLookup[Add]) CompileThreeRegisterCommand(ref values, Add);
            else if (command == CommandLookup[Subtract]) CompileThreeRegisterCommand(ref values, Subtract);
            else if (command == CommandLookup[Multiply]) CompileThreeRegisterCommand(ref values, Multiply);
            else if (command == CommandLookup[Divide]) CompileThreeRegisterCommand(ref values, Divide);
            else if (command == CommandLookup[Load]) CompileLoad(ref values);
            else
            {
                Console.WriteLine($"Invalid Syntax; {command} is not a valid command");
                throw new Exception();
            }
        }

        private static void CompileLoadI(ref string[] values)
        {
            if (values[1].ToCharArray().First() == '$' && CompileConstantHexadecimalValue(ref values[1]) > 255)
            {
                Console.WriteLine("Constant Too Large");
                throw new Exception();
            }

            var compiledCommand = LoadI.ToInt() << 12;
            compiledCommand = (CompileRegisterValue(ref values[0]) << 8) | compiledCommand;
            var charId = values[1].ToCharArray().First();

            switch (charId)
            {
                case '$':
                    compiledCommand += CompileConstantHexadecimalValue(ref values[1]);
                    break;
                case '#':
                    compiledCommand += CompileConstantDecimalValue(ref values[1]);
                    break;
                default:
                    Console.WriteLine($"Invalid Identifier {charId}");
                    throw new Exception();
            }

            FileStream.Add(compiledCommand);
        }

        private static void CompileLoad(ref string[] values)
        {
            var compiledCommand = Load.ToInt() << 12;
            compiledCommand = (CompileRegisterValue(ref values[0]) << 8) | compiledCommand;
            compiledCommand = (CompileRegisterValue(ref values[1]) << 4) | compiledCommand;

            FileStream.Add(compiledCommand);
        }

        private static void CompileThreeRegisterCommand(ref string[] values, Utils.CommandCode command)
        {
            var compiledCommand = command.ToInt() << 12;
            compiledCommand = (CompileRegisterValue(ref values[0]) << 8) | compiledCommand;
            compiledCommand = (CompileRegisterValue(ref values[1]) << 4) | compiledCommand;
            compiledCommand = (CompileRegisterValue(ref values[2])) | compiledCommand;

            FileStream.Add(compiledCommand);
        }

        private static int CompileRegisterValue(ref string value)
        {
            foreach (var i in RegisterLookup)
                if (i.Key == value) return i.Value;
            
            throw new Exception($"Register {value} not found.");
        }

        private static int CompileConstantDecimalValue(ref string value)
        {
            return int.Parse(value.Substring(1));
        }

        private static int CompileConstantHexadecimalValue(ref string value)
        {
            return int.Parse(value.Substring(1), NumberStyles.HexNumber);
        }

        public static int[] Compile()
        {
            ReadFileAndCompile();

            return FileStream.ToArray();
        }
    }
}
