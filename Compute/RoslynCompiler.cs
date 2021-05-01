using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Compute
{
    public static class RoslynCompiler
    {
        public static T CompileAndExecuteLib<T>(SourceCodeModel<T> sourceCodeModel)
        {
            var references = new[]
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location)
            };

            // analyse and generate IL code from syntax tree
            var compilation = CSharpCompilation.Create(
                Path.GetRandomFileName(), // assembly name 
                new[] {CSharpSyntaxTree.ParseText(sourceCodeModel.Code)},
                references,
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            using var ms = new MemoryStream();
            // write IL code into memory
            var compileResult = compilation.Emit(ms);

            if (!compileResult.Success)
            {
                // handle exceptions
                var failures = compileResult.Diagnostics.Where(diagnostic =>
                        diagnostic.IsWarningAsError ||
                        diagnostic.Severity == DiagnosticSeverity.Error)
                    .Select(diagnostic => $"{diagnostic.Id}: {diagnostic.GetMessage()}");
                Console.Error.Write(string.Join(Environment.NewLine, failures));
                return default;
            }

            // load this 'virtual' DLL so that we can use
            ms.Seek(0, SeekOrigin.Begin);

            // create instance of the desired class and call the desired function
            var type = Assembly.Load(ms.ToArray()).GetType($"{sourceCodeModel.NameSpace}.{sourceCodeModel.Class}");
            var result = type.InvokeMember(sourceCodeModel.Method,
                BindingFlags.Default | BindingFlags.InvokeMethod,
                System.Type.DefaultBinder,
                Activator.CreateInstance(type),
                null);

            sourceCodeModel.Data = (T) result;
            return (T) result;
        }
    }
}