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
	es.RaiseWarning(message: "one");
	var engine = new Engine();
	engine.RegisterLogger(logger);
	es.RaiseTargetStarted(targetName: "some target+", message: "raise targetstarted");
	es.RaiseTaskStarted(targetName: "some task+", message: "raise taskstarted");
	es.RaiseWarning(message:"two");	
	es.RaiseTaskFinished(targetName: "some task-", message: "raise taskfinished");
	es.RaiseTargetFinished(targetName: "some target-", message: "raise targetfinished");
	var project = new Project(engine);
	project.Load("LPRun.sln");
	es.RaiseWarning(message:"three");	
	
	project.Build();
	es.RaiseWarning(message:"four - not run as msbuild must have shut down the logger");	
	
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

		public void RaiseMessage(BuildMessageEventArgs e)
		{
			BuildMessageEventHandler handler = MessageRaised;
			if (handler != null) handler(this, e);
		}

		public event BuildErrorEventHandler ErrorRaised;

		public void RaiseError(BuildErrorEventArgs e)
		{
			BuildErrorEventHandler handler = ErrorRaised;
			if (handler != null) handler(this, e);
		}

		public event BuildWarningEventHandler WarningRaised;

		public void RaiseWarning(BuildWarningEventArgs e)
		{
			BuildWarningEventHandler handler = WarningRaised;
			if (handler != null) handler(this, e);
		}

		public event BuildStartedEventHandler BuildStarted;

		public void RaiseBuildStarted(BuildStartedEventArgs e)
		{
			BuildStartedEventHandler handler = BuildStarted;
			if (handler != null) handler(this, e);
		}

		public event BuildFinishedEventHandler BuildFinished;

		public void RaiseBuildFinished(BuildFinishedEventArgs e)
		{
			BuildFinishedEventHandler handler = BuildFinished;
			if (handler != null) handler(this, e);
		}

		public event ProjectStartedEventHandler ProjectStarted;

		public void RaiseProjectStarted(ProjectStartedEventArgs e)
		{
			ProjectStartedEventHandler handler = ProjectStarted;
			if (handler != null) handler(this, e);
		}

		public event ProjectFinishedEventHandler ProjectFinished;

		public void RaiseProjectFinished(ProjectFinishedEventArgs e)
		{
			ProjectFinishedEventHandler handler = ProjectFinished;
			if (handler != null) handler(this, e);
		}

		public event TargetStartedEventHandler TargetStarted;

		public void RaiseTargetStarted(TargetStartedEventArgs e)
		{
			TargetStartedEventHandler handler = TargetStarted;
			if (handler != null) handler(this, e);
		}

		public event TargetFinishedEventHandler TargetFinished;

		public void RaiseTargetFinished(TargetFinishedEventArgs e)
		{
			TargetFinishedEventHandler handler = TargetFinished;
			if (handler != null) handler(this, e);
		}

		public event TaskStartedEventHandler TaskStarted;

		public void RaiseTaskStarted(TaskStartedEventArgs e)
		{
			TaskStartedEventHandler handler = TaskStarted;
			if (handler != null) handler(this, e);
		}

		public event TaskFinishedEventHandler TaskFinished;

		public void RaiseTaskFinished(TaskFinishedEventArgs e)
		{
			TaskFinishedEventHandler handler = TaskFinished;
			if (handler != null) handler(this, e);
		}

		public event CustomBuildEventHandler CustomEventRaised;

		public void RaiseCustomEvent(CustomBuildEventArgs e)
		{
			CustomBuildEventHandler handler = CustomEventRaised;
			if (handler != null) handler(this, e);
		}

		public event BuildStatusEventHandler StatusEventRaised;

		public void RaiseStatusEvent(BuildStatusEventArgs e)
		{
			BuildStatusEventHandler handler = StatusEventRaised;
			if (handler != null) handler(this, e);
		}

		public event AnyEventHandler AnyEventRaised;

		public void RaiseAnyEvent(BuildEventArgs e)
		{
			AnyEventHandler handler = AnyEventRaised;
			if (handler != null) handler(this, e);
		}
		public void RaiseMessage(String message = default(String), String helpKeyword = default(String), String senderName = default(String), MessageImportance importance = default(MessageImportance), DateTime eventTimestamp = default(DateTime), Object[] messageArgs = default(Object[]))
		{
			var arg = new BuildMessageEventArgs(message, helpKeyword, senderName, importance, eventTimestamp, messageArgs);
			RaiseMessage(arg);
		}
		public void RaiseError(String subcategory = default(String), String code = default(String), String file = default(String), Int32 lineNumber = default(Int32), Int32 columnNumber = default(Int32), Int32 endLineNumber = default(Int32), Int32 endColumnNumber = default(Int32), String message = default(String), String helpKeyword = default(String), String senderName = default(String), DateTime eventTimestamp = default(DateTime), Object[] messageArgs = default(Object[]))
		{
			var arg = new BuildErrorEventArgs(subcategory, code, file, lineNumber, columnNumber, endLineNumber, endColumnNumber, message, helpKeyword, senderName, eventTimestamp, messageArgs);
			RaiseError(arg);
		}
		public void RaiseWarning(String subcategory = default(String), String code = default(String), String file = default(String), Int32 lineNumber = default(Int32), Int32 columnNumber = default(Int32), Int32 endLineNumber = default(Int32), Int32 endColumnNumber = default(Int32), String message = default(String), String helpKeyword = default(String), String senderName = default(String), DateTime eventTimestamp = default(DateTime), Object[] messageArgs = default(Object[]))
		{
			var arg = new BuildWarningEventArgs(subcategory, code, file, lineNumber, columnNumber, endLineNumber, endColumnNumber, message, helpKeyword, senderName, eventTimestamp, messageArgs);
			RaiseWarning(arg);
		}
		public void RaiseBuildStarted(String message = default(String), String helpKeyword = default(String), DateTime eventTimestamp = default(DateTime), Object[] messageArgs = default(Object[]))
		{
			var arg = new BuildStartedEventArgs(message, helpKeyword, eventTimestamp, messageArgs);
			RaiseBuildStarted(arg);
		}
		public void RaiseBuildFinished(String message = default(String), String helpKeyword = default(String), Boolean succeeded = default(Boolean), DateTime eventTimestamp = default(DateTime), Object[] messageArgs = default(Object[]))
		{
			var arg = new BuildFinishedEventArgs(message, helpKeyword, succeeded, eventTimestamp, messageArgs);
			RaiseBuildFinished(arg);
		}
		public void RaiseProjectStarted(Int32 projectId = default(Int32), String message = default(String), String helpKeyword = default(String), String projectFile = default(String), String targetNames = default(String), IEnumerable properties = default(IEnumerable), IEnumerable items = default(IEnumerable), BuildEventContext parentBuildEventContext = default(BuildEventContext), DateTime eventTimestamp = default(DateTime))
		{
			var arg = new ProjectStartedEventArgs(projectId, message, helpKeyword, projectFile, targetNames, properties, items, parentBuildEventContext, eventTimestamp);
			RaiseProjectStarted(arg);
		}
		public void RaiseProjectFinished(String message = default(String), String helpKeyword = default(String), String projectFile = default(String), Boolean succeeded = default(Boolean), DateTime eventTimestamp = default(DateTime))
		{
			var arg = new ProjectFinishedEventArgs(message, helpKeyword, projectFile, succeeded, eventTimestamp);
			RaiseProjectFinished(arg);
		}
		public void RaiseTargetStarted(String message = default(String), String helpKeyword = default(String), String targetName = default(String), String projectFile = default(String), String targetFile = default(String), String parentTarget = default(String), DateTime eventTimestamp = default(DateTime))
		{
			var arg = new TargetStartedEventArgs(message, helpKeyword, targetName, projectFile, targetFile, parentTarget, eventTimestamp);
			RaiseTargetStarted(arg);
		}
		public void RaiseTargetFinished(String message = default(String), String helpKeyword = default(String), String targetName = default(String), String projectFile = default(String), String targetFile = default(String), Boolean succeeded = default(Boolean), DateTime eventTimestamp = default(DateTime), IEnumerable targetOutputs = default(IEnumerable))
		{
			var arg = new TargetFinishedEventArgs(message, helpKeyword, targetName, projectFile, targetFile, succeeded, eventTimestamp, targetOutputs);
			RaiseTargetFinished(arg);
		}
		public void RaiseTaskStarted(String message = default(String), String helpKeyword = default(String), String projectFile = default(String), String taskFile = default(String), String taskName = default(String), DateTime eventTimestamp = default(DateTime))
		{
			var arg = new TaskStartedEventArgs(message, helpKeyword, projectFile, taskFile, taskName, eventTimestamp);
			RaiseTaskStarted(arg);
		}
		public void RaiseTaskFinished(String message = default(String), String helpKeyword = default(String), String projectFile = default(String), String taskFile = default(String), String taskName = default(String), Boolean succeeded = default(Boolean), DateTime eventTimestamp = default(DateTime))
		{
			var arg = new TaskFinishedEventArgs(message, helpKeyword, projectFile, taskFile, taskName, succeeded, eventTimestamp);
			RaiseTaskFinished(arg);
		}
	}