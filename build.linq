<Query Kind="Statements">
  <Reference>&lt;RuntimeDirectory&gt;\Microsoft.Build.Engine.dll</Reference>
  <Namespace>Microsoft.Build.BuildEngine</Namespace>
</Query>

var engine = new Engine();
var project = new Project(engine);
project.Load("LPRun.sln");

if (!engine.BuildProject(project)){
	Environment.ExitCode = 1;
	Console.WriteLine("Failure!");
}else{
	Console.WriteLine("Success!");
}