namespace Immutability.Tests;

public class AuditManagerTests
{
	[Fact]
	public void AddRecord_AddsRecordToExistingFile_IfNotOverflown()
	{
		var manager = new AuditManager(2);
		var file = new FileContent("Audit_1.txt",
			["1;Peter Peterson;2016-04-06T16:30:00",]);

		var action = manager.AddRecord(file,
			"Jane Doe",
			new DateTime(2016, 4, 6, 16, 30, 0)
		);

		Assert.Equal(ActionType.Update, action.Type);
		Assert.Equal("Audit_1.txt", file.FileName);
		Assert.Equal(
		[
			"1;Peter Peterson;2016-04-06T16:30:00",
			"2;Jane Doe;2016-04-06T16:30:00",
		], action.Content);
	}

	[Fact]
	public void AddRecord_AddsRecordToNewFile_IfOverflown()
	{
		var manager = new AuditManager(2);
		var file = new FileContent("Audit_1.txt",
			[
				"1;Peter Peterson;2016-04-06T16:30:00",
				"2;Peter Peterson;2016-04-06T16:35:00"
			]);

		var action = manager.AddRecord(file,
			"Jane Doe",
			new DateTime(2016, 4, 6, 16, 30, 0)
		);

		Assert.Equal(ActionType.Create, action.Type);
		Assert.Equal("Audit_2.txt", action.FileName);
		Assert.Equal([
			"1;Jane Doe;2016-04-06T16:30:00",
		], action.Content);
	}
	
	[Fact]
	public void RemoveMentionsAbout_RemovesMentionsFromAllFiles()
	{
		var manager = new AuditManager(2);
		var files = new[]
		{
			new FileContent("Audit_1.txt",
				[
					"1;Peter Peterson;2016-04-06T16:30:00",
					"2;Jane Doe;2016-04-06T16:30:00"
				]),
			new FileContent("Audit_2.txt",
				[
					"1;Jane Doe;2016-04-06T16:30:00",
					"2;Peter Peterson;2016-04-06T16:35:00"
				])
		};

		var actions = manager.RemoveMentionsAbout("Jane Doe", files);

		Assert.Equal(2, actions.Count);
		Assert.Equal(ActionType.Update, actions[0].Type);
		Assert.Equal("Audit_1.txt", actions[0].FileName);
		Assert.Equal(["1;Peter Peterson;2016-04-06T16:30:00",], actions[0].Content);
		Assert.Equal(ActionType.Update, actions[1].Type);
		Assert.Equal("Audit_2.txt", actions[1].FileName);
		Assert.Equal(["1;Peter Peterson;2016-04-06T16:35:00",], actions[1].Content);
	}

	[Fact]
	public void RemoveMentionsAbout_RemovesFile_IfWasOnlyEntry()
	{
		var manager = new AuditManager(2);
		var file = new FileContent("Audit_1.txt",
		[
			"1;Peter Peterson;2016-04-06T16:30:00"
		]);
		var actions = manager.RemoveMentionsAbout("Peter Peterson", [file]);

		Assert.Equal(1, actions.Count);
		Assert.Equal("Audit_1.txt", actions[0].FileName);
		Assert.Equal(ActionType.Delete, actions[0].Type);
	}
	
	[Fact]
	public void RemoveMentionsAbout_DoesNothing_WhenNoNameMatches()
	{
		var manager = new AuditManager(2);
		var files = new[]
		{
			new FileContent("Audit_1.txt",
			[
				"1;Peter Peterson;2016-04-06T16:30:00",
				"2;John Doe;2016-04-06T16:35:00"
			]),
			new FileContent("Audit_2.txt",
			[
				"1;Alice Smith;2016-04-06T16:40:00",
				"2;Bob Johnson;2016-04-06T16:45:00"
			])
		};

		var actions = manager.RemoveMentionsAbout("Jane Doe", files);
		Assert.Empty(actions);
	}
}