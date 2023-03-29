using HD_Backend.Data.Entities;

namespace HD_Backend.Data.Interfaces
{
    public interface ITicketRepository
    {
        Task<IEnumerable<Ticket>> GetAllTickets(bool trackChanges);
        Task<Ticket> GetTicket(long ticketId, long departmentId, long facultyId, bool trackChanges);

        Task CreateTicket(Ticket ticket, long departmentId, long facultyId);

        Task DeleteTicket(Ticket ticket);
    }
}
