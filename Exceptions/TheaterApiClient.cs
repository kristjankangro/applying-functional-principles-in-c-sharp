namespace Exceptions;

public class TheaterApiClient
{
	/// <summary>
	/// Throws:
	/// HttpRequestException if unable to connect to the API;
	/// InvalidOperationException if no tickets are available
	/// </summary>
	public void Reserve(DateTime date, string customerName)
	{
		throw new NotImplementedException();
	}
}