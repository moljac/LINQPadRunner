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

	//Set up logger from command line a
	ILogger logger = GetLogger(args) ?? new ConsoleLogger();
	var es = new ManualEventSource();
	logger.Initialize(es);
	es.RaiseWarning("one");
	var engine = new Engine();
	engine.RegisterLogger(logger);
	es.RaiseWarning("two");	
	var project = new Project(engine);
	project.Load("LPRun.sln");
	es.RaiseWarning("three");	
	
	project.Build();
	es.RaiseWarning("four");	
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

 
public class ManualEventSource : IEventSource
{
	public event BuildMessageEventHandler MessageRaised;
	public event BuildErrorEventHandler ErrorRaised;

	public void RaiseError(string message, string subcategory = null, string code = null, string file = null, int endColumnNumber = 0, int endLineNumber = 0, int columnNumber = 0, int number = 0, string helpKeyword = null, string senderName = null)
	{
		var args = new BuildErrorEventArgs(subcategory, code, file, endColumnNumber, endLineNumber, columnNumber, number, message, helpKeyword, senderName);
		RaiseError(args);
	}
	private void RaiseError(BuildErrorEventArgs e)
	{
		BuildErrorEventHandler handler = ErrorRaised;
		if (handler != null) handler(this, e);
	}

	public event BuildWarningEventHandler WarningRaised;

	public void RaiseWarning(string message, string subcategory = null, string code = null, string file = null, int endColumnNumber = 0, int endLineNumber = 0, int columnNumber = 0, int number = 0, string helpKeyword = null, string senderName = null)
	{
		var args = new BuildWarningEventArgs(subcategory, code, file, endColumnNumber, endLineNumber, columnNumber, number, message, helpKeyword, senderName);
		RaiseWarning(args);
	}
	private void RaiseWarning(BuildWarningEventArgs e)
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