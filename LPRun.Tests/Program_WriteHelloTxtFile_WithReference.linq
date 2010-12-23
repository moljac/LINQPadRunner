<Query Kind="Program">
  <Reference Relative="..\packages\NUnit.2.5.7.10213\lib\nunit.framework.dll">D:\projects\LPRun\packages\NUnit.2.5.7.10213\lib\nunit.framework.dll</Reference>
</Query>

void Main()
{
	var x = typeof(NUnit.Framework.Assert);
	File.WriteAllText("hello.txt", "world");
}

// Define other methods and classes here