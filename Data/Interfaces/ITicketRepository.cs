using HD_Backend.Data.Entities;

namespace HD_Backend.Data.Interfaces
{
    public interface ITicketRepository
    {
        Task<IEnumerable<Ticket>> GetAllTickets(bool trackChanges);
        Task<Ticket> GetTicket(long ticketId, bool trackChanges);

        Task CreateTicket(Ticket ticket);

        Task<Ticket?> GetTicketByCode(string code, bool trackChanges);

        Task DeleteTicket(Ticket ticket);
    }
}
