using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace LPRun.Tests
{

    public class Tests
    {
        [Test]
        public void StatementsGetExecuted()
        {
            File.Delete("hello.txt");
            LPRun.Program.ExecuteFile("Statement_WriteHelloTxtFile.linq", null);
            Assert.AreEqual("world", File.ReadAllText("hello.txt"));
        }
        [Test]
        public void ProgramsGetExecuted()
        {
            File.Delete("hello.txt");
            LPRun.Program.ExecuteFile("Program_WriteHelloTxtFile.linq", null);
            Assert.AreEqual("world", File.ReadAllText("hello.txt"));
        }
        [Test]
        public void NamespacesGetImported()
        {
            File.Delete("hello.txt");
            LPRun.Program.ExecuteFile("Program_WriteHelloTxtFile_WithNamespaceImports.linq", null);
            Assert.AreEqual("world", File.ReadAllText("hello.txt"));
        }
        [Test]
        public void CommandLineArgumentsAreExecuted()
        {
            File.Delete("args.txt");
            LPRun.Program.ExecuteFile("Program_WriteHelloTxtFile_WithCommandLineArguments.linq", "args.txt", "heyhey");
            Assert.AreEqual("heyhey", File.ReadAllText("args.txt"));
        }
        [Test]
        public void GACReferencesGetImported()
        {
            File.Delete("args.txt");
            LPRun.Program.ExecuteFile("Program_WriteHelloTxtFile_WithGACReference.linq", "hello.txt", "world");
            Assert.AreEqual("world", File.ReadAllText("hello.txt"));
        }
        [Test]
        public void ReferencesGetImported()
        {
            File.Delete("args.txt");
            LPRun.Program.ExecuteFile("Program_WriteHelloTxtFile_WithGACReference.linq", "hello.txt", "world");
            Assert.AreEqual("world", File.ReadAllText("hello.txt"));
        }

    }
}
