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
            LPRun.Program.ExecuteFile(null, "Statement_WriteHelloTxtFile.linq");
            Assert.AreEqual("world", File.ReadAllText("hello.txt"));
        }
        [Test]
        public void ProgramsGetExecuted()
        {
            File.Delete("hello.txt");
            LPRun.Program.ExecuteFile(null, "Program_WriteHelloTxtFile.linq");
            Assert.AreEqual("world", File.ReadAllText("hello.txt"));
        }

    }
}
