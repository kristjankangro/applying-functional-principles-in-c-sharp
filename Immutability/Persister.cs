namespace Immutability;

public class Persister
{
	public FileContent ReadFile(string fileName)
	{
		// Read the file from disk
		return new FileContent(fileName, File.ReadAllLines(fileName));
	}

	public FileContent[] ReadDirectory(string directory)
	{
		// Read all files from the directory
		return Directory
			.GetFiles(directory)
			.Select(ReadFile)
			.ToArray();
	}

	public void ApplyChanges(IReadOnlyList<FileAction> actions)
	{
		foreach (var action in actions)
		{
			switch (action.Type)
			{
				case ActionType.Create:
				case ActionType.Update:
					File.WriteAllLines(action.FileName, action.Content);
					break;
				case ActionType.Delete:
					File.Delete(action.FileName);
					break;
				default:
					throw new InvalidOperationException();
			}
		}
	}

	public void ApplyChanges(FileAction action) =>
		ApplyChanges(new List<FileAction> { action });
}