using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exceptions
{
    internal class DbUpdateException : Exception
    {
    }


    internal class MyContext : IDisposable
    {
        public void SaveChanges()
        {
            throw new NotImplementedException();
        }


        public IList<Customer> Customers { get; set; }
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }


    internal class Customer
    {
        public Customer(string name)
        {
            throw new NotImplementedException();
        }


        public int Id { get; set; }
    }

    public class Ticket
    {
        public Ticket(DateTime date, string customerName)
        {
        }
    }


    public class TicketRepository
    {
        public void Save(Ticket ticket)
        {
            throw new NotImplementedException();
        }
    }

    public class Controller
    {
        protected ActionResult View(string error, string message = null)
        {
            return null;
        }
    }

    public class ActionResult
    {
    }

    public class HttpPostAttribute : Attribute
    {
    }
}
