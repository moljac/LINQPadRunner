<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\Microsoft.Build.Engine.dll</Reference>
</Query>

void Main()
{
	var x = typeof(Microsoft.Build.BuildEngine.Project);
	File.WriteAllText("hello.txt", "world");
}

// Define other methods and classes here