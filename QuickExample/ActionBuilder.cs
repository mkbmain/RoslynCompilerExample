using System;
using System.Threading.Tasks;

namespace QuickExample
{
    public class ActionBuilder
    {

        public string Func { get; set; }


        internal SourceCodeModelForContractForFuncs _sourceCodeModelForLogRequest { get; set; } = null;

        public void Validate()
        {

            if (string.IsNullOrWhiteSpace(Func))
            {
                throw new ArgumentNullException(nameof(Func));
            }
            var model = CodeBuilder.BuildFunctionCode(Func);

            RoslynCompiler.CompileAndStore(model);
            _sourceCodeModelForLogRequest = model;
            GC.Collect();

        }
    }
}
