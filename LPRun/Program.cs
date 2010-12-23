using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;
using LPRun.Linqpad;
using Microsoft.CSharp;

namespace LPRun
{
    public class Program
    {
        public static List<string> StandardNamespaces
        {
            get
            {
                var strings = new List<string>
                {
                    "System",
                    "System.IO",
                    "System.Text",
                    "System.Text.RegularExpressions",
                    "System.Diagnostics",
                    "System.Threading",
                    "System.Reflection",
                    "System.Collections",
                    "System.Collections.Generic",
                    "System.Linq",
                    "System.Linq.Expressions",
                    "System.Data",
                    "System.Data.SqlClient",
                    "System.Data.Linq",
                    "System.Data.Linq.SqlClient",
                    "System.Xml",
                    "System.Xml.Linq",
                    "System.Xml.XPath"
                };
                return strings;
            }
        }
        public static List<string> Assemblies
        {
            get
            {
                var strings = new List<string>
                {
                    "System.dll",
                    "System.Core.dll",
                    "System.Data.dll",
                    "System.Xml.dll",
                    "System.Xml.Linq.dll",
                    "System.Data.Linq.dll",
                    "System.Drawing.dll",
                    "System.Data.DataSetExtensions.dll"
                };

                return strings;
            }
        }

        private const string NamespaceStart = @"namespace LPRun.Generated {";
        private const string ClassStart = @"public class Program {";
        private const string MethodStart = @"public void Main() {";
        private const string MethodEnd = @"}";

        private const string ClassEnd = @"}";
        private const string NamespaceEnd = "}";
        public static void Main(string[] args)
        {
            var path = args.First();
            args = args.Skip(1).ToArray();
            ExecuteFile(path, args);
        }

        public static void ExecuteFile(string path, params string[] args)
        {
            args = args ?? new string[] {};
            var content = File.ReadAllLines(path);

            var xml = string.Join("\r\n", content.TakeWhile(l => l.Trim().StartsWith("<")));

            var queryElement = XDocument.Parse(xml).Element("Query");
            var query = new Query
                            {
                                Kind = queryElement.Attribute("Kind").Value,
                                Namespaces = queryElement.Elements("Namespace").Select(n => n.Value).ToList(),
                                GACReferences = queryElement.Elements("GACReference").Select(n => n.Value).ToList(),
                                RelativeReferences = queryElement.Elements("Reference").Where(e => e.Attribute("Relative") != null).Select(n => n.Attribute("Relative").Value).ToList(),
                                OtherReferences = queryElement.Elements("Reference").Where(e => e.Attribute("Relative") == null).Select(n => n.Value.Replace("<RuntimeDirectory>", RuntimeEnvironment.GetRuntimeDirectory())).ToList(),

                            };
            var code = string.Join("\r\n", content.SkipWhile(l => l.Trim().StartsWith("<")));

            var codeBuilder = new StringBuilder();
            codeBuilder.AppendLine("using " + string.Join(";\r\nusing ", query.Namespaces.Union(StandardNamespaces)) + ";");
            codeBuilder.AppendLine(NamespaceStart);
            codeBuilder.AppendLine(ClassStart);

            if (query.Kind != "Program")
                codeBuilder.AppendLine(MethodStart);

            
            codeBuilder.AppendLine(code);
            
            if (query.Kind != "Program")
                codeBuilder.AppendLine(MethodEnd);
            codeBuilder.AppendLine(ClassEnd);
            codeBuilder.AppendLine(NamespaceEnd);
            
            var output = ExecuteCode(query, codeBuilder.ToString(), "LPRun.Generated", "Program", "Main", false, args);
            Console.WriteLine(output);
        }

     
        static object ExecuteCode(Query query, string code, string namespacename, string classname, string functionname, bool isstatic, string[] args)
        {
            object returnval = null;
            Assembly asm = BuildAssembly(query, code);
            object instance = null;
            Type type = null;
            if (isstatic)
            {
                type = asm.GetType(namespacename + "." + classname);
            }
            else
            {
                instance = asm.CreateInstance(namespacename + "." + classname);
                type = instance.GetType();
            }
            var mainMethods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static).Where(m => m.Name == functionname).OrderByDescending(m => m.GetParameters().Count());
            var mainMethodWithArgs =
                mainMethods.SingleOrDefault(
                    m => m.GetParameters().SingleOrDefault(p => p.ParameterType == typeof (string[])) != null);

            if(mainMethodWithArgs != null)
                return mainMethodWithArgs.Invoke(instance, new object[]{args});

            var mainMethodWithoutArgs = mainMethods.SingleOrDefault(m => m.GetParameters().Count() == 0);
            if(mainMethodWithoutArgs != null)
                return mainMethodWithoutArgs.Invoke(instance, new object[]{});
            throw new Exception("No suitable Main methods found");

        }

        private static Assembly BuildAssembly(Query query, string code)
        {
            var provOptions = new Dictionary<string, string>();
            provOptions.Add("CompilerVersion", "v4.0");
            var provider = new CSharpCodeProvider(provOptions);
            var assemblies = new[]
                                 {
                                     query.GACReferences.Select(s => s.Substring(0, s.IndexOf(",")) + ".dll"),
                                     query.RelativeReferences,
                                     query.OtherReferences,
                                     Assemblies,
                                 };

            var compilerparams = new CompilerParameters(assemblies.SelectMany(a => a).ToArray())
            {
                GenerateExecutable = false,
                GenerateInMemory = true
            };

var results = provider.CompileAssemblyFromSource(compilerparams, code);
            if (results.Errors.HasErrors)
            {
                StringBuilder errors = new StringBuilder("Compiler Errors :\r\n");
                foreach (CompilerError error in results.Errors)
                {
                    errors.AppendFormat("Line {0},{1}\t: {2}\n",
                                        error.Line, error.Column, error.ErrorText);
                }
                throw new Exception("Errors compiling:\r\n" + code + "\r\n\r\n" + errors.ToString());
            }
            else
            {
                return results.CompiledAssembly;
            }
        }
    }
}