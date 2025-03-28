namespace Exceptions
{
    public class TicketController : Controller
    {
        private readonly TicketRepository _repository;
        private readonly TheaterGateway _gateway;

        public TicketController(TicketRepository repository, TheaterGateway gateway)
        {
            _gateway = gateway;
            _repository = repository;
        }

        [HttpPost]
        public ActionResult BuyTicket(DateTime date, string customerName)
        {
            var validationResult = Validate(date, customerName);
            if (validationResult.IsFailure)
                return View("Error", validationResult.Error);

            var reserveResult = _gateway.Reserve(date, customerName);
            if (reserveResult.IsFailure)
                return View("Error", reserveResult.Error);

            var ticket = new Ticket(date, customerName);
            _repository.Save(ticket);
            
            return View("Success");
        }

        private Result Validate(DateTime date, string customerName)
        {
            if (date.Date < DateTime.Now.Date)
                return Result.Fail("Cannot reserve on a past date");

            if (string.IsNullOrWhiteSpace(customerName) || customerName.Length > 200)
                return Result.Fail("Incorrect customer name");

            return Result.Ok();
        }
    }
}
