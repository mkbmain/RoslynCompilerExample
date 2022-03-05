using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Linq;

namespace QuickExample
{
    public static class RoslynCompiler
    {
        private static bool isManaged(string path)
        {
            try
            {
                var b = AssemblyName.GetAssemblyName(path);
                return true;
            }
            catch (Exception e)
            {

            }

            return false;
        }

        private static string[] DllToInclude = System.IO.Directory.GetFiles(Path.GetDirectoryName(typeof(object).Assembly.Location))
            .Where(t => t.EndsWith(".dll"))
     .Where(t => isManaged(t)).ToArray();


        private static MetadataReference[] MetadataReferences = DllToInclude
     .Select(t => MetadataReference.CreateFromFile(t))
     .Union(new[] { MetadataReference.CreateFromFile(typeof(ContractForFuncs).Assembly.Location) }).ToArray();


        public static void CompileAndStore<T>(SourceCodeModel<T> sourceCodeModel)
        {


            // analyse and generate IL code from syntax tree
            var compilation = CSharpCompilation.Create(
                Path.GetRandomFileName(), // assembly name 
                new[] { CSharpSyntaxTree.ParseText(sourceCodeModel.Code) },
                MetadataReferences,
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            using (var ms = new MemoryStream())
            {
                // write IL code into memory
                var compileResult = compilation.Emit(ms);

                if (!compileResult.Success)
                {
                    // handle exceptions
                    var failures = compileResult.Diagnostics.Where(diagnostic =>
                            diagnostic.IsWarningAsError || diagnostic.Severity == DiagnosticSeverity.Error)
                        .Select(diagnostic => $"{diagnostic.Id}: {diagnostic.GetMessage()}");
                    Console.Error.Write(string.Join(Environment.NewLine, failures));
                    throw new Exception($"Compelation Failed : {Environment.NewLine}{string.Join(Environment.NewLine, failures)}");
                }

                // load this 'virtual' DLL so that we can use
                ms.Seek(0, SeekOrigin.Begin);

                // create instance of the desired class and call the desired function
                var assembly = Assembly.Load(ms.ToArray());
                var type = assembly.GetType($"{sourceCodeModel.NameSpace}.{sourceCodeModel.Class}");
                var propss = type.GetMethod(sourceCodeModel.Method);
                sourceCodeModel.Item = (T)type.InvokeMember(sourceCodeModel.Method,
                    BindingFlags.Default | BindingFlags.InvokeMethod,
                    System.Type.DefaultBinder,
                    Activator.CreateInstance(type),
                    null);
            }

          
        }
    }
}
