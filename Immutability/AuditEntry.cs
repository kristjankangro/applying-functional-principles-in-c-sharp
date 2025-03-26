namespace Immutability;

public class AuditEntry {
	public readonly int Number;
	public readonly string Visitor;
	public readonly DateTime TimeOfVisit;

	public AuditEntry(int number,string visitorName, DateTime timeOfVisit) {
		Number = number;
		Visitor = visitorName;
		TimeOfVisit = timeOfVisit;
	}
}