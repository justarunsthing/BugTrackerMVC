using System.ComponentModel;

namespace BugTrackerMVC.Models
{
    public class TicketType
    {
        public int Id { get; set; }

        [DisplayName("Ticket Type")]
        public string Name { get; set; }
    }
}