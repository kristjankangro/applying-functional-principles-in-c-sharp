namespace Immutability;

public class AuditManager
{
	private readonly int _maxEntriesPerFile;

	public AuditManager(int maxEntriesPerFile)
	{
		_maxEntriesPerFile = maxEntriesPerFile;
	}

	public FileAction AddRecord(FileContent currentFile, string visitorName, DateTime timeOfVisit)
	{
		List<AuditEntry> entries = Parse(currentFile.Content);

		if (entries.Count < _maxEntriesPerFile)
		{
			entries.Add(new AuditEntry(entries.Count + 1, visitorName, timeOfVisit));
			var newContent = Serialize(entries);
			return new FileAction(currentFile.FileName, ActionType.Update, newContent);
		}
		else
		{
			var entry = new AuditEntry(1, visitorName, timeOfVisit);
			var newContent = Serialize([entry]);
			var newFileName = GetNewFileName(currentFile.FileName);
			return new FileAction(newFileName, ActionType.Create, newContent);
		}
	}

	private static string[] Serialize(List<AuditEntry> entries) =>
		entries
			.Select(entry => entry.Number + ";" + entry.Visitor + ";" + entry.TimeOfVisit.ToString("s"))
			.ToArray();

	private List<AuditEntry> Parse(string[] content) =>
		content
			.Select(line => line.Split(';'))
			.Select(data =>
				new AuditEntry(int.Parse(data[0]), data[1], DateTime.Parse(data[2])))
			.ToList();

	private string GetNewFileName(string existingFileName)
	{
		string fileName = Path.GetFileNameWithoutExtension(existingFileName);
		int index = int.Parse(fileName.Split('_')[1]);
		return "Audit_" + (index + 1) + ".txt";
	}

	public IReadOnlyList<FileAction> RemoveMentionsAbout(string visitorName, FileContent[] files)
	{
		return files
			.Select(file => RemoveMentionsIn(file, visitorName))
			.Where(a => a != null)
			.Select(a => a.Value)
			.ToList();
	}

	private FileAction? RemoveMentionsIn(FileContent file, string visitorName)
	{
		var entries = Parse(file.Content);

		var newEntries = entries
			.Where(entry => entry.Visitor != visitorName)
			.Select((entry, index) => new AuditEntry(index + 1, entry.Visitor, entry.TimeOfVisit))
			.ToList();

		if (newEntries.Count == entries.Count)
			return null;
		
		return newEntries.Count == 0
			? new FileAction(file.FileName, ActionType.Delete, [])
			: new FileAction(file.FileName, ActionType.Update, Serialize(newEntries));
	}
}