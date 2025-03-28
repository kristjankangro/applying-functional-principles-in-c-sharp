namespace Exceptions;

public class TheaterGateway
{
	public Result Reserve(DateTime date, string customerName)
	{
		try
		{
			var client = new TheaterApiClient();
			client.Reserve(date, customerName);

			return Result.Ok();
		}
		catch (HttpRequestException)
		{
			return Result.Fail("Unable to connect to the theater. Please try again later.");
		}
		catch (InvalidOperationException)
		{
			return Result.Fail("Sorry, tickets on this date are no longer available.");
		}
	}
}