<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\Microsoft.Build.Framework.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\Microsoft.Build.Engine.dll</Reference>
  <Namespace>Microsoft.Build.BuildEngine</Namespace>
</Query>

void Main(){
	Main(new string[]{});
}
void Main(string[] args)
{
	Console.WriteLine("args:" + string.Join(" ", args));
	var engine = new Engine();
	engine.RegisterLogger(new ConsoleLogger());
	var project = new Project(engine);
	project.Load("LPRun.sln");
	
	if (!engine.BuildProject(project)){
		
		throw new Exception("Failure");
	}else{
		Console.WriteLine("Success!");
	}
}

// Define other methods and classes here
