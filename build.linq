<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\Microsoft.Build.Framework.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\Microsoft.Build.Engine.dll</Reference>
  <Namespace>Microsoft.Build.BuildEngine</Namespace>
  <Namespace>Microsoft.Build.Framework</Namespace>
</Query>

void Main(){
	Main(@"/logger:C:\TeamCity\buildAgent\plugins\dotnetPlugin\bin\JetBrains.BuildServer.MSBuildLoggers.dll,JetBrains.BuildServer.MSBuildLoggers.MSBuildLogger");
}

void Main(params string[] args)
{
	var engine = new Engine();
	var logger = GetLogger(args) ?? new ConsoleLogger();
	engine.RegisterLogger(logger);
	
	var project = new Project(engine);
	project.Load("LPRun.sln");
	project.Build();
}

ILogger GetLogger(string[] args)
{

	var loggerArg = args.SingleOrDefault (a => a.StartsWith("/logger:"));
	if (loggerArg == null) return null;
	
	var fullPath = loggerArg.Substring(7).Split(new[] {':'}, 2)[1]; //get the part after '/logger:'
	var parts = fullPath.Split(','); //split into ["assemblypath", "classpath"]
	var assembly = Assembly.LoadFrom(parts[0]);
	return (ILogger) assembly.CreateInstance(parts[1]);

}

void WriteError(ILogger logger){

}

