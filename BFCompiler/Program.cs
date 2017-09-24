using System;
using System.IO;
using Microsoft.CSharp;
using System.CodeDom.Compiler;

namespace BFCompiler
{
    class Program
    {
        public static string Code = "using System;class Program{static void Main(string[] args){int c=0;";
        static void Main(string[] args)
        {
            try
            {
                Console.Title = "Brainfuck Compiler";
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("Enter your Brainfuck source file path: ");
                string bfCode = File.ReadAllText(Console.ReadLine());
                Console.Write("Enter memory size (in bytes): ");
                int memSize;
                try
                {
                    memSize = int.Parse(Console.ReadLine());
                }
                catch { memSize = 64 * 1024; }
                Console.Write("Enter cell type: (i. e. int): ");
                string cellType = Console.ReadLine();
                Console.Write("Enter output file path: ");
                string outputPath = Console.ReadLine();
                Code += $"{cellType}[] m=new {cellType}[{memSize}];";
                foreach (char c in bfCode)
                {
                    switch (c)
                    {
                        case '>': Code += "c++;";                               break;
                        case '<': Code += "c--;";                               break;
                        case '+': Code += "m[c]++;";                            break;
                        case '-': Code += "m[c]--;";                            break;
                        case '[': Code += "while(m[c]!=0){";                    break;
                        case ']': Code += "}";                                  break;
                        case '.': Code += "Console.Write((char)m[c]);";         break;
                        case ',': Code += "m[c]=Console.ReadKey().KeyChar;";    break;
                    }
                }
                Code += "}}";
                File.WriteAllText("work.cs", Code);
                Console.ForegroundColor = ConsoleColor.White;
                CSharpCodeProvider codeProvider = new CSharpCodeProvider();
                ICodeCompiler icc = codeProvider.CreateCompiler();
                CompilerParameters parameters = new CompilerParameters();
                parameters.GenerateExecutable = true;
                parameters.OutputAssembly = outputPath;
                CompilerResults res = icc.CompileAssemblyFromSource(parameters, Code);
                Console.BackgroundColor = ConsoleColor.Red;
                foreach(CompilerError err in res.Errors)
                    Console.WriteLine($"[{err.ErrorNumber}][Line {err.Line}][Column {err.Column}] {err.ErrorText}");
                File.Delete("work.cs");
                Console.BackgroundColor = ConsoleColor.Green;
                Console.WriteLine("Compilation finished. You now can run your compiled Brainfuck program (unless there are compilation errors).");
                Console.ReadLine();
            }
            catch(Exception e) { Console.WriteLine($"Error:\r\n{e}"); }
        }
    }
}
