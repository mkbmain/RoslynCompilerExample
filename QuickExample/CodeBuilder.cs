namespace QuickExample
{
    public static class CodeBuilder
    {
        public static SourceCodeModelForContractForFuncs BuildFunctionCode(string code)
        {
            const string Base = @" using QuickExample;
using System;
using System.Threading.Tasks;

 namespace TBD.Roslyn
{
  
        public class CodeGen
        {
            public Func<ContractForFuncs, Task<bool>> Run()
            {
                return te;
            }

            private static Func<ContractForFuncs, Task<bool>> te = async request =>

{
             
                  |CODE|
  return true;         
 }             
    
            ;
        }
    
}
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
