namespace Immutability;

public struct FileAction
{
	public readonly string FileName;
	public readonly string[] Content;
	public readonly ActionType Type;


	public FileAction(string fileName, ActionType type, string[] content)
	{
		FileName = fileName;
		Content = content;
		Type = type;
	}
}