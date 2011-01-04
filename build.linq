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
	ILogger logger = GetLogger(args) ?? new ConsoleLogger();
	var dummyEventSource = new DummyEventSource();
	logger.Initialize(dummyEventSource);
	var errorArg= new BuildErrorEventArgs("Subcategory", "code", "file", 0, 0, 0, 0, "Message", "Helpkeyword", "sender");
	dummyEventSource.InvokeErrorRaised(errorArg);
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

 

public class DummyEventSource : IEventSource
{
	public event BuildMessageEventHandler MessageRaised;
	public event BuildErrorEventHandler ErrorRaised;

	public void InvokeErrorRaised(BuildErrorEventArgs e)
	{
		BuildErrorEventHandler handler = ErrorRaised;
		if (handler != null) handler(this, e);
	}

	public event BuildWarningEventHandler WarningRaised;

	public void InvokeWarningRaised(BuildWarningEventArgs e)
	{
		BuildWarningEventHandler handler = WarningRaised;
		if (handler != null) handler(this, e);
	}

	public event BuildStartedEventHandler BuildStarted;
	public event BuildFinishedEventHandler BuildFinished;
	public event ProjectStartedEventHandler ProjectStarted;
	public event ProjectFinishedEventHandler ProjectFinished;
	public event TargetStartedEventHandler TargetStarted;
	public event TargetFinishedEventHandler TargetFinished;
	public event TaskStartedEventHandler TaskStarted;
	public event TaskFinishedEventHandler TaskFinished;
	public event CustomBuildEventHandler CustomEventRaised;
	public event BuildStatusEventHandler StatusEventRaised;
	public event AnyEventHandler AnyEventRaised;
}