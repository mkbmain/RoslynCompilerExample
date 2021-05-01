using System;
using System.Linq;


namespace Compute
{
    static class Program
    {
        static void Main()
        {
            while (true)
            {
                var (baseValue, sequence) = GetInputs();
                var forumla = CodeBuilder.BaseSequenceFormulaBuilder(baseValue, sequence);
                var sourceCodeModel = CodeBuilder.BuildFunctionCode<double>(forumla);
                var result = RoslynCompiler.CompileAndExecuteLib(sourceCodeModel);
                Console.WriteLine(result);
            }
        }

        private static (int baseValue, string sequence) GetInputs()
        {
            Console.Write("enter your base value:");
            var baseValuestr = Console.ReadLine();
            if (!int.TryParse(baseValuestr, out var baseValue) || baseValue > 11|| baseValue < 2)
            {
                Console.WriteLine("Invalid baseValue");
            }

       
            Console.Write("enter your sequence:");
            var sequence = Console.ReadLine();
            var acceptableNumbers = string.Join("", Enumerable.Range(0, baseValue));

            if (sequence == null || sequence.ToCharArray().Any(f => !acceptableNumbers.Contains(f)))
            {
                Console.WriteLine("Invalid sequence");
                GetInputs();
            }

            return (baseValue, sequence);
        }
    }
}