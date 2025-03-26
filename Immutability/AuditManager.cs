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

	public void RemoveMentionsAbout(string visitorName, string directoryName)
	{
		foreach (string fileName in Directory.GetFiles(directoryName))
		{
			string tempFile = Path.GetTempFileName();
			List<string> linesToKeep = File
				.ReadLines(fileName)
				.Where(line => !line.Contains(visitorName))
				.ToList();

			if (linesToKeep.Count == 0)
			{
				File.Delete(fileName);
			}
			else
			{
				File.WriteAllLines(tempFile, linesToKeep);
				File.Delete(fileName);
				File.Move(tempFile, fileName);
			}
		}
	}
}