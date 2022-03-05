namespace QuickExample
{
    public static class CodeBuilder
    {
        public static SourceCodeModelForContractForFuncs BuildFunctionCode(string code)
        {
            const string Base = @"using QuickExample;
using System;
using System.Threading.Tasks;

namespace TBD.Roslyn
{

    public class CodeGen
    {
        public Func<ContractForFuncs, Task> Run() => AddonEntry;

          private static async Task AddonEntry(ContractForFuncs contract)|CODE| }}
";
            return new SourceCodeModelForContractForFuncs
            {
                Code = Base.Replace("|CODE|", code),
                NameSpace = "TBD.Roslyn",
                Class = "CodeGen",
                Method = "Run",
            };
        }
    }
}
