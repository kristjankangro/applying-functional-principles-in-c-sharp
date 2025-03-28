namespace Immutability;

public class ApplicationService
{
	private readonly string _directoryName;
	private readonly AuditManager auditManager;
	private readonly Persister persister;

	public ApplicationService(string directoryName)
	{
		_directoryName = directoryName;
	}
	
	public void RemoveMentionsAbout(string visitorName)
	{
		var files = persister.ReadDirectory(_directoryName);
		var actions = auditManager.RemoveMentionsAbout(visitorName, files);
		persister.ApplyChanges(actions);
	}
	
	public void AddRecord(string visitorName, DateTime date)
	{
		FileInfo file = new DirectoryInfo(_directoryName)
			.GetFiles()
			.OrderByDescending(x => x.LastWriteTime)
			.First();
		
		var fileContent = persister.ReadFile(file.Name);
		var action = auditManager.AddRecord(fileContent, visitorName, date);
		
		persister.ApplyChanges(action);
	}
}